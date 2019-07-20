using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LinePutScript;
using LinePutScript.DataBase;


namespace DBHost
{
    /// <summary>
    /// 本程序是一个案例使用 LPSDB 同时作为一个简易的数据库主机
    /// 已经支持常用功能,可以直接使用
    /// </summary>
    class Program
    {
        static LPSDBHost Host;

        /// <summary>
        /// 默认储存地址
        /// </summary>
        public static readonly string PathMain = System.Environment.CurrentDirectory + @"\LPSDB";
        static void Main(string[] args)
        {
            Console.WriteLine("正在启动 LPSDBHost");
            DirectoryInfo di = new DirectoryInfo(PathMain);
            if (!di.Exists)
            {
                di.Create();//如果目录不存在则创建
            }
            Host = new LPSDBHost(PathMain);
            if (Host.CONFIG.AutoMapping)
                Console.WriteLine("自动映射数据库 完成");

            Console.WriteLine("启动完成 准备就绪\n\n输入 help:| 查看指令帮助");
            CommandLoop();
        }
        /// <summary>
        /// 循环询问指令
        /// </summary>
        static void CommandLoop()
        {
            Console.WriteLine();
            Line cm = new Line(Console.ReadLine());
            switch (cm.Name.ToLower())
            {
                case "help"://help#1:|
                    CMHelp(cm);
                    break;
                case "create"://create#name:|
                    CMCreate(cm);
                    break;
                case "list"://list:|Mapping#0:|(列出相关状态)
                    CMList(cm);
                    break;
                case "info"://info#dbname:|(某个的详细信息) info:|系统的详细信息
                    CMInfo(cm);
                    break;
                case "save"://储存
                    CMSave(cm);
                    break;
                case "saveall":
                    Host.SaveALL();
                    Console.WriteLine("全部数据库储存成功");
                    break;
                case "backup":
                    CMBackup(cm);
                    break;
                case "backupauto":
                    Host.BackUpAuto();
                    Console.WriteLine("自动备份数据库已经备份完毕");
                    break;
                case "map":
                    CMMap(cm);
                    break;
                case "mapauto":
                    Host.MaptoMemoryAuto();
                    Console.WriteLine("自动部署数据库已经映射至内存");
                    break;
                case "mapall":
                    Host.MaptoMemoryALL();
                    Console.WriteLine("全部数据库已经映射至内存");
                    break;
                case "mapoff":
                    CMMapoff(cm);
                    break;
                case "mapoffall":
                    Host.MapOffALL();
                    break;
                case "setdb"://setdb:|AutoMapping#1:| 
                case "set"://set#dbname:|AutoMapping#1:|
                    CMSet(cm);
                    break;
                case "rename"://rename#dbname,newname:|
                    CMReName(cm);
                    break;
                case "exit":
                case "stop":
                    Console.WriteLine("确定退出? Yes/No");
                    if (Console.ReadLine().ToLower() != "yes")
                    {
                        Console.WriteLine("退出操作已经取消");
                        break;
                    }
                    Host.SaveALL();
                    return;
                default:
                    Console.WriteLine("错误的指令,请检查拼写 输入 help:| 查看帮助");
                    break;
            }
            CommandLoop();
        }

        /// <summary>
        /// 判断数据库名是否合法
        /// </summary>
        public static bool NameCheck(string str)
        {
            string pattern = @"^[0-9A-Z_\-\(\)]+$";
            if (str == "")
            {
                Console.WriteLine("数据库名不能为空");
                return false;
            }
            if (str.Length > 32)
            {
                Console.WriteLine("数据库名长度不能超过32");
                return false;
            }
            if (System.Text.RegularExpressions.Regex.IsMatch(str, pattern))
            {
                return true;
            }
            else
            {
                Console.WriteLine("数据库名限定 英文大写+数字+下划线");
                return false;
            }
        }
        /// <summary>
        /// 数据大小转换工具
        /// </summary>
        public static string ToSizeString(long Size)
        {
            int i = 0;
            while (Size / (Math.Pow(1024, i)) > 1024)
            {
                i++;
            }
            double TrueSize = Convert.ToInt32(Size / (Math.Pow(1024, i)) * 100) * 0.01;
            switch (i)
            {
                case 0:
                    return TrueSize + "B";
                case 1:
                    return TrueSize + "KB";
                case 2:
                    return TrueSize + "MB";
                case 3:
                    return TrueSize + "GB";
                case 4:
                    return TrueSize + "TB";
                case 5:
                    return TrueSize + "PB";
                case 6:
                    return TrueSize + "EB";
                case 7:
                    return TrueSize + "ZB";
                case 8:
                    return TrueSize + "YB";
                default:
                    return TrueSize + "x" + i;
            }
        }

