using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using LinePutScript;
using System.Threading;

namespace LinePutScript.DataBase
{
    public static class MapHelper
    {
        /// <summary>
        /// 映射文本到内存
        /// </summary>
        /// <param name="Name">名字 确认内存的位置</param>
        /// <param name="str">文本</param>
        public static void MapStringToMemory(string Name, string str)
        {
            ////创建异步阻塞
            //MemoryMappedFile lockmmf = MemoryMappedFile.CreateOrOpen("lpsdb" + Name + "lock", 1, MemoryMappedFileAccess.ReadWrite);
            //MemoryMappedViewAccessor lockmmva = lockmmf.CreateViewAccessor();
            //while (lockmmva.ReadBoolean(0))
            //{
            //    Thread.Sleep(10);
            //}
            //lockmmva.Write(0,true);

            //正常操作
            MemoryMappedFile mmf = MemoryMappedFile.OpenExisting(Name);
            MemoryMappedViewAccessor mmva = mmf.CreateViewAccessor();
            mmva.Write(0, str.Length);
            mmva.WriteArray(4, str.ToCharArray(), 0, str.Length);
            mmva.Dispose();
            mmf.Dispose();

        }
        /// <summary>
        /// 从内存获取文本
        /// </summary>
        /// <param name="Name">名字 确认内存的位置</param>
        /// <returns>获取的文本</returns>
        public static string GETStringFromMemory(string Name)
        {
            //创建异步阻塞
            MemoryMappedFile lockmmf = MemoryMappedFile.CreateOrOpen("lpsdb" + Name + "lock", 1, MemoryMappedFileAccess.ReadWrite);
            MemoryMappedViewAccessor lockmmva = lockmmf.CreateViewAccessor();
            while (lockmmva.ReadBoolean(0))
            {
                Thread.Sleep(10);
            }
            lockmmva.Write(0, true);
            //正常操作
            MemoryMappedFile mmf = MemoryMappedFile.OpenExisting(Name);
            MemoryMappedViewAccessor mmva = mmf.CreateViewAccessor();
            int len = mmva.ReadInt32(0);
            char[] buff = new char[len];
            mmva.ReadArray<char>(4, buff, 0, len);
            mmva.Dispose();
            mmf.Dispose();
            lockmmva.Write(0, false);
            lockmmva.Dispose();
            lockmmf.Dispose();
            return new string(buff);
        }
        /// <summary>
        /// 从内存获取Line
        /// </summary>
        /// <param name="Name">名字 确认内存的位置</param>
        /// <returns>获取的Line</returns>
        public static Line GETLineFromMemory(string Name)
        {
            return new Line(GETStringFromMemory(Name));
        }

        /// <summary>
        /// 从内存获取文本
        /// </summary>
        /// <param name="Name">名字 确认内存的位置</param>
        /// <returns>获取的文本</returns>
        public static string GETStringAndLock(string Name)
        {
            //创建异步阻塞
            MemoryMappedFile lockmmf = MemoryMappedFile.CreateOrOpen("lpsdb" + Name + "lock", 1, MemoryMappedFileAccess.ReadWrite);
            MemoryMappedViewAccessor lockmmva = lockmmf.CreateViewAccessor();
            while (lockmmva.ReadBoolean(0))
            {
                Thread.Sleep(10);
            }
            lockmmva.Write(0, true);
            //正常操作
            MemoryMappedFile mmf = MemoryMappedFile.OpenExisting(Name);
            MemoryMappedViewAccessor mmva = mmf.CreateViewAccessor();
            int len = mmva.ReadInt32(0);
            char[] buff = new char[len];
            mmva.ReadArray<char>(4, buff, 0, len);
            mmva.Dispose();
            mmf.Dispose();
            lockmmva.Dispose();
            lockmmf.Dispose();//可以毁掉 经过测试不会被清空
            return new string(buff);
        }
        /// <summary>
        /// 从内存获取Line
        /// </summary>
        /// <param name="Name">名字 确认内存的位置</param>
        /// <returns>获取的Line</returns>
        public static Line GETLineAndLock(string Name)
        {
            return new Line(GETStringAndLock(Name));
        }

