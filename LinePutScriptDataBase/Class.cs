using System.IO;
using System.Text;
using System.IO.MemoryMappedFiles;
using System.Threading;

namespace LinePutScript.DataBase
{
    /// <summary>
    /// 数据库
    /// </summary>
    public class DataBase
    {
        /// <summary>
        /// 库名
        /// </summary>
        public string Name;
        /// <summary>
        /// 数据文件
        /// </summary>
        public LpsDocument LPS;
        public DataBase(string name)
        {
            Name = name;
            LPS = new LpsDocument($"DataBase#{name}:|ver#1:|capacity#1048576:|automapping#1:|");
        }
        public DataBase(string name, string lps)
        {
            Name = name;
            LPS = new LpsDocument(lps);
            if (LPS.First() == null || LPS.First().Name.ToLower() != "database")
                LPS.AddLine(new Line($"DataBase#{name}:|ver#1:|capacity#1048576:|automapping#1:|"));
        }
        public DataBase(string name, FileInfo lpsdb)
        {
            Name = name;
            //强制UTF-8
            StreamReader sr = new StreamReader(lpsdb.OpenRead(), Encoding.UTF8);
            string lps = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();
            LPS = new LpsDocument(lps);
            if (LPS.First() == null || LPS.First().Name.ToLower() != "database")
                LPS.AddLine(new Line($"DataBase#{name}:|ver#1:|capacity#1048576:|automapping#1:|"));

        }

        ///// <summary>
        ///// 映射到内存的数据库
        ///// </summary>
        //public DataBaseLocker DBL;


        //设置内容

        /// <summary>
        /// 分配的内存容量
        /// </summary>
        public long Capacity
        {
            get
            {
                Sub sb = LPS.First().Find("capacity");
                if (sb == null || !long.TryParse(sb.info, out long ot))
                {
                    LPS.First().AddSub(new Sub("capacity", "1048576"));
                    return 1048576;//默认分配1mb
                }
                return ot;
            }
            set
            {
                Sub sb = LPS.First().Find("capacity");
                if (sb == null)
                {
                    LPS.First().AddSub(new Sub("capacity", value.ToString()));
                }
                else
                    sb.info = value.ToString();
            }
        }

        /// <summary>
        /// 获取文件大小
        /// </summary>
        public int Length
        {
            get => LPS.Length * 4;//保险起见用*4进行计算
        }

        /// <summary>
        /// 是否自动部署
        /// </summary>
        public bool AutoMapping
        {
            get
            {
                Sub sb = LPS.First().Find("automapping");
                if (sb == null)
                {
                    LPS.First().AddSub(new Sub("automapping", "1"));
                    return true;//默认自动部署
                }
                return sb.info != "0";
            }
            set
            {
                Sub sb = LPS.First().Find("automapping");
                if (sb == null)
                {
                    LPS.First().AddSub(new Sub("capacity", (value ? "1" : "0")));
                }
                else
                    sb.info = (value ? "1" : "0");
            }
        }
        /// <summary>
        /// 是否自动备份
        /// </summary>
        public bool AutoBackup
        {
            get
            {
                Sub sb = LPS.First().Find("backup");
                if (sb == null)
                {
                    LPS.First().AddSub(new Sub("backup", "1"));
                    return true;//默认自动备份
                }
                return sb.info != "0";
            }
            set
            {
                Sub sb = LPS.First().Find("backup");
                if (sb == null)
                {
                    LPS.First().AddSub(new Sub("backup", (value ? "1" : "0")));
                }
                else
                    sb.info = (value ? "1" : "0");
            }
        }
        /// <summary>
        /// 读取密码密码,为空则未设置
        /// </summary>
        public string Password
        {
            get
            {
                Sub sb = LPS.First().Find("password");
                if (sb == null)
                {
                    return "";//默认未设置
                }
                return sb.Info;
            }
            set
            {
                Sub sb = LPS.First().Find("password");
                if (sb == null)
                {
                    LPS.First().AddSub(new Sub("capacity", value));
                }
                else
                    sb.Info = value;
            }
        }
        //映射内容

        private bool mapping = false;
        /// <summary>
        /// 指示是否映射成功
        /// </summary>
        public bool Mapping
        {
            get => mapping;
        }


        /// <summary>
        /// 映射数据库到内存中
        /// </summary>
        public void MaptoMemory()
        {
            if (mapping)
                return;
            if (Length > Capacity)//如果空间不足,不进行映射
                return;


            //mmf不需要回收
            string tmp; MemoryMappedFile mmf; MemoryMappedViewAccessor mmva;
            Line index = new Line("lpsdb" + Name);
            //index: ilpsdbdbname#{递增id}:|line名字#id{被用在mmf}:|.... 其中这个sub顺序决定最终储存的位置
            foreach (Line li in LPS)
            {                
                tmp = li.ToString();
                mmf = MemoryMappedFile.CreateOrOpen("lpsdb" + Name + index.Subs.Count.ToString(), Capacity, MemoryMappedFileAccess.ReadWrite);
                mmva = mmf.CreateViewAccessor();
                mmva.Write(0, tmp.Length);
                mmva.WriteArray(4, tmp.ToCharArray(), 0, tmp.Length);
                index.AddSub(new Sub(li.Name, index.Subs.Count.ToString()));
                mmva.Dispose();
            }

            index.info = index.Subs.Count.ToString();

            mmf = MemoryMappedFile.CreateOrOpen("lpsdb" + Name, Capacity, MemoryMappedFileAccess.ReadWrite);
            mmva = mmf.CreateViewAccessor();
            tmp = index.ToString();
            mmva.Write(0, tmp.Length);
            mmva.WriteArray(4, tmp.ToCharArray(), 0, tmp.Length);
            mmva.Dispose();

            

            mapping = true;
        }
        /// <summary>
        /// 关闭映射数据库
        /// </summary>
        public void Mapoff()
        {
            if (!mapping)
                return;
            MemoryMappedFile mmf = MemoryMappedFile.OpenExisting("lpsdb" + Name);
            MemoryMappedViewAccessor mmva = mmf.CreateViewAccessor();
            int len = mmva.ReadInt32(0);
            char[] buff = new char[len];
            mmva.ReadArray<char>(4, buff, 0, len);
            mmva.Dispose();
            
            mmf.Dispose();//关掉mmf进行回收
            Line index = new Line(new string(buff));

            LPS.Assemblage.Clear();//清除内部数据准备填装
            for (int i = 0; i < index.Subs.Count; i++)
            {
                mmf = MemoryMappedFile.OpenExisting("lpsdb" + Name + index.Subs[i].info);
                mmva = mmf.CreateViewAccessor();
                len = mmva.ReadInt32(0);
                buff = new char[len];
                mmva.ReadArray<char>(4, buff, 0, len);
                mmva.Dispose();
                mmf.Dispose();//关掉mmf进行回收
                LPS.Assemblage.Add(new Line(new string(buff)));
            }
            mapping = false;
        }

    }

    



    /// <summary>
    /// 映射在内存的数据库
    /// </summary>
    public struct DataBaseLocker
    {
        private DataBase DB;
        /// <summary>
        /// 获取储存在其中的DataBase 密码错误则退回null
        /// </summary>
        /// <param name="password">密码,未设置则为空即可</param>
        /// <returns></returns>
        public DataBase GetDataBase(string password = "")
        {
            if (DB == null)
                return null;
            if (password == DB.Password)
                return DB;
            return null;
        }
        public DataBaseLocker(DataBase db)
        {
            DB = db;
        }
    }

}