        //以下全是指令操作
        /// <summary>
        /// 查看指令帮助
        /// </summary>
        static void CMHelp(Line cm)
        {
            switch (cm.info)
            {
                case "":
                case "1":

                    break;
                default:
                    Console.WriteLine("错误的帮助id/已经达到帮助的末尾");
                    break;
            }
        }

        /// <summary>
        /// 创建数据库
        /// </summary>
        static void CMCreate(Line cm)
        {
            cm.info = cm.info.ToUpper();
            if (NameCheck(cm.info))
            {
                if (Host.DataBases.Find(x => x.Name == cm.info) != null)
                {
                    Console.WriteLine("已存在相同名称的数据库");
                    return;
                }
                Host.DataBases.Add(new DataBase(cm.info));
                Console.WriteLine($"数据库'{cm.info}' 已经成功创建");
            }
        }

        /// <summary>
        /// 更改数据库名字
        /// </summary>
        static void CMReName(Line cm)
        {
            cm.info = cm.info.ToUpper();
            string[] vs = cm.GetInfos();
            if (vs.Length != 2)
            {
                Console.WriteLine("更改数据库指令 ReName#{原数据库名},{新数据库名}:|");
                return;
            }
            var db = Host.DataBases.Find(x => x.Name == vs[0]);
            if (db == null)
            {
                Console.WriteLine($"找不到数据库 '{vs[0]}' 请检查名称后重试");
                return;
            }
            if (db.Mapping)
            {
                Console.WriteLine($"数据库 '{vs[0]}' 已被映射至内存,请关闭映射后重试");
                return;
            }
            if (NameCheck(vs[1]))
            {
                if (Host.DataBases.Find(x => x.Name == vs[1]) != null)
                {
                    Console.WriteLine("已存在相同名称的数据库");
                    return;
                }
                Host.ReName(db, vs[1]);
                Console.WriteLine($"数据库'{vs[0]}' 成功改名为 '{db.Name}'");
            }
        }

        /// <summary>
        /// 查看数据库信息
        /// </summary>
        static void CMSet(Line cm)
        {
            if (cm.info == "")//全局设置
            {
                foreach (Sub sb in cm)
                {
                    switch (sb.Name.ToLower())
                    {
                        case "":
                            break;

                        default:
                            Line li = Host.CONFIG.DB.LPS.FindLine(sb.Name.ToLower());
                            if (li == null)
                            {
                                Host.CONFIG.DB.LPS.AddLine(new Line(sb.Name.ToLower() + '#' + sb.info + ":|"));
                            }
                            else
                            {
                                li.info = sb.info;
                            }
                            Console.WriteLine($"全局设置:名称={sb.Name.ToLower()};内容={sb.Info}");
                            break;
                        case "automapping":
                            if (sb.info == "" || sb.info == "0" || sb.info.ToLower() == "false")
                                Host.CONFIG.AutoMapping = false;
                            else
                                Host.CONFIG.AutoMapping = true;
                            Console.WriteLine($"全局设置:启动后自动映射;内容={Host.CONFIG.AutoMapping}");
                            break;
                        case "autosavetime":
                            Host.CONFIG.AutoSaveTime = sb.InfoToInt;
                            Console.WriteLine($"全局设置:自动储存时间间隔;内容={Host.CONFIG.AutoSaveTime}毫秒");
                            break;
                        case "autobackuptime":
                            Host.CONFIG.AutoBackupTime = sb.InfoToInt;
                            Console.WriteLine($"全局设置:自动储存时间间隔;内容={Host.CONFIG.AutoBackupTime}毫秒");
                            break;
                    }
                }
                return;
            }
            cm.info = cm.info.ToUpper();
            var db = Host.DataBases.Find(x => x.Name == cm.info);
            if (db == null)
            {
                Console.WriteLine($"找不到数据库 '{cm.info}' 请检查名称后重试");
                return;
            }
            foreach (Sub sb in cm)
            {
                sb.Name = sb.Name.ToLower();
                switch (sb.Name)
                {
                    case "":
                        break;

                    default:
                        Sub li = db.LPS.First().Find(sb.Name);
                        if (li == null)
                        {
                            db.LPS.First().AddSub(sb);
                        }
                        else
                        {
                            li.info = sb.info;
                        }
                        Console.WriteLine($"'{cm.info}'设置:名称={sb.Name};内容={sb.Info}");
                        break;
                    case "automapping":
                        if (sb.info == "" || sb.info == "0" || sb.info.ToLower() == "false")
                            db.AutoMapping = false;
                        else
                            db.AutoMapping = true;
                        Console.WriteLine($"'{cm.info}'设置:启动后自动映射;内容={db.AutoMapping}");
                        break;
                    case "backup":
                    case "autobackup":
                        if (sb.info == "" || sb.info == "0" || sb.info.ToLower() == "false")
                            db.AutoBackup = false;
                        else
                            db.AutoBackup = true;
                        Console.WriteLine($"'{cm.info}'设置:启动后自动映射;内容={db.AutoBackup}");
                        break;
                    case "password":
                        db.Password = sb.Info;
                        Console.WriteLine($"'{cm.info}'设置:访问密码;内容={db.Password}");
                        break;
                    case "capacity":
                        db.Capacity = sb.InfoToInt;
                        Console.WriteLine($"'{cm.info}'设置:分配的内存容量;内容={db.Capacity.ToString()}({ToSizeString(db.Capacity)})");
                        break;
                }
            }
        }