        ///// <summary>
        ///// 映射文本到内存 !无视内存锁!
        ///// </summary>
        ///// <param name="Name">名字 确认内存的位置</param>
        ///// <param name="str">文本</param>
        //public static void NLMapStringToMemory(string Name, string str)
        //{            
        //    MemoryMappedViewAccessor mmva = MemoryMappedFile.OpenExisting(Name).CreateViewAccessor();
        //    mmva.Write(0, str.Length);
        //    mmva.WriteArray(4, str.ToCharArray(), 0, str.Length);
        //    mmva.Dispose();
        //}
        /// <summary>
        /// 从内存获取文本 !无视内存锁!
        /// </summary>
        /// <param name="Name">名字 确认内存的位置</param>
        /// <returns>获取的文本</returns>
        public static string NLGETStringFromMemory(string Name)
        {
            MemoryMappedFile mmf = MemoryMappedFile.OpenExisting(Name);
            MemoryMappedViewAccessor mmva = mmf.CreateViewAccessor();
            int len = mmva.ReadInt32(0);
            char[] buff = new char[len];
            mmva.ReadArray<char>(4, buff, 0, len);
            mmva.Dispose();
            mmf.Dispose();
            return new string(buff);
        }
        /// <summary>
        /// 从内存获取Line !无视内存锁!
        /// </summary>
        /// <param name="Name">名字 确认内存的位置</param>
        /// <returns>获取的Line</returns>
        public static Line NLGETLineFromMemory(string Name)
        {
            return new Line(NLGETStringFromMemory(Name));
        }



        /// <summary>
        /// 判断内存是否被锁定
        /// </summary>
        /// <param name="Name">名字 确认内存的位置</param>
        /// <returns>退回锁定状态</returns>
        public static bool IsLock(string Name)
        {
            MemoryMappedFile lockmmf = MemoryMappedFile.CreateOrOpen("lpsdb" + Name + "lock", 1, MemoryMappedFileAccess.ReadWrite);
            MemoryMappedViewAccessor lockmmva = lockmmf.CreateViewAccessor();
            bool bl = lockmmva.ReadBoolean(0);
            lockmmva.Dispose();
            lockmmf.Dispose();
            return bl;
        }


        /// <summary>
        /// 锁定指定内存
        /// </summary>
        /// <param name="Name">名字 确认内存的位置</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("代码质量", "IDE0067:Dispose objects before losing scope", Justification = "<挂起>")]
        public static void Lock(string Name)
        {
            MemoryMappedFile lockmmf = MemoryMappedFile.CreateOrOpen("lpsdb" + Name + "lock", 1, MemoryMappedFileAccess.ReadWrite);
            MemoryMappedViewAccessor lockmmva = lockmmf.CreateViewAccessor();
            lockmmva.Write(0, true);//启动挂起 开始进行操作
            lockmmva.Dispose();//不可以销毁mmf因为需要保留数据
        }
        /// <summary>
        /// 解锁指定内存
        /// </summary>
        /// <param name="Name">名字 确认内存的位置</param>
        public static void UnLock(string Name)
        {
            MemoryMappedFile lockmmf = MemoryMappedFile.CreateOrOpen("lpsdb" + Name + "lock", 1, MemoryMappedFileAccess.ReadWrite);
            MemoryMappedViewAccessor lockmmva = lockmmf.CreateViewAccessor();
            lockmmva.Write(0, false);
            lockmmva.Dispose();
            lockmmf.Dispose();
        }
        /// <summary>
        /// 等待锁定解锁
        /// </summary>
        /// <param name="Name">名字 确认内存的位置</param>
        public static void WaitLock(string Name)
        {
            while (IsLock(Name))
            {
                Thread.Sleep(20);
            }
        }

        //public static DataBase GetLPSDB(string DBName, string password = "")
        //{
        //    DBName = DBName.ToUpper();
        //    MemoryMappedViewAccessor viewAccessor = MemoryMappedFile.OpenExisting("lpsdb" + DBName).CreateViewAccessor();
        //    viewAccessor.Read(0, out DataBaseLocker locker);
        //    return locker.GetDataBase(password);
        //}
        //public static LpsDocument GetLPS(string DBName, string password = "")
        //    => GetLPSDB(DBName, password).LPS;

        /// <summary>
        /// 行读取帮助类
        /// </summary>
        public class LineHelper
        {
            /// <summary>
            /// 连接数据库名
            /// </summary>
            public string DBName;
            public LineHelper(string dbname)
            {
                DBName = dbname;
            }

