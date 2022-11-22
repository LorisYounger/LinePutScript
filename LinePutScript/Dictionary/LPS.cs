using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable enable
namespace LinePutScript.Dictionary
{
    /// <summary>
    /// 通过字典类型的文件 包括文件读写等一系列操作
    /// </summary>
    public class LPS<TLine, TSub, V> : ILPS<TLine, TSub, V>, IReadOnlyList<TLine>, IReadOnlyCollection<TLine>
        where TLine : ILine<TSub, V>, new()
        where TSub : ISub<V>, new()
        where V : ISetObject, new()
    {
        /// <summary>
        /// 集合 全部文件的数据
        /// </summary>
        public Dictionary<string, TLine> Assemblage { get; set; } = new Dictionary<string, TLine>();

        /// <summary>
        /// 创建一个 LpsDocument
        /// </summary>
        public LPS() { }
        /// <summary>
        /// 从指定的字符串创建 LpsDocument
        /// </summary>
        /// <param name="lps">包含要加载的LPS文档的字符串</param>
        public LPS(string lps)
        {
            Load(lps);
        }

        #region List操作
        /// <summary>
        /// 将指定的Line添加到Assemblage列表
        /// </summary>
        /// <param name="newLine">要添加的Line</param>
        public void AddLine(TLine newLine)
        {
            Assemblage[newLine.Name] = newLine;
        }
        /// <summary>
        /// 若无相同名称(Name)的Line,则将指定的Line添加到Assemblage列表
        /// 若有,则替换成要添加的Line
        /// </summary>
        /// <param name="newLine">要添加的Line</param>
        public void AddorReplaceLine(TLine newLine)
        {
            Assemblage[newLine.Name] = newLine;
        }
        /// <summary>
        /// 将指定Line的元素添加到Assemblage
        /// </summary>
        /// <param name="newLines">要添加的多个Line</param>
        public void AddRange(params TLine[] newLines)
        {
            foreach (var item in newLines)
            {
                Assemblage[item.Name] = item;
            }
        }
        /// <summary>
        /// 将指定Line的元素添加到Assemblage
        /// </summary>
        /// <param name="newLines">要添加的多个Line</param>
        public void AddRange(IEnumerable<TLine> newLines)
        {
            foreach (var item in newLines)
            {
                Assemblage[item.Name] = item;
            }
        }
        /// <summary>
        /// 将指定的Line添加到指定索引处(失效:字典没有顺序)
        /// </summary>
        /// <param name="index">应插入 Line 的从零开始的索引</param>
        /// <param name="newLine">要添加的Line</param>
        [Obsolete]
        public void InsertLine(int index, TLine newLine)
        {
            Assemblage[newLine.Name] = newLine;
        }
        /// <summary>
        /// 将指定Line的元素添加指定索引处(失效:字典没有顺序)
        /// </summary>
        /// <param name="index">应插入 Line 的从零开始的索引</param>
        /// <param name="newLines">要添加的多个Line</param>
        [Obsolete]
        public void InsertRange(int index, params TLine[] newLines)
        {
            AddRange(newLines);
        }
        /// <summary>
        /// 将指定Line的元素添加指定索引处(失效:字典没有顺序)
        /// </summary>
        /// <param name="index">应插入 Line 的从零开始的索引</param>
        /// <param name="newLines">要添加的多个Line</param>
        [Obsolete]
        public void InsertRange(int index, IEnumerable<TLine> newLines)
        {
            AddRange(newLines);
        }
        /// <summary>
        /// 从Assemblage中移除特定对象的第一个匹配项
        /// </summary>
        /// <param name="line">要从Assemblage中删除的Line的名称</param>
        /// <returns>如果成功移除了line,则为 true; 否则为 false</returns>
        public bool Remove(TLine line)
        {
            return Assemblage.Remove(line.Name);
        }
        /// <summary>
        /// 从Assemblage中移除特定名称的第一个匹配项 
        /// </summary>
        /// <param name="lineName">要从Assemblage中删除的Line的名称</param>
        /// <returns>如果成功移除了line,则为 true; 否则为 false</returns>
        public bool Remove(string lineName)
        {
            return Assemblage.Remove(lineName);
        }

        /// <summary>
        /// 从Assemblage中移除移除与条件相匹配的所有Line (失效:字典没有顺序)
        /// </summary>
        /// <param name="line">要从Assemblage中删除的Line</param>
        [Obsolete]
        public void RemoveAll(TLine line)
        {
            Assemblage.Remove(line.Name);
        }
        /// <summary>
        /// 从Assemblage中移除移除与名称相匹配的所有Line (失效:字典没有顺序)
        /// </summary>
        /// <param name="lineName">要从Assemblage中删除的Line的名称</param>
        [Obsolete]
        public void RemoveAll(string lineName)
        {
            Assemblage.Remove(lineName);
        }
        /// <summary>
        /// 移除Assemblage的指定索引处的Line (错误:字典没有引索)
        /// </summary>
        /// <param name="index">要移除的Line的从零开始的索引</param>
        [Obsolete]
        public void RemoveAt(int index)
        {
            throw new ArrayTypeMismatchException();
        }

        /// <summary>
        /// 确定某Line是否在Assemblage中
        /// </summary>
        /// <param name="line">要在Assemblage中定位的Line</param>
        /// <returns>如果在Assemblage中找到line,则为True; 否则为false </returns>
        public bool Contains(TLine line)
        {
            return Assemblage.ContainsKey(line.Name);
        }
        /// <summary>
        /// 确定某Sub是否在Assemblage中
        /// </summary>
        /// <param name="sub">要在Assemblage中定位的Sub</param>
        /// <returns>如果在Assemblage中找到line,则为True; 否则为false</returns>
        public bool Contains(TSub sub)
        {
            foreach (TLine li in Assemblage.Values)
                if (li.Contains(sub))
                    return true;
            return false;
        }
        /// <summary>
        /// 确定某Line(名字定位)是否在Assemblage中
        /// </summary>
        /// <param name="value">Line的名字</param>
        /// <returns>如果在Assemblage中找到相同的名字,则为True; 否则为false</returns>
        public bool ContainsLine(string value)
        {
            return Assemblage.ContainsKey(value);
        }
        /// <summary>
        /// 确定某sub(名字定位)是否在Assemblage中
        /// </summary>
        /// <param name="value">sub的名字</param>
        /// <returns>如果在Assemblage的sub中找到相同的名字,则为True; 否则为false</returns>
        public bool ContainsSub(string value)
        {
            return Assemblage.Values.FirstOrDefault(x => x.Contains(value)) != null;
        }


        /// <summary>
        /// 匹配拥有相同名称的Line的所有元素(注意:在字典中,信息是唯一的)
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的Line,其中所有元素均与指定谓词定义的条件匹配,则为该数组; 否则为一个空的Array</returns>
        [Obsolete]
        public TLine[] FindAllLine(string lineName)
        {
            var v = Assemblage[lineName];
            if (v == null)
                return new TLine[] { };
            else
                return new TLine[] { v };
        }
        /// <summary>
        /// 匹配拥有相同名称和信息的Line的所有元素(注意:在字典中,信息是唯一的)
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <param name="lineinfo">用于定义匹配的信息 (去除关键字的文本)</param>
        /// <returns>如果找到相同名称和信息的Line,其中所有元素均与指定谓词定义的条件匹配,则为该数组; 否则为一个空的Array</returns>
        [Obsolete]
        public TLine[] FindAllLine(string lineName, string lineinfo)
        {
            var v = FindLine(lineName, lineinfo);
            if (v == null)
                return new TLine[] { };
            else
                return new TLine[] { v };
        }
        /// <summary>
        /// 匹配拥有相同信息的Line的所有元素(注意:在字典中,信息是唯一的)
        /// </summary>
        /// <param name="lineinfo">用于定义匹配的信息 (去除关键字的文本)</param>
        /// <returns>如果找到相同信息的Line,其中所有元素均与指定谓词定义的条件匹配,则为该数组; 否则为一个空的Array</returns>
        [Obsolete]
        public TLine[] FindAllLineInfo(string lineinfo)
        {
            var v = FindLineInfo(lineinfo);
            if (v == null)
                return new TLine[] { };
            else
                return new TLine[] { v };
        }
        /// <summary>
        /// 搜索与指定名称,并返回整个Assemblage中的第一个匹配元素
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的第一个Line,则为该Line; 否则为null</returns>
        public TLine FindLine(string lineName)
        {
            return Assemblage[lineName];
        }
        /// <summary>
        /// 搜索与指定名称和信息,并返回整个Assemblage中的第一个匹配元素
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <param name="lineinfo">用于定义匹配的信息 (去除关键字的文本)</param>
        /// <returns>如果找到相同名称和信息的第一个Line,则为该Line; 否则为null</returns>
        public TLine FindLine(string lineName, string lineinfo)
        {
            return Assemblage.Values.FirstOrDefault(x => x.Name == lineName && x.info.GetString().Equals(lineinfo));
        }
        /// <summary>
        /// 搜索与指定信息,并返回整个Assemblage中的第一个匹配元素
        /// </summary>
        /// <param name="lineinfo">用于定义匹配的信息 (去除关键字的文本)</param>
        /// <returns>如果找到相同信息的第一个Line,则为该Line; 否则为null</returns>
        public TLine FindLineInfo(string lineinfo)
        {
            return Assemblage.Values.FirstOrDefault(x => x.info.GetString().Equals(lineinfo));
        }
        /// <summary>
        /// 搜索与指定名称,并返回整个Assemblage中的第一个匹配元素; 若未找到,则新建并添加相同名称的Line,并且返回这个Line
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的第一个Line,则为该Line; 否则为新建的相同名称Line</returns>
        public TLine FindorAddLine(string lineName)
        {
            TLine line = FindLine(lineName);
            if (line == null)
            {
                line = new TLine();
                line.Name = lineName;
                AddLine(line);
                return line;
            }
            else
            {
                return line;
            }
        }
        /// <summary>
        /// 匹配拥有相同名称的Sub的所有元素
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的Sub,其中所有元素均与指定谓词定义的条件匹配,则为该数组; 否则为一个空的Array</returns>
        public TSub[] FindAllSub(string subName)
        {
            List<TSub> lines = new List<TSub>();
            foreach (TLine li in Assemblage.Values)
            {
                lines.AddRange(li.FindAll(subName));
            }
            return lines.ToArray();
        }
        /// <summary>
        /// 匹配拥有相同名称和信息的Sub的所有元素
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <param name="subinfo">用于定义匹配的信息 (去除关键字的文本)</param>
        /// <returns>如果找到相同名称和信息的Sub,其中所有元素均与指定谓词定义的条件匹配,则为该数组; 否则为一个空的Array</returns>
        public TSub[] FindAllSub(string subName, string subinfo)
        {
            List<TSub> lines = new List<TSub>();
            foreach (TLine li in Assemblage.Values)
            {
                lines.AddRange(li.FindAll(subName, subinfo));
            }
            return lines.ToArray();
        }
        /// <summary>
        /// 匹配拥有相同信息的Sub的所有元素
        /// </summary>
        /// <param name="subinfo">用于定义匹配的信息 (去除关键字的文本)</param>
        /// <returns>如果找到相同信息的Sub,其中所有元素均与指定谓词定义的条件匹配,则为该数组; 否则为一个空的Array</returns>
        public TSub[] FindAllSubInfo(string subinfo)
        {
            List<TSub> lines = new List<TSub>();
            foreach (TLine li in Assemblage.Values)
            {
                lines.AddRange(li.FindAllInfo(subinfo));
            }
            return lines.ToArray();
        }
        /// <summary>
        /// 搜索与指定名称,并返回整个Assemblage中的第一个匹配元素
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的第一个Sub,则为该Line; 否则为null</returns>
        public TSub FindSub(string subName)
        {
            foreach (TLine li in Assemblage.Values)
            {
                var l = li.Find(subName);
                if (l != null)
                    return l;
            }
            return default;
        }
        /// <summary>
        /// 搜索与指定名称和信息,并返回整个Assemblage中的第一个匹配元素
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <param name="subinfo">用于定义匹配的信息 (去除关键字的文本)</param>
        /// <returns>如果找到相同名称和信息的第一个Sub,则为该Line; 否则为null</returns>
        public TSub FindSub(string subName, string subinfo)
        {
            foreach (TLine li in Assemblage.Values)
            {
                var l = li.Find(subName, subinfo);
                if (l != null)
                    return l;
            }
            return default;
        }
        /// <summary>
        /// 搜索与指定信息,并返回整个Assemblage中的第一个匹配元素
        /// </summary>
        /// <param name="subinfo">用于定义匹配的信息 (去除关键字的文本)</param>
        /// <returns>如果找到相同信息的第一个Sub,则为该Line; 否则为null</returns>
        public TSub FindSubInfo(string subinfo)
        {
            foreach (TLine li in Assemblage.Values)
            {
                var l = li.FindInfo(subinfo);
                if (l != null)
                    return l;
            }
            return default;
        }

        /// <summary>
        /// 搜索全部相似名称的Line的所有元素
        /// </summary>
        /// <param name="value">%字段%</param>
        /// <returns>如果找到相似名称的Line,则为数组; 否则为一个空的Array</returns>
        public TLine[] SearchAllLine(string value)
        {
            List<TLine> lines = new List<TLine>();
            foreach (TLine li in Assemblage.Values)
                if (li.Name.Contains(value))
                    lines.Add(li);
            return lines.ToArray();
        }
        /// <summary>
        /// 搜索字段是否出现在Line名称,并返回整个Assemblage中的第一个匹配元素
        /// </summary>
        /// <param name="value">%字段%</param>
        /// <returns>如果找到相似名称的第一个Line,则为该Line; 否则为null</returns>
        public TLine SearchLine(string value)
        {
            return Assemblage.Values.FirstOrDefault(x => x.Name.Contains(value));
        }
        /// <summary>
        /// 搜索全部相似名称的Sub的所有元素
        /// </summary>
        /// <param name="value">%字段%</param>
        /// <returns>如果找到相似名称的Line,则为该数组; 否则为一个空的Array</returns>
        public TSub[] SearchAllSub(string value)
        {
            List<TSub> lines = new List<TSub>();
            foreach (TLine li in Assemblage.Values)
            {
                lines.AddRange(li.SeachALL(value));
            }
            return lines.ToArray();
        }
        /// <summary>
        /// 搜索字段是否出现在Sub名称,并返回整个Assemblage中的第一个匹配元素
        /// </summary>
        /// <param name="value">%字段%</param>
        /// <returns>如果找到相同名称的第一个Sub,则为该Sub; 否则为null</returns>
        public TSub SearchSub(string value)
        {
            foreach (TLine li in Assemblage.Values)
            {
                var l = li.Seach(value);
                if (l != null)
                    return l;
            }
            return default;
        }

        /// <summary>
        /// 搜索相同名称的Line,并返回整个Assemblage中第一个匹配的Line从零开始的索引 (错误:字典没有引索)
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的Line的第一个元素,则为该元素的从零开始的索引; 否则为 -1</returns>
        [Obsolete]
        public int IndexOf(string lineName)
        {
            throw new ArrayTypeMismatchException();
        }
        /// <summary>
        /// 搜索相同名称的Line,并返回整个Assemblage中全部匹配的Line从零开始的索引 (错误:字典没有引索)
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的Line的元素,则为该元素的从零开始的索引组; 否则为空的Array</returns>
        [Obsolete]
        public int[] IndexsOf(string lineName)
        {
            throw new ArrayTypeMismatchException();
        }
        /// <summary>
        /// 获得Assemblage目前储存的Line数量
        /// </summary>
        public int Count => Assemblage.Count;
        #endregion

        /// <summary>
        /// 从指定的字符串加载LPS文档
        /// </summary>
        /// <param name="lps">包含要加载的LPS文档的字符串</param>
        public void Load(string lps)
        {
            Assemblage.Clear();//清空当前文档
            string[] file = lps.Replace("\r", "").Replace(":\n|", "/n").Replace(":\n:", "").Trim('\n').Split('\n');
            foreach (string str in file)
            {
                if (str != "")
                {
                    TLine t = new TLine();
                    t.Load(str);
                    Add(t);
                }
            }
        }

        /// <summary>
        /// 返回一个Assemblage的第一个元素。
        /// </summary>
        /// <returns>要返回的第一个元素</returns>
        public TLine First() => Assemblage.Values.First();
        /// <summary>
        /// 返回一个Assemblage的最后一个元素。
        /// </summary>
        /// <returns>要返回的最后一个元素</returns>
        public TLine Last() => Assemblage.Values.Last();
        /// <summary>
        /// 返回循环访问 Assemblage 的枚举数。
        /// </summary>
        /// <returns>用于 Assemblage 的枚举数</returns>
        public IEnumerator<TLine> GetEnumerator()
        {
            return Assemblage.Values.GetEnumerator();
        }

        /// <summary>
        /// 搜索与指定名称,并返回整个Assemblage中的第一个匹配元素; 若未找到,则新建并添加相同名称的Line,并且返回这个Line
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的第一个Line,则为该Line; 否则为新建的相同名称Line</returns>
        public TLine this[string lineName]
        {
            get
            {
                return FindorAddLine(lineName);
            }
            set
            {
                AddorReplaceLine(value);
            }
        }


        /// <summary>
        /// 将当前Documents转换成文本格式
        /// </summary>
        /// <returns>LinePutScript的文本格式</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (TLine li in Assemblage.Values)
                li.ToString(sb);
            return sb.ToString().Trim('\n');
        }
        /// <summary>
        /// 获得该LPS文档的长哈希代码
        /// </summary>
        /// <returns>64位哈希代码</returns>
        public long GetLongHashCode()
        {
            int id = 2;
            long hash = 0;
            foreach (TLine li in Assemblage.Values)
                hash += li.GetLongHashCode() * id++;
            return hash;
        }
        /// <summary>
        /// 获得该LPS文档的哈希代码
        /// </summary>
        /// <returns>32位哈希代码</returns>
        public override int GetHashCode() => (int)GetLongHashCode();
        /// <summary>
        /// 确认对象是否等于当前对象
        /// </summary>
        /// <param name="obj">Subs</param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            if (obj?.GetType() != GetType())
                return false;
            return ((LPS<TLine, TSub, V>)obj).GetLongHashCode() == GetLongHashCode();
        }
        /// <summary>
        /// 获得当前文档大小 单位:字符
        /// </summary>
        public int Length
        {
            get
            {
                int l = 3;
                foreach (TLine li in Assemblage.Values)
                {
                    l += li.Name.Length + li.info.GetStoreString().Length + li.Text.Length + 6;
                    foreach (TSub sb in li)
                    {
                        l += sb.Name.Length + sb.info.GetStoreString().Length + 3;
                    }
                }

                return l;
            }
        }
        /// <summary>
        /// 是否只读
        /// </summary>
        public bool IsReadOnly => ((ICollection<TLine>)Assemblage).IsReadOnly;

        /// <summary>
        /// 通过引索修改lps中Line内容 (错误:字典没有引索)
        /// </summary>
        /// <param name="index">要获得或设置的引索</param>
        /// <returns>引索指定的Line</returns>
        [Obsolete] public TLine this[int index] { get => throw new ArrayTypeMismatchException(); set => throw new ArrayTypeMismatchException(); }

        #region GETER

        /// <summary>
        /// 获得bool属性的line
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的line,则为True; 否则为false</returns>
        public bool GetBool(string lineName) => FindLine(lineName)?.info.GetBoolean() ?? false;
        /// <summary>
        /// 设置bool属性的line
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <param name="value">
        /// 如果为ture,则在没有相同name为lineName的line时候添加新的line
        /// 如果为false,则删除所有name为lineName的line
        /// </param>
        public void SetBool(string lineName, bool value)
        {
            FindorAddLine(lineName).info.SetBoolean(value);
        }
        /// <summary>
        /// 获得int属性的line
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <param name="defaultvalue">如果没找到返回的默认值</param>
        /// <returns>
        /// 如果找到相同名称的line,返回line中储存的int值
        /// 如果没找到,则返回默认值
        /// </returns>
        public int GetInt(string lineName, int defaultvalue = default)
        {
            TLine line = FindLine(lineName);
            if (line == null)
                return defaultvalue;
            return line.InfoToInt;
        }
        /// <summary>
        /// 设置int属性的line
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <param name="value">储存进line的int值</param>
        public void SetInt(string lineName, int value) => FindorAddLine(lineName).InfoToInt = value;

        /// <summary>
        /// 获得long属性的line
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <param name="defaultvalue">如果没找到返回的默认值</param>
        /// <returns>
        /// 如果找到相同名称的line,返回line中储存的long值
        /// 如果没找到,则返回默认值
        /// </returns>
        public long GetInt64(string lineName, long defaultvalue = default)
        {
            TLine line = FindLine(lineName);
            if (line == null)
                return defaultvalue;
            return line.InfoToInt64;
        }
        /// <summary>
        /// 设置long属性的line
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <param name="value">储存进line的long值</param>
        public void SetInt64(string lineName, long value) => FindorAddLine(lineName).InfoToInt64 = value;

        /// <summary>
        /// 获得String属性的line
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <param name="defaultvalue">如果没找到返回的默认值</param>
        /// <returns>
        /// 如果找到相同名称的line,返回line中储存的string值
        /// 如果没找到,则返回默认值
        /// </returns>
        public string? GetString(string lineName, string? defaultvalue = default)
        {
            TLine line = FindLine(lineName);
            if (line == null)
                return defaultvalue;
            return line.Info;
        }
        /// <summary>
        /// 设置String属性的line
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <param name="value">储存进line的String值</param>
        public void SetString(string lineName, string? value) => FindorAddLine(lineName).Info = value ?? "";

        /// <summary>
        /// 获得double属性的line
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <param name="defaultvalue">如果没找到返回的默认值</param>
        /// <returns>
        /// 如果找到相同名称的line,返回line中储存的double值
        /// 如果没找到,则返回默认值
        /// </returns>
        public double GetDouble(string lineName, double defaultvalue = default)
        {
            TLine line = FindLine(lineName);
            if (line == null)
                return defaultvalue;
            return line.InfoToDouble;
        }
        /// <summary>
        /// 设置double属性的line
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <param name="value">储存进line的double值</param>
        public void SetDouble(string lineName, double value) => FindorAddLine(lineName).info.SetDouble(value);


        /// <summary>
        /// 获得double(long)属性的line 通过转换long获得更精确的小数,小数位最大保留9位
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <param name="defaultvalue">如果没找到返回的默认值</param>
        /// <returns>
        /// 如果找到相同名称的line,返回line中储存的double(long)值
        /// 如果没找到,则返回默认值
        /// </returns>
        public double GetFloat(string lineName, double defaultvalue = default)
        {
            TLine line = FindLine(lineName);
            if (line == null)
                return defaultvalue;
            return line.info.GetFloat();
        }
        /// <summary>
        /// 设置double(long)属性的line 通过转换long获得更精确的小数,小数位最大保留9位
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <param name="value">储存进line的double(long)值</param>
        public void SetFloat(string lineName, double value) => FindorAddLine(lineName).info.SetFloat(value);

        /// <summary>
        /// 获得DateTime属性的line
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <param name="defaultvalue">如果没找到返回的默认值</param>
        /// <returns>
        /// 如果找到相同名称的line,返回line中储存的DateTime值
        /// 如果没找到,则返回默认值
        /// </returns>
        public DateTime GetDateTime(string lineName, DateTime defaultvalue = default)
        {
            TLine line = FindLine(lineName);
            if (line == null)
                return defaultvalue;
            return line.info.GetDateTime();
        }
        /// <summary>
        /// 设置DateTime属性的line
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <param name="value">储存进line的DateTime值</param>
        public void SetDateTime(string lineName, DateTime value) => FindorAddLine(lineName).info.SetDateTime(value);

        #endregion

        #region GOBJ

        /// <summary>
        /// 获取或设置 String 属性的line
        /// </summary>
        /// <param name="lineName">(gstr)用于定义匹配的名称</param>
        /// <returns>获取或设置对 String 属性的Line</returns>
        public string? this[gstr lineName]
        {
            get => GetString((string)lineName);
            set => SetString((string)lineName, value);
        }
        /// <summary>
        /// 获取或设置 Bool 属性的line
        /// </summary>
        /// <param name="lineName">(gbol)用于定义匹配的名称</param>
        /// <returns>获取或设置对 bool 属性的Line</returns>
        public bool this[gbol lineName]
        {
            get => GetBool((string)lineName);
            set => SetBool((string)lineName, value);
        }

        /// <summary>
        /// 获取或设置 Int 属性的line
        /// </summary>
        /// <param name="lineName">(gint)用于定义匹配的名称</param>
        /// <returns>获取或设置对 int 属性的Line</returns>
        public int this[gint lineName]
        {
            get => GetInt((string)lineName);
            set => SetInt((string)lineName, value);
        }

        /// <summary>
        /// 获取或设置 Long 属性的line
        /// </summary>
        /// <param name="lineName">(gi64)用于定义匹配的名称</param>
        /// <returns>获取或设置对 long 属性的Line</returns>
        public long this[gi64 lineName]
        {
            get => GetInt64((string)lineName);
            set => SetInt64((string)lineName, value);
        }

        /// <summary>
        /// 获取或设置 Double 属性的line
        /// </summary>
        /// <param name="lineName">(gdbe)用于定义匹配的名称</param>
        /// <returns>获取或设置对 double 属性的Line</returns>
        public double this[gdbe lineName]
        {
            get => GetDouble((string)lineName);
            set => SetDouble((string)lineName, value);
        }

        /// <summary>
        /// 获取或设置 Double(long) 属性的line  通过转换long获得更精确的小数,小数位最大保留9位
        /// </summary>
        /// <param name="lineName">(gflt)用于定义匹配的名称</param>
        /// <returns>获取或设置对 double 属性的line</returns>
        public double this[gflt lineName]
        {
            get => GetFloat((string)lineName);
            set => SetFloat((string)lineName, value);
        }

        /// <summary>
        /// 获取或设置 DateTime 属性的line
        /// </summary>
        /// <param name="lineName">(gdbe)用于定义匹配的名称</param>
        /// <returns>获取或设置对 double 属性的line</returns>
        public DateTime this[gdat lineName]
        {
            get => GetDateTime((string)lineName);
            set => SetDateTime((string)lineName, value);
        }
        #endregion

        #region Enumerable
        /// <summary>
        /// 搜索相同Line,并返回整个Assemblage中第一个匹配的Line从零开始的索引 (错误:字典没有引索)
        /// </summary>
        /// <param name="line">用于定义匹配的Line</param>
        /// <returns>如果找到相同名称的Line的第一个元素,则为该元素的从零开始的索引; 否则为 -1</returns>
        [Obsolete] public int IndexOf(TLine line) => throw new ArrayTypeMismatchException();
        /// <summary>
        /// 将指定的Line添加到指定索引处 (失效:字典没有顺序)
        /// </summary>
        /// <param name="index">应插入 Line 的从零开始的索引</param>
        /// <param name="newLine">要添加的Line</param>
        [Obsolete] public void Insert(int index, TLine newLine) => AddLine(newLine);
        /// <summary>
        /// 将指定的Line添加到Assemblage列表
        /// </summary>
        /// <param name="newLine">要添加的Line</param>
        public void Add(TLine newLine) => AddLine(newLine);
        /// <summary>
        /// 移除Assemblage中所有的Line
        /// </summary>
        public void Clear() => Assemblage.Clear();
        /// <summary>
        /// 将整个array复制到Assemblage
        /// </summary>
        /// <param name="array">复制到Assemblage的Line列表</param>
        /// <param name="arrayIndex">从零开始的引索,从引索处开始复制</param>
        public void CopyTo(TLine[] array, int arrayIndex)
        {
            for (int i = arrayIndex; i < array.Length; i++)
                Assemblage[array[i].Name] = array[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Assemblage).GetEnumerator();
        }
        /// <summary>
        /// 返回一个新List,包含所有Line
        /// </summary>
        /// <returns>所有储存的Line</returns>
        public List<TLine> ToList()
        {
            return Assemblage.Values.ToList();
        }

        #endregion
    }
}