        /// <summary>
        /// 查看数据库信息
        /// </summary>
        static void CMInfo(Line cm)
        {
            cm.info = cm.info.ToUpper();
            var db = Host.DataBases.Find(x => x.Name == cm.info);
            if (db == null)
            {
                Console.WriteLine($"找不到数据库 '{cm.info}' 请检查名称后重试");
                return;
            }
            Console.WriteLine($"数据库:'{db.Name}' 映射:{db.Mapping.ToString()}\n密码:{db.Password}\n已用/分配:{ToSizeString(db.Length)}/{ToSizeString(db.Capacity)}"
                + $"自动备份:{db.AutoBackup.ToString()} 自动部署:{db.AutoMapping.ToString()}");
        }
        /// <summary>
        /// 储存指定数据库
        /// </summary>
        static void CMSave(Line cm)
        {
            cm.info = cm.info.ToUpper();
            var db = Host.DataBases.Find(x => x.Name == cm.info);
            if (db == null)
            {
                Console.WriteLine($"找不到数据库 '{cm.info}' 请检查名称后重试");
                return;
            }
            Host.Save(db);
            Console.WriteLine($"数据库 '{cm.info}' 储存成功");
        }
        /// <summary>
        /// 备份指定数据库
        /// </summary>
        static void CMBackup(Line cm)
        {
            cm.info = cm.info.ToUpper();
            var db = Host.DataBases.Find(x => x.Name == cm.info);
            if (db == null)
            {
                Console.WriteLine($"找不到数据库 '{cm.info}' 请检查名称后重试");
                return;
            }
            Host.BackUp(db);
            Console.WriteLine($"数据库 '{cm.info}' 备份成功");
        }
        /// <summary>
        /// 映射指定数据库到内存
        /// </summary>
        static void CMMap(Line cm)
        {
            cm.info = cm.info.ToUpper();
            var db = Host.DataBases.Find(x => x.Name == cm.info);
            if (db == null)
            {
                Console.WriteLine($"找不到数据库 '{cm.info}' 请检查名称后重试");
                return;
            }
            if (db.Mapping)
            {
                Console.WriteLine($"数据库 '{cm.info}' 已经映射至内存,无需重复映射");
            }
            db.MaptoMemory();
            Console.WriteLine($"数据库 '{cm.info}' 成功映射至内存");
        }
        /// <summary>
        /// 关闭映射指定数据库到内存
        /// </summary>
        static void CMMapoff(Line cm)
        {
            cm.info = cm.info.ToUpper();
            var db = Host.DataBases.Find(x => x.Name == cm.info);
            if (db == null)
            {
                Console.WriteLine($"找不到数据库 '{cm.info}' 请检查名称后重试");
                return;
            }
            if (!db.Mapping)
            {
                Console.WriteLine($"数据库 '{cm.info}' 已经关闭映射,无需重复关闭");
            }
            db.Mapoff();
            Console.WriteLine($"数据库 '{cm.info}' 成功关闭映射");
        }
        /// <summary>
        /// 列出全部数据库
        /// </summary>
        static void CMList(Line cm)
        {
            if (cm.Find("mapping") != null)
            {
                if (cm.Find("mapping").info == "0")
                {
                    Console.WriteLine("列出所有未映射的数据库\n--数据库名--        --映射状态--  --已用大小/分配大小--");
                    foreach (DataBase db in Host.DataBases.Where(x => !x.Mapping))
                    {
                        Console.WriteLine(db.Name.PadRight(25) + db.Mapping.ToString().PadRight(15) + ToSizeString(db.Length) + '/' + ToSizeString(db.Capacity));
                    }
                    return;
                }
                else
                {
                    Console.WriteLine("列出所有映射的数据库\n--数据库名--        --映射状态--  --已用大小/分配大小--");
                    foreach (DataBase db in Host.DataBases.Where(x => x.Mapping))
                    {
                        Console.WriteLine(db.Name.PadRight(25) + db.Mapping.ToString().PadRight(15) + ToSizeString(db.Length) + '/' + ToSizeString(db.Capacity));
                    }
                    return;
                }
            }
            Console.WriteLine("列出所有数据库\n--数据库名--        --映射状态--  --已用大小/分配大小--");
            foreach (DataBase db in Host.DataBases)
            {
                Console.WriteLine(db.Name.PadRight(25) + db.Mapping.ToString().PadRight(15) + ToSizeString(db.Length) + '/' + ToSizeString(db.Capacity));
            }
        }


    }
}