            public Line LineLock() => GETLineAndLock(DBName);
            public void LineUnLock() => UnLock(DBName);

            /// <summary>
            /// 获取行
            /// </summary>
            public Line line
            {
                get => GETLineFromMemory(DBName);
                set => MapStringToMemory(DBName, value.ToString());
            }
            /// <summary>
            /// 名称 没有唯一性
            /// </summary>
            public string Name
            {
                get => line.Name;
                set
                {
                    Line tmp = line;
                    tmp.Name = value;
                    line = tmp;
                }
            }
            /// <summary>
            /// 信息 (去除关键字的文本)
            /// </summary>
            public string info
            {
                get => line.info;
                set
                {
                    Line tmp = line;
                    tmp.info = value;
                    line = tmp;
                }
            }
            /// <summary>
            /// 信息 (正常)
            /// </summary>
            public string Info
            {
                get => line.Info;
                set
                {
                    Line tmp = line;
                    tmp.Info = value;
                    line = tmp;
                }
            }
            /// <summary>
            /// 文本 在末尾没有结束行号的文本 (去除关键字的文本)
            /// </summary>
            public string test
            {
                get => line.text;
                set
                {
                    Line tmp = line;
                    tmp.text = value;
                    line = tmp;
                }
            }
            /// <summary>
            /// 文本 在末尾没有结束行号的文本 (正常)
            /// </summary>
            public string Test
            {
                get => line.Text;
                set
                {
                    Line tmp = line;
                    tmp.Text = value;
                    line = tmp;
                }
            }
            /// <summary>
            /// 子项目 !注意:无法直接修改,仅供获取!
            /// </summary>
            public List<Sub> Subs
            {
                get => line.Subs;
                set
                {
                    Line tmp = line;
                    tmp.Subs = value;
                    line = tmp;
                }
            }
            /// <summary>
            /// 将指定的Sub添加到Subs列表的末尾
            /// </summary>
            /// <param name="newSub">要添加的Sub</param>
            public void AddSub(Sub newSub)
            {
                Line tmp = LineLock();
                tmp.Subs.Add(newSub);
                line = tmp;
                LineUnLock();
            }
            /// <summary>
            /// 将指定Sub的元素添加到Subs的末尾
            /// </summary>
            /// <param name="newSubs">要添加的多个Sub</param>
            public void AddRange(params Sub[] newSubs)
            {
                Line tmp = LineLock();
                tmp.Subs.AddRange(newSubs); ;
                line = tmp;
                LineUnLock();
            }
            /// <summary>
            /// 将指定的Sub添加到指定索引处
            /// </summary>
            /// <param name="index">应插入 Sub 的从零开始的索引</param>
            /// <param name="newSub">要添加的Sub</param>
            public void InsertSub(int index, Sub newSub)
            {
                Line tmp = LineLock();
                tmp.Subs.Insert(index, newSub);
                line = tmp;
                LineUnLock();
            }
            /// <summary>
            /// 将指定Sub的元素添加指定索引处
            /// </summary>
            /// <param name="index">应插入 Sub 的从零开始的索引</param>
            /// <param name="newSubs">要添加的多个Sub</param>
            public void InsertRange(int index, params Sub[] newSubs)
            {
                Line tmp = LineLock();
                tmp.Subs.InsertRange(index, newSubs);
                line = tmp;
                LineUnLock();
            }
            /// <summary>
            /// 从Subs中移除特定对象的第一个匹配项
            /// </summary>
            /// <param name="SubName">要从Subs中删除的Sub的名称</param>
            /// <returns>如果成功移除了Sub，则为 true；否则为 false</returns>
            public bool Remove(string SubName)
            {
                Line tmp = LineLock();
                if (tmp.Remove(SubName))
                {
                    line = tmp;
                    LineUnLock();
                    return true;
                }
                LineUnLock();
                return false;
            }

