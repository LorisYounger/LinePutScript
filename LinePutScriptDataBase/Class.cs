using System.IO;
using System.Text;
using System.IO.MemoryMappedFiles;
using System.Threading;
using System.Collections.Generic;

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
            LPS = new LpsDocument($"DataBase#{name}:|ver#1:|capacity#10240:|automapping#1:|");
        }
        public DataBase(string name, string lps)
        {
            Name = name;
            LPS = new LpsDocument(lps);
            if (LPS.First() == null || LPS.First().Name.ToLower() != "database")
                LPS.InsertLine(0, new Line($"DataBase#{name}:|ver#1:|capacity#10240:|automapping#1:|"));
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
                LPS.InsertLine(0, new Line($"DataBase#{name}:|ver#1:|capacity#10240:|automapping#1:|"));

        }

        ///// <summary>
        ///// 映射到内存的数据库
        ///// </summary>
        //public DataBaseLocker DBL;


        //设置内容

        /// <summary>
        /// 分配给每行的内存容量
        /// </summary>
        public int LineCapacity
        {
            get
            {
                Sub sb = LPS.First().Find("capacity");
                if (sb == null || !int.TryParse(sb.info, out int ot))
                {
                    LPS.First().AddSub(new Sub("capacity", "10240"));
                    return 10240;//默认分配10kb
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
        /// 提前准备多少行
        /// </summary>
        public int LinePrepare
        {
            get
            {
                Sub sb = LPS.First().Find("prepare");
                if (sb == null || !int.TryParse(sb.info, out int ot))
                {
                    LPS.First().AddSub(new Sub("prepare", "1000"));
                    return 1000;//默认分配1000行
                }
                return ot;
            }
            set
            {
                Sub sb = LPS.First().Find("prepare");
                if (sb == null)
                {
                    LPS.First().AddSub(new Sub("prepare", value.ToString()));
                }
                else
                    sb.info = value.ToString();
            }
        }

        /// <summary>
        /// 获取使用了多少行
        /// </summary>
        public int UsePrepare()
        {
            if (mapping)
            {
                MemoryMappedFile mmf = MemoryMappedFile.OpenExisting("lpsdb" + Name);
                MemoryMappedViewAccessor mmva = mmf.CreateViewAccessor();
                int len = mmva.ReadInt32(0);
                char[] buff = new char[len];
                mmva.ReadArray<char>(4, buff, 0, len);
                mmva.Dispose();
                mmf.Dispose();
                return new Line(new string(buff)).InfoToInt;
            }
            return LPS.Assemblage.Count;
        }

        /// <summary>
        /// 获取文件大小
        /// </summary>
        public int Length
        {
            get => LPS.Length * 20 / LPS.Assemblage.Count;//保险起见用*40进行计算
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

        public List<MemoryMappedFile> MMFS = new List<MemoryMappedFile>();

        /// <summary>
        /// 映射数据库到内存中
        /// </summary>
        public void MaptoMemory()
        {
            if (mapping)
                return;
            int ca = LineCapacity; int cp = LinePrepare;
            if (Length > ca || LPS.Assemblage.Count * 2 > cp)//如果空间不足,不进行映射
                return;

            //mmf不需要回收
            string tmp; MemoryMappedFile mmf; MemoryMappedViewAccessor mmva;

            Line index = new Line("lpsdb" + Name);
            //index: ilpsdbdbname#{递增id}:|line名字#id{被用在mmf}:|.... 其中这个sub顺序决定最终储存的位置
            foreach (Line li in LPS)
            {
                tmp = li.ToString();
                mmf = MemoryMappedFile.CreateOrOpen("lpsdb" + Name + index.Subs.Count.ToString(), ca, MemoryMappedFileAccess.ReadWrite);
                mmva = mmf.CreateViewAccessor();
                mmva.Write(0, tmp.Length);
                mmva.WriteArray(4, tmp.ToCharArray(), 0, tmp.Length);
                index.AddSub(new Sub(li.Name, index.Subs.Count.ToString()));
                mmva.Dispose();
                MMFS.Add(mmf);
            }

            //创建预留的缓存空间
            int cs = index.Subs.Count;
            index.info = cs.ToString();

            for (int i = cs; i <= cp; i++)
            {
                mmf = MemoryMappedFile.CreateOrOpen("lpsdb" + Name + index.Subs.Count.ToString(), ca, MemoryMappedFileAccess.ReadWrite);
                MMFS.Add(mmf);
            }



            mmf = MemoryMappedFile.CreateOrOpen("lpsdb" + Name, ca, MemoryMappedFileAccess.ReadWrite);
            mmva = mmf.CreateViewAccessor();
            tmp = index.ToString();
            mmva.Write(0, tmp.Length);
            mmva.WriteArray(4, tmp.ToCharArray(), 0, tmp.Length);
            mmva.Dispose();

            MMFS.Add(mmf);

            mapping = true;
        }
        /// <summary>
        /// 关闭映射数据库
        /// </summary>
        public void Mapoff()
        {
            if (!mapping)
                return;
            //首先获得index确认位置
            MemoryMappedFile mmf = MemoryMappedFile.OpenExisting("lpsdb" + Name);
            MemoryMappedViewAccessor mmva = mmf.CreateViewAccessor();
            int len = mmva.ReadInt32(0);
            char[] buff = new char[len];
            mmva.ReadArray<char>(4, buff, 0, len);
            mmva.Dispose();

            mmf.Dispose();//关掉mmf进行回收
            Line index = new Line(new string(buff));

            //通过index获取数据
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

            //回收内存
            foreach (MemoryMappedFile mmff in MMFS)
                mmff.Dispose();
            MMFS.Clear();

            mapping = false;
        }
        /// <summary>
        /// 将映射在数据库的内存 储存回LPS
        /// </summary>
        public void SaveMemory()
        {
            if (!mapping)
                return;
            //首先获得index确认位置
            MemoryMappedFile mmf = MemoryMappedFile.OpenExisting("lpsdb" + Name);
            MemoryMappedViewAccessor mmva = mmf.CreateViewAccessor();
            int len = mmva.ReadInt32(0);
            char[] buff = new char[len];
            mmva.ReadArray<char>(4, buff, 0, len);
            mmva.Dispose();
            mmf.Dispose();
            Line index = new Line(new string(buff));

            //通过index获取数据
            LPS.Assemblage.Clear();//清除内部数据准备填装
            for (int i = 0; i < index.Subs.Count; i++)
            {
                mmf = MemoryMappedFile.OpenExisting("lpsdb" + Name + index.Subs[i].info);
                mmva = mmf.CreateViewAccessor();
                len = mmva.ReadInt32(0);
                buff = new char[len];
                mmva.ReadArray<char>(4, buff, 0, len);
                mmva.Dispose();
                mmf.Dispose();
                LPS.Assemblage.Add(new Line(new string(buff)));
            }
        }
    }





    ///// <summary>
    ///// 映射在内存的数据库
    ///// </summary>
    //public struct DataBaseLocker
    //{
    //    private DataBase DB;
    //    /// <summary>
    //    /// 获取储存在其中的DataBase 密码错误则退回null
    //    /// </summary>
    //    /// <param name="password">密码,未设置则为空即可</param>
    //    /// <returns></returns>
    //    public DataBase GetDataBase(string password = "")
    //    {
    //        if (DB == null)
    //            return null;
    //        if (password == DB.Password)
    //            return DB;
    //        return null;
    //    }
    //    public DataBaseLocker(DataBase db)
    //    {
    //        DB = db;
    //    }
    //}

}
