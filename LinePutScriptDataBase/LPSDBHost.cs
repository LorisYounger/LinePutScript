using LinePutScript;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LinePutScript.DataBase
{
    /// <summary>
    /// 主机
    /// </summary>
    public class LPSDBHost
    {
        /// <summary>
        /// 主机数据库位置
        /// </summary>
        public readonly string PathMain;
        /// <summary>
        /// 数据库数据
        /// </summary>
        public List<DataBase> DataBases = new List<DataBase>();

        /// <summary>
        /// 配置
        /// </summary>
        public Config CONFIG;

        /// <summary>
        /// 备份和储存共用的timer
        /// </summary>
        public System.Timers.Timer timer;
        private int backuptime;

        /// <summary>
        /// 从路径启动数据库主机
        /// </summary>
        /// <param name="path">路径:数据库储存位置</param>
        public LPSDBHost(string path)
        {
            PathMain = path;

            //从文件导入数据库
            DirectoryInfo di = new DirectoryInfo(path);
            foreach (FileInfo fi in di.EnumerateFiles().Where(x => x.Extension.ToLower() == ".lpsdb"))
            {
                DataBases.Add(new DataBase(fi.Name.Substring(0, fi.Name.Length - 6).ToUpper(), fi));
            }
            //获取配置文件
            DataBase tmp = DataBases.Find(x => x.Name == "CONFIG");
            if (tmp == null)
            {
                CONFIG = new Config();
                DataBases.Add(CONFIG.DB);
            }
            else
                CONFIG = new Config(tmp);

            if (CONFIG.AutoMapping)
                MaptoMemoryAuto();

            timer = new System.Timers.Timer();
            timer.Elapsed += Timer_Elapsed;
            timer.Interval = CONFIG.AutoSaveTime;
            timer.AutoReset = true;
            timer.Start();
            backuptime = CONFIG.AutoBackupTime;
        }

        /// <summary>
        /// 自动备份和自动储存计时器
        /// </summary>
        public void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            SaveALL();
            backuptime -= CONFIG.AutoSaveTime;
            if (backuptime <= 0)
            {
                backuptime = CONFIG.AutoBackupTime;
                BackUpAuto();
            }
        }

        /// <summary>
        /// 映射全部自动启动数据库
        /// </summary>
        public void MaptoMemoryAuto()
        {
            foreach (DataBase db in DataBases.FindAll(x => x.AutoMapping))
            {
                db.MaptoMemory();
            }
        }
        /// <summary>
        /// 映射全部数据库
        /// </summary>
        public void MaptoMemoryALL()
        {
            foreach (DataBase db in DataBases)
            {
                db.MaptoMemory();
            }
        }
        /// <summary>
        /// 关闭映射全部数据库
        /// </summary>
        public void MapOffALL()
        {
            foreach (DataBase db in DataBases)
            {
                db.Mapoff();
            }
        }

        /// <summary>
        /// 进行备份操作 注意:如果在备份前未保存,需要先使用 SaveMemory 将内存数据拉回本地
        /// </summary>
        public void BackUp(DataBase db)
        {
            DirectoryInfo di = new DirectoryInfo(PathMain + '\\' + db.Name);
            if (!di.Exists)
            {
                di.Create();//如果目录不存在则创建
            }
            FileInfo fi = new FileInfo(PathMain + '\\' + db.Name + '\\' + DateTime.Now.ToString("yyMMddhhmm") + ".lpsdb");
            FileStream fs = fi.Create();
            byte[] buff = Encoding.UTF8.GetBytes(db.LPS.ToString());
            fs.Write(buff, 0, buff.Length);
            fs.Close();
            fs.Dispose();
        }
        /// <summary>
        /// 备份全部自动备份数据库
        /// </summary>
        public void BackUpAuto()
        {
            foreach (DataBase db in DataBases.FindAll(x => x.AutoBackup))
            {
                BackUp(db);
            }
        }
        /// <summary>
        /// 进行存档操作
        /// </summary>
        public void Save(DataBase db)
        {
            db.SaveMemory();
            FileInfo fi = new FileInfo(PathMain + '\\' + db.Name + ".lpsdb");
            FileStream fs = fi.Create();
            byte[] buff = Encoding.UTF8.GetBytes(db.LPS.ToString());
            fs.Write(buff, 0, buff.Length);
            fs.Close();
            fs.Dispose();
        }
        /// <summary>
        /// 进行关闭操作
        /// </summary>
        public void Close(DataBase db)
        {
            db.Mapoff();
            FileInfo fi = new FileInfo(PathMain + '\\' + db.Name + ".lpsdb");
            FileStream fs = fi.Create();
            byte[] buff = Encoding.UTF8.GetBytes(db.LPS.ToString());
            fs.Write(buff, 0, buff.Length);
            fs.Close();
            fs.Dispose();
        }
        /// <summary>
        /// 存档全部数据库
        /// </summary>
        public void SaveALL()
        {
            foreach (DataBase db in DataBases)
            {
                Save(db);
            }
        }
        /// <summary>
        /// 退出全部数据库
        /// </summary>
        public void CloseALL()
        {
            foreach (DataBase db in DataBases)
            {
                Close(db);
            }
        }
        /// <summary>
        /// 修改数据库名
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="NewName">新名称:仅大写+下划线</param>
        public void ReName(DataBase db, string NewName)
        {
            NewName = NewName.ToUpper();
            if (db.Mapping)
                return;
            //首先先改文件
            FileInfo fi = new FileInfo(PathMain + '\\' + db.Name + ".lpsdb");
            fi.MoveTo(fi.DirectoryName + '\\' + NewName + ".lpsdb");
            DirectoryInfo di = new DirectoryInfo(PathMain + '\\' + db.Name);
            if (di.Exists)
            {
                di.MoveTo(PathMain + '\\' + NewName);
            }
            //然后改文件名
            db.Name = NewName;
            db.LPS.First().First().info = NewName;
        }
    }

    /// <summary>
    /// 配置文件
    /// </summary>
    public class Config
    {
        /// <summary>
        /// 配置文件RAW
        /// </summary>
        public DataBase DB;

        /// <summary>
        /// 无配置文件时默认设置
        /// </summary>
        public Config()
        {
            DB = new DataBase("CONFIG");
            DB.LineCapacity = 1024;//默认容量 1k(如果不映射 0k也是可以的)
            DB.LinePrepare = 50;//默认预备行 50
            DB.AutoBackup = false;//不自动备份配置文件
            DB.AutoMapping = false;//不自动映射配置文件
        }
        /// <summary>
        /// 有配置文件
        /// </summary>
        public Config(DataBase db)
        {
            DB = db;
        }
        /// <summary>
        /// 是否启动后自动部署
        /// </summary>
        public bool AutoMapping
        {
            get
            {
                Line li = DB.LPS.FindLine("automapping");
                if (li == null)
                {
                    DB.LPS.AddLine(new Line("automapping", "1"));
                    return true;//默认自动部署
                }
                return li.info != "0";
            }
            set
            {
                Line li = DB.LPS.FindLine("automapping");
                if (li == null)
                {
                    DB.LPS.AddLine(new Line("automapping", (value ? "1" : "0")));
                }
                else
                    li.info = (value ? "1" : "0");
            }
        }

        /// <summary>
        /// 自动储存时间间隔 单位:毫秒
        /// </summary>
        public int AutoSaveTime
        {
            get
            {
                Line li = DB.LPS.FindLine("autosavetime");
                if (li == null || !int.TryParse(li.info, out int ot))
                {
                    DB.LPS.AddLine(new Line("autosavetime", "3600000"));
                    return 3600000;//默认1小时一次刷新
                }
                return ot;
            }
            set
            {
                if (value < 1000)
                    value = 1000;//最小值:1秒
                Line li = DB.LPS.FindLine("autosavetime");
                if (li == null)
                {
                    DB.LPS.AddLine(new Line("autosavetime", value.ToString()));
                }
                else
                    li.info = value.ToString();
            }
        }
        /// <summary>
        /// 自动备份时间间隔 单位:毫秒
        /// </summary>
        public int AutoBackupTime
        {
            get
            {
                Line li = DB.LPS.FindLine("autobackuptime");
                if (li == null || !int.TryParse(li.info, out int ot))
                {
                    DB.LPS.AddLine(new Line("autobackuptime", "86400000"));
                    return 86400000;//默认1天备份一次
                }
                return ot;
            }
            set
            {
                if (value < 60000)
                    value = 60000;//最小值 1分钟
                Line li = DB.LPS.FindLine("autobackuptime");
                if (li == null)
                {
                    DB.LPS.AddLine(new Line("autobackuptime", value.ToString()));
                }
                else
                    li.info = value.ToString();
            }
        }
    }




}