            /// <summary>
            /// 返回一个值，该值指示指定的字段是否出现在Subs的Sub的名字
            /// </summary>
            /// <param name="value">字段</param>
            /// <returns>如果在Line集合中找到符合的名字，则为True；否则为false</returns>
            public bool Contains(string value)
            {
                return (Subs.FirstOrDefault(x => x.Name.Contains(value)) != null);
            }
            /// <summary>
            /// 确定某Sub是否在Line集合中
            /// </summary>
            /// <param name="subName">要在Line集合中定位的Sub的名字</param>
            /// <returns>如果在Line集合中找到符合的名字，则为True；否则为false</returns>
            public bool Have(string subName)
            {
                if (Name == subName)
                    return true;
                return (Subs.FirstOrDefault(x => x.Name == subName) != null);
            }


            /// <summary>
            /// 匹配拥有相同名称的Line或sub的所有元素 !注意:无法直接修改,仅供获取!
            /// </summary>
            /// <param name="subName">用于定义匹配的名称</param>
            /// <returns>如果找到相同名称的sub，其中所有元素均与指定谓词定义的条件匹配，则为该数组；否则为一个空的Array</returns>
            public Sub[] FindAll(string subName)
            {
                List<Sub> subs = new List<Sub>();
                Line tmp = line;
                if (Name == subName)
                    subs.Add(tmp);
                foreach (Sub su in tmp)
                    if (su.Name == subName)
                        subs.Add(su);
                return subs.ToArray();
            }
            /// <summary>
            /// 搜索与指定名称，并返回Line或整个Subs中的第一个匹配元素 !注意:无法直接修改,仅供获取!
            /// </summary>
            /// <param name="subName">用于定义匹配的名称</param>
            /// <returns>如果找到相同名称的第一个sub，则为该sub；否则为null</returns>
            public Sub Find(string subName)
            {
                return Subs.FirstOrDefault(x => x.Name == subName);
            }

            /// <summary>
            /// 搜索全部相似名称的Sub的所有元素 !注意:无法直接修改,仅供获取!
            /// </summary>
            /// <param name="value">字段</param>
            /// <returns>如果找到相似名称的Sub,则为数组；否则为一个空的Array</returns>
            public Sub[] SeachALL(string value)
            {
                Line tmp = line;
                List<Sub> subs = new List<Sub>();
                if (Name.Contains(value))
                    subs.Add(tmp);
                foreach (Sub su in tmp)
                    if (su.Name.Contains(value))
                        subs.Add(su);
                return subs.ToArray();
            }
            /// <summary>
            /// 搜索字段是否出现在Line名称，并返回整个Subs中的第一个匹配元素 !注意:无法直接修改,仅供获取!
            /// </summary>
            /// <param name="value">字段</param>
            /// <returns>如果找到相似名称的第一个Sub，则为该Sub；否则为null</returns>
            public Sub Seach(string value)
            {
                Line tmp = line;
                if (this.Name.Contains(value))
                    return tmp;
                return tmp.Subs.FirstOrDefault(x => x.Name.Contains(value));
            }

            /// <summary>
            /// 搜索相同名称的Sub，并返回整个Subs中第一个匹配的sub从零开始的索引
            /// </summary>
            /// <param name="subName">用于定义匹配的名称</param>
            /// <returns>如果找到相同名称的sub的第一个元素，则为该元素的从零开始的索引；否则为 -1</returns>
            public int IndexOf(string subName)
            {
                var sb = Subs;
                for (int i = 0; i < sb.Count; i++)
                {
                    if (sb[i].Name == subName)
                        return i;
                }
                return -1;
            }
            /// <summary>
            /// 搜索相同名称的Sub，并返回整个Sub中全部匹配的sub从零开始的索引
            /// </summary>
            /// <param name="subName">用于定义匹配的名称</param>
            /// <returns>如果找到相同名称的sub的元素，则为该元素的从零开始的索引组；否则为空的Array</returns>
            public int[] IndexsOf(string subName)
            {
                List<Sub> sb = Subs;
                List<int> lines = new List<int>();
                for (int i = 0; i < sb.Count; i++)
                {
                    if (sb[i].Name == subName)
                        lines.Add(i);
                }
                return lines.ToArray();
            }

            /// <summary>
            /// 将当前Line转换成文本格式 (info已经被转义/去除关键字)
            /// </summary>
            /// <returns>Line的文本格式 (info已经被转义/去除关键字)</returns>
            public new string ToString()//不能继承
            {
                return GETStringFromMemory(DBName);
            }

            /// <summary>
            /// 返回循环访问 Subs 的枚举数。 !注意:无法直接修改,仅供获取!
            /// </summary>
            /// <returns>用于 Subs 的枚举数</returns>
            public IEnumerator<Sub> GetEnumerator()
            {
                return Subs.GetEnumerator();
            }
            /// <summary>
            /// 返回一个 Subs 的第一个元素。!注意:无法直接修改,仅供获取!
            /// </summary>
            /// <returns>要返回的第一个元素</returns>
            public Sub First()
            {
                List<Sub> sb = Subs;
                if (sb.Count == 0)
                    return null;
                return sb[0];
            }
            /// <summary>
            /// 返回一个 Subs 的最后一个元素。!注意:无法直接修改,仅供获取!
            /// </summary>
            /// <returns>要返回的最后一个元素</returns>
            public Sub Last()
            {
                List<Sub> sb = Subs;
                if (sb.Count == 0)
                    return null;
                return sb[sb.Count - 1];
            }

            /// <summary>
            /// 添加或替换Sub在Subs
            /// </summary>
            /// <param name="sub">被替换的subs</param>
            public void AddOrReplace(Sub sub)
            {
                Line tmp = LineLock();
                Sub sb = tmp.Find(sub.Name);
                if (sb == null)
                {
                    tmp.AddSub(sub);
                }
                else
                {
                    sb.info = sub.info;
                }
                line = tmp;
                LineUnLock();
            }

        }
        /// <summary>
        /// 数据库读取帮助类
        /// </summary>
        public class DBHelper
        {          
            /// <summary>
            /// 要求:全称 包括前缀lpsdb
            /// </summary>
            public string DBName;

            public Line IndexLock() => GETLineAndLock(DBName);
            public void IndexUnLock() => UnLock(DBName);

            public DBHelper(string dbname)
            {
                DBName = dbname;
                IndexHelper = new LineHelper(dbname);
            }

            public Line Index
            {
                get => GETLineFromMemory(DBName);
                set => MapStringToMemory(DBName, value.ToString());
            }
            public LineHelper IndexHelper;

            /// <summary>
            /// 获取整个LPS文档 !注意:无法直接修改,仅供获取! !注意:占用时间长!
            /// </summary>
            public LpsDocument LPS
            {
                get
                {
                    MemoryMappedFile mmf = MemoryMappedFile.OpenExisting(DBName);
                    MemoryMappedViewAccessor mmva = mmf.CreateViewAccessor();
                    int len = mmva.ReadInt32(0);
                    char[] buff = new char[len];
                    mmva.ReadArray<char>(4, buff, 0, len);
                    mmva.Dispose();
                    mmf.Dispose();
                    Line index = new Line(new string(buff));

                    LpsDocument lps = new LpsDocument();
                    for (int i = 0; i < index.Subs.Count; i++)
                    {
                        mmf = MemoryMappedFile.OpenExisting(DBName + index.Subs[i].info);
                        mmva = mmf.CreateViewAccessor();
                        len = mmva.ReadInt32(0);
                        buff = new char[len];
                        mmva.ReadArray<char>(4, buff, 0, len);
                        mmva.Dispose();
                        mmf.Dispose();//关掉mmf进行回收
                        lps.Assemblage.Add(new Line(new string(buff)));
                    }
                    return lps;
                }
            }


            //List数据操作

            /// <summary>
            /// 将指定的Line添加到Assemblage列表的末尾
            /// </summary>
            /// <param name="newLine">要添加的Line</param>
            /// <returns>返回添加行的数据位置</returns>
            public LineHelper AddLine(Line newLine)
            {
                //首先先获得index
                Line idx = IndexLock();
                int id = idx.InfoToInt;//这是这次操作要进行的id
                MapStringToMemory(DBName + id.ToString(), newLine.ToString());//映射到内存
                idx.AddSub(new Sub(newLine.Name, id.ToString()));//绑定到index
                idx.info = (id + 1).ToString();//将新序号加入index
                Index = idx;//写入
                IndexUnLock();//解锁
                return new LineHelper(DBName + id.ToString());
            }
            /// <summary>
            /// 将指定Line的元素添加到Assemblage的末尾
            /// </summary>
            /// <param name="newLines">要添加的多个Line</param>
            public void AddRange(params Line[] newLines)
            {
                //首先先获得index
                Line idx = IndexLock();
                int id = idx.InfoToInt;//这是这次操作要进行的id
                foreach (Line li in newLines)
                {
                    MapStringToMemory(DBName + id.ToString(), li.ToString());//映射到内存
                    idx.AddSub(new Sub(li.Name, id++.ToString()));//绑定到index                    
                }
                idx.info = id.ToString();//将新序号加入index
                Index = idx;//写入
                IndexUnLock();//解锁
            }
            /// <summary>
            /// 将指定的Line添加到指定索引处
            /// </summary>
            /// <param name="index">应插入 Line 的从零开始的索引</param>
            /// <param name="newLine">要添加的Line</param>
            /// <returns>返回添加行的数据位置</returns>
            public LineHelper InsertLine(int index, Line newLine)
            {
                //首先先获得index
                Line idx = IndexLock();
                int id = idx.InfoToInt;//这是这次操作要进行的id
                MapStringToMemory(DBName + id.ToString(), newLine.ToString());//映射到内存
                idx.InsertSub(index, new Sub(newLine.Name, id.ToString()));//绑定到index
                idx.info = (id + 1).ToString();//将新序号加入index
                Index = idx;//写入
                IndexUnLock();//解锁
                return new LineHelper(DBName + id.ToString());
            }
            /// <summary>
            /// 将指定Line的元素添加指定索引处
            /// </summary>
            /// <param name="index">应插入 Line 的从零开始的索引</param>
            /// <param name="newLines">要添加的多个Line</param>
            public void InsertRange(int index, params Line[] newLines)
            {
                //首先先获得index
                Line idx = IndexLock();
                List<Sub> subs = new List<Sub>();
                int id = idx.InfoToInt;//这是这次操作要进行的id
                foreach (Line li in newLines)
                {
                    MapStringToMemory(DBName + id.ToString(), li.ToString());//映射到内存
                    subs.Add(new Sub(li.Name, id++.ToString()));//写入到缓存中                    
                }
                idx.InsertRange(index, subs.ToArray());//一次性绑定进index
                idx.info = id.ToString();//将新序号加入index
                Index = idx;//写入
                IndexUnLock();//解锁
            }
            /// <summary>
            /// 从Assemblage中移除特定名称的第一个匹配项
            /// </summary>
            /// <param name="lineName">要从Assemblage中删除的Line的名称</param>
            /// <returns>如果成功移除了line，则为 true；否则为 false</returns>
            public bool Remove(string lineName)
            {
                //首先先获得index
                Line idx = IndexLock();
                Sub sb = idx.Find(lineName);
                if (sb == null)
                {
                    IndexUnLock();//解锁
                    return false;
                }
                idx.Remove(sb);
                Index = idx;//写入
                IndexUnLock();//解锁
                return true;
            }
            /// <summary>
            /// 移除Assemblage的指定索引处的Line
            /// </summary>
            /// <param name="index">要移除的Line的从零开始的索引</param>
            public void RemoveAt(int index)
            {
                //首先先获得index
                Line idx = IndexLock();
                idx.Subs.RemoveAt(index);
                Index = idx;//写入
                IndexUnLock();//解锁
            }
            /// <summary>
            /// 确定某Line(名字定位)是否在Assemblage中
            /// </summary>
            /// <param name="lineName">Line的名字</param>
            /// <returns>如果在Assemblage中找到相同的名字，则为True；否则为false</returns>
            public bool HaveLine(string lineName)
            {
                return (Index.Subs.FirstOrDefault(x => x.Name == lineName) != null);
            }

            /// <summary>
            /// 搜索与指定名称，并返回整个Assemblage中的第一个匹配元素
            /// </summary>
            /// <param name="lineName">用于定义匹配的名称</param>
            /// <returns>如果找到相同名称的第一个Line，则为该Line；否则为null</returns>
            public LineHelper FindLine(string lineName)
            {
                Sub sb = Index.Subs.FirstOrDefault(x => x.Name == lineName);
                if (sb == null)
                    return null;
                return new LineHelper(DBName + sb.info);
            }
            /// <summary>
            /// 匹配拥有相同名称的Line的所有元素
            /// </summary>
            /// <param name="lineName">用于定义匹配的名称</param>
            /// <returns>如果找到相同名称的Line，其中所有元素均与指定谓词定义的条件匹配，则为该数组；否则为一个空的Array</returns>
            public LineHelper[] FindAllLine(string lineName)
            {
                List<LineHelper> lines = new List<LineHelper>();
                foreach (Sub li in Index.Subs.Where(x => x.Name == lineName))
                    lines.Add(new LineHelper(DBName + li.info));
                return lines.ToArray();
            }
            /// <summary>
            /// 搜索全部相似名称的Line的所有元素
            /// </summary>
            /// <param name="value">字段</param>
            /// <returns>如果找到相似名称的Line,则为数组；否则为一个空的Array</returns>
            public LineHelper[] SearchAllLine(string value)
            {
                List<LineHelper> lines = new List<LineHelper>();
                foreach (Sub li in Index.Subs.Where(x => x.Name.Contains(value)))
                    lines.Add(new LineHelper(DBName + li.info));
                return lines.ToArray();
            }
            /// <summary>
            /// 搜索字段是否出现在Line名称，并返回整个Assemblage中的第一个匹配元素
            /// </summary>
            /// <param name="value">字段</param>
            /// <returns>如果找到相似名称的第一个Line，则为该Line；否则为null</returns>
            public LineHelper SearchLine(string value)
            {
                Sub sb = Index.Subs.FirstOrDefault(x => x.Name.Contains(value));
                if (sb == null)
                    return null;
                return new LineHelper(DBName + sb.info);
            }
            /// <summary>
            /// 搜索相同名称的Line，并返回整个Assemblage中第一个匹配的Line从零开始的索引
            /// </summary>
            /// <param name="lineName">用于定义匹配的名称</param>
            /// <returns>如果找到相同名称的Line的第一个元素，则为该元素的从零开始的索引；否则为 -1</returns>
            public int IndexOf(string lineName)
            {
                Sub sb = Index.Subs.FirstOrDefault(x => x.Name == lineName);
                if (sb == null)
                    return -1;
                return sb.InfoToInt;
            }
            /// <summary>
            /// 搜索相同名称的Line，并返回整个Assemblage中全部匹配的Line从零开始的索引
            /// </summary>
            /// <param name="lineName">用于定义匹配的名称</param>
            /// <returns>如果找到相同名称的Line的元素，则为该元素的从零开始的索引组；否则为空的Array</returns>
            public int[] IndexsOf(string lineName)
            {
                List<int> lines = new List<int>();
                foreach (Sub li in Index.Subs.Where(x => x.Name == lineName))
                    lines.Add(li.InfoToInt);
                return lines.ToArray();
            }
            /// <summary>
            /// 返回一个Assemblage的第一个元素。
            /// </summary>
            /// <returns>要返回的第一个元素</returns>
            public LineHelper First()
            {
                Line id = Index;
                if (id.Subs.Count == 0)
                    return null;
                return new LineHelper(DBName + id.First());
            }
            /// <summary>
            /// 返回一个Assemblage的最后一个元素。
            /// </summary>
            /// <returns>要返回的最后一个元素</returns>
            public LineHelper Last()
            {
                Line id = Index;
                if (id.Subs.Count == 0)
                    return null;
                return new LineHelper(DBName + id.Last());
            }
            /// <summary>
            /// 将当前Documents转换成文本格式 !占用时间长!
            /// </summary>
            /// <returns>LinePutScript的文本格式</returns>
            public new string ToString() => LPS.ToString();
            /// <summary>
            /// 添加或替换Line在Assemblage
            /// </summary>
            /// <param name="line">被替换的Line</param>
            public void AddOrReplace(Line line)
            {
                Line idx = IndexLock();
                Sub sb = idx.Find(line.Name);
                if (sb == null)
                {
                    int id = idx.InfoToInt;//这是这次操作要进行的id
                    MapStringToMemory(DBName + id.ToString(), line.ToString());//映射到内存
                    idx.AddSub(new Sub(line.Name, id.ToString()));//绑定到index
                    idx.info = (id + 1).ToString();//将新序号加入index
                    Index = idx;//写入
                    IndexUnLock();
                    return;
                }
                MapStringToMemory(DBName + sb.info, line.ToString());//映射到内存
                sb.Name = line.Name;//同步下名字
                Index = idx;//写入
                IndexUnLock();
            }

        }


    }
}
