using LinePutScript.Dictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinePutScript
{
    /// <summary>
    /// 文件 包括文件读写等一系列操作
    /// </summary>
    public class LpsDocument : ILPS, IReadOnlyList<ILine>, IReadOnlyCollection<ILine>
    {
        /// <summary>
        /// 集合 全部文件的数据
        /// </summary>
        public List<ILine> Assemblage { get; set; } = new List<ILine>();

        /// <summary>
        /// 创建一个 LpsDocument
        /// </summary>
        public LpsDocument() { }
        /// <summary>
        /// 从指定的字符串创建 LpsDocument
        /// </summary>
        /// <param name="lps">包含要加载的LPS文档的字符串</param>
        public LpsDocument(string lps)
        {
            Load(lps);
        }

        #region List操作
        /// <summary>
        /// 将指定的Line添加到Assemblage列表的末尾
        /// </summary>
        /// <param name="newLine">要添加的Line</param>
        public void AddLine(ILine newLine)
        {
            Assemblage.Add(newLine);
        }
        /// <summary>
        /// 若无相同名称(Name)的Line,则将指定的Line添加到Assemblage列表的末尾
        /// 若有,则替换成要添加的Line
        /// </summary>
        /// <param name="newLine">要添加的Line</param>
        public void AddorReplaceLine(ILine newLine)
        {
            ILine oldline = FindLine(newLine.Name);
            if (oldline != null)
            {
                oldline.Set(newLine);
            }
            else
                Assemblage.Add(newLine);
        }
        /// <summary>
        /// 将指定Line的元素添加到Assemblage的末尾
        /// </summary>
        /// <param name="newLines">要添加的多个Line</param>
        public void AddRange(params ILine[] newLines)
        {
            Assemblage.AddRange(newLines);
        }
        /// <summary>
        /// 将指定Line的元素添加到Assemblage的末尾
        /// </summary>
        /// <param name="newLines">要添加的多个Line</param>
        public void AddRange(IEnumerable<ILine> newLines)
        {
            Assemblage.AddRange(newLines);
        }
        /// <summary>
        /// 将指定的Line添加到指定索引处
        /// </summary>
        /// <param name="index">应插入 Line 的从零开始的索引</param>
        /// <param name="newLine">要添加的Line</param>
        public void InsertLine(int index, ILine newLine)
        {
            Assemblage.Insert(index, newLine);
        }
        /// <summary>
        /// 将指定Line的元素添加指定索引处
        /// </summary>
        /// <param name="index">应插入 Line 的从零开始的索引</param>
        /// <param name="newLines">要添加的多个Line</param>
        public void InsertRange(int index, params ILine[] newLines)
        {
            Assemblage.InsertRange(index, newLines);
        }
        /// <summary>
        /// 将指定Line的元素添加指定索引处
        /// </summary>
        /// <param name="index">应插入 Line 的从零开始的索引</param>
        /// <param name="newLines">要添加的多个Line</param>
        public void InsertRange(int index, IEnumerable<ILine> newLines)
        {
            Assemblage.InsertRange(index, newLines);
        }
        /// <summary>
        /// 从Assemblage中移除特定对象的第一个匹配项
        /// </summary>
        /// <param name="line">要从Assemblage中删除的Line的名称</param>
        /// <returns>如果成功移除了line,则为 true; 否则为 false</returns>
        public bool Remove(ILine line)
        {
            return Assemblage.Remove(line);
        }
        /// <summary>
        /// 从Assemblage中移除特定名称的第一个匹配项
        /// </summary>
        /// <param name="lineName">要从Assemblage中删除的Line的名称</param>
        /// <returns>如果成功移除了line,则为 true; 否则为 false</returns>
        public bool Remove(string lineName)
        {
            for (int i = 0; i < Assemblage.Count; i++)
            {
                if (Assemblage[i].Name == lineName)
                {
                    Assemblage.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 从Assemblage中移除移除与条件相匹配的所有Line
        /// </summary>
        /// <param name="line">要从Assemblage中删除的Line</param>
        public void RemoveAll(ILine line)
        {
            while (Assemblage.Remove(line)) ;
        }
        /// <summary>
        /// 从Assemblage中移除移除与名称相匹配的所有Line
        /// </summary>
        /// <param name="lineName">要从Assemblage中删除的Line的名称</param>
        public void RemoveAll(string lineName)
        {
            for (int i = 0; i < Assemblage.Count; i++)
            {
                if (Assemblage[i].Name == lineName)
                {
                    Assemblage.RemoveAt(i--);
                }
            }
        }
        /// <summary>
        /// 移除Assemblage的指定索引处的Line
        /// </summary>
        /// <param name="index">要移除的Line的从零开始的索引</param>
        public void RemoveAt(int index)
        {
            Assemblage.RemoveAt(index);
        }

        /// <summary>
        /// 确定某Line是否在Assemblage中
        /// </summary>
        /// <param name="line">要在Assemblage中定位的Line</param>
        /// <returns>如果在Assemblage中找到line,则为True; 否则为false </returns>
        public bool Contains(ILine line)
        {
            return Assemblage.Contains(line);
        }
        /// <summary>
        /// 确定某Sub是否在Assemblage中
        /// </summary>
        /// <param name="sub">要在Assemblage中定位的Sub</param>
        /// <returns>如果在Assemblage中找到line,则为True; 否则为false</returns>
        public bool Contains(ISub sub)
        {
            foreach (ILine li in Assemblage)
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
            return (Assemblage.FirstOrDefault(x => x.Name == value) != null);
        }
        /// <summary>
        /// 确定某sub(名字定位)是否在Assemblage中
        /// </summary>
        /// <param name="value">sub的名字</param>
        /// <returns>如果在Assemblage的sub中找到相同的名字,则为True; 否则为false</returns>
        public bool ContainsSub(string value)
        {
            return Assemblage.FirstOrDefault(x => x.Contains(value)) != null;

        }


        /// <summary>
        /// 匹配拥有相同名称的Line的所有元素
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的Line,其中所有元素均与指定谓词定义的条件匹配,则为该数组; 否则为一个空的Array</returns>
        public ILine[] FindAllLine(string lineName)
        {
            List<ILine> lines = new List<ILine>();
            foreach (ILine li in Assemblage)
                if (li.Name == lineName)
                    lines.Add(li);
            return lines.ToArray();
        }
        /// <summary>
        /// 匹配拥有相同名称和信息的Line的所有元素
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <param name="lineinfo">用于定义匹配的信息 (去除关键字的文本)</param>
        /// <returns>如果找到相同名称和信息的Line,其中所有元素均与指定谓词定义的条件匹配,则为该数组; 否则为一个空的Array</returns>
        public ILine[] FindAllLine(string lineName, string lineinfo)
        {
            List<ILine> lines = new List<ILine>();
            foreach (ILine li in Assemblage)
                if (li.Name == lineName && li.infoComparable.Equals(lineinfo))
                    lines.Add(li);
            return lines.ToArray();
        }
        /// <summary>
        /// 匹配拥有相同信息的Line的所有元素
        /// </summary>
        /// <param name="lineinfo">用于定义匹配的信息 (去除关键字的文本)</param>
        /// <returns>如果找到相同信息的Line,其中所有元素均与指定谓词定义的条件匹配,则为该数组; 否则为一个空的Array</returns>
        public ILine[] FindAllLineInfo(string lineinfo)
        {
            List<ILine> lines = new List<ILine>();
            foreach (ILine li in Assemblage)
                if (li.infoComparable.Equals(lineinfo))
                    lines.Add(li);
            return lines.ToArray();
        }
        /// <summary>
        /// 搜索与指定名称,并返回整个Assemblage中的第一个匹配元素
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的第一个Line,则为该Line; 否则为null</returns>
        public ILine FindLine(string lineName)
        {
            return Assemblage.FirstOrDefault(x => x.Name == lineName);
        }
        /// <summary>
        /// 搜索与指定名称和信息,并返回整个Assemblage中的第一个匹配元素
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <param name="lineinfo">用于定义匹配的信息 (去除关键字的文本)</param>
        /// <returns>如果找到相同名称和信息的第一个Line,则为该Line; 否则为null</returns>
        public ILine FindLine(string lineName, string lineinfo)
        {
            return Assemblage.FirstOrDefault(x => x.Name == lineName && x.infoComparable.Equals(lineinfo));
        }
        /// <summary>
        /// 搜索与指定信息,并返回整个Assemblage中的第一个匹配元素
        /// </summary>
        /// <param name="lineinfo">用于定义匹配的信息 (去除关键字的文本)</param>
        /// <returns>如果找到相同信息的第一个Line,则为该Line; 否则为null</returns>
        public ILine FindLineInfo(string lineinfo)
        {
            return Assemblage.FirstOrDefault(x => x.infoComparable.Equals(lineinfo));
        }
        /// <summary>
        /// 搜索与指定名称,并返回整个Assemblage中的第一个匹配元素; 若未找到,则新建并添加相同名称的Line,并且返回这个Line
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的第一个Line,则为该Line; 否则为新建的相同名称Line</returns>
        public ILine FindorAddLine<T>(string lineName) where T : ILine, new()
        {
            ILine line = FindLine(lineName);
            if (line == null)
            {
                line = new T();
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
        /// 搜索与指定名称,并返回整个Assemblage中的第一个匹配元素; 若未找到,则新建并添加相同名称的Line,并且返回这个Line
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的第一个Line,则为该Line; 否则为新建的相同名称Line</returns>
        public ILine FindorAddLine(string lineName) => FindorAddLine<Line>(lineName);
        /// <summary>
        /// 匹配拥有相同名称的Sub的所有元素
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的Sub,其中所有元素均与指定谓词定义的条件匹配,则为该数组; 否则为一个空的Array</returns>
        public ISub[] FindAllSub(string subName)
        {
            List<ISub> lines = new List<ISub>();
            foreach (ILine li in Assemblage)
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
        public ISub[] FindAllSub(string subName, string subinfo)
        {
            List<ISub> lines = new List<ISub>();
            foreach (ILine li in Assemblage)
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
        public ISub[] FindAllSubInfo(string subinfo)
        {
            List<ISub> lines = new List<ISub>();
            foreach (ILine li in Assemblage)
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
        public ISub FindSub(string subName)
        {
            foreach (ILine li in Assemblage)
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
        public ISub FindSub(string subName, string subinfo)
        {
            foreach (ILine li in Assemblage)
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
        public ISub FindSubInfo(string subinfo)
        {
            foreach (ILine li in Assemblage)
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
        public ILine[] SearchAllLine(string value)
        {
            List<ILine> lines = new List<ILine>();
            foreach (ILine li in Assemblage)
                if (li.Name.Contains(value))
                    lines.Add(li);
            return lines.ToArray();
        }
        /// <summary>
        /// 搜索字段是否出现在Line名称,并返回整个Assemblage中的第一个匹配元素
        /// </summary>
        /// <param name="value">%字段%</param>
        /// <returns>如果找到相似名称的第一个Line,则为该Line; 否则为null</returns>
        public ILine SearchLine(string value)
        {
            return Assemblage.FirstOrDefault(x => x.Name.Contains(value));
        }
        /// <summary>
        /// 搜索全部相似名称的Sub的所有元素
        /// </summary>
        /// <param name="value">%字段%</param>
        /// <returns>如果找到相似名称的Line,则为该数组; 否则为一个空的Array</returns>
        public ISub[] SearchAllSub(string value)
        {
            List<ISub> lines = new List<ISub>();
            foreach (ILine li in Assemblage)
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
        public ISub SearchSub(string value)
        {
            foreach (ILine li in Assemblage)
            {
                var l = li.Seach(value);
                if (l != null)
                    return l;
            }
            return default;
        }

        /// <summary>
        /// 搜索相同名称的Line,并返回整个Assemblage中第一个匹配的Line从零开始的索引
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的Line的第一个元素,则为该元素的从零开始的索引; 否则为 -1</returns>
        public int IndexOf(string lineName)
        {
            for (int i = 0; i < Assemblage.Count; i++)
            {
                if (Assemblage[i].Name == lineName)
                    return i;
            }
            return -1;
        }
        /// <summary>
        /// 搜索相同名称的Line,并返回整个Assemblage中全部匹配的Line从零开始的索引
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的Line的元素,则为该元素的从零开始的索引组; 否则为空的Array</returns>
        public int[] IndexsOf(string lineName)
        {
            List<int> lines = new List<int>();
            for (int i = 0; i < Assemblage.Count; i++)
            {
                if (Assemblage[i].Name == lineName)
                    lines.Add(i);
            }
            return lines.ToArray();
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
        public void Load<T>(string lps) where T : ILine, new()
        {
            Assemblage.Clear();//清空当前文档
            string[] file = lps.Replace("\r", "").Replace(":\n|", "/n").Replace(":\n:", "").Trim('\n').Split('\n');
            foreach (string str in file)
            {
                if (str != "")
                {
                    ILine t = new T();
                    t.Load(str);
                    Assemblage.Add(t);
                }
            }
        }
        /// <summary>
        /// 从指定的字符串加载LPS文档
        /// </summary>
        /// <param name="lps">包含要加载的LPS文档的字符串</param>
        public void Load(string lps) => Load<Line>(lps);

        /// <summary>
        /// 返回一个Assemblage的第一个元素。
        /// </summary>
        /// <returns>要返回的第一个元素</returns>
        public ILine First()
        {
            if (Assemblage.Count == 0)
                return default;
            return Assemblage[0];
        }
        /// <summary>
        /// 返回一个Assemblage的最后一个元素。
        /// </summary>
        /// <returns>要返回的最后一个元素</returns>
        public ILine Last()
        {
            if (Assemblage.Count == 0)
                return default;
            return Assemblage[Assemblage.Count - 1];
        }
        /// <summary>
        /// 返回循环访问 Assemblage 的枚举数。
        /// </summary>
        /// <returns>用于 Assemblage 的枚举数</returns>
        public IEnumerator<ILine> GetEnumerator()
        {
            return Assemblage.GetEnumerator();
        }

        /// <summary>
        /// 搜索与指定名称,并返回整个Assemblage中的第一个匹配元素; 若未找到,则新建并添加相同名称的Line,并且返回这个Line
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的第一个Line,则为该Line; 否则为新建的相同名称Line</returns>
        public ILine this[string lineName]
        {
            get
            {
                return FindorAddLine<Line>(lineName);
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
            foreach (ILine li in Assemblage)
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
            foreach (ILine li in Assemblage)
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
            if (obj == null || !obj.GetType().IsAssignableFrom(typeof(ILPS)))
                return false;
            return ((ILPS)obj).GetLongHashCode() == GetLongHashCode();
        }
        /// <summary>
        /// 获得当前文档大小 单位:字符
        /// </summary>
        public int Length
        {
            get
            {
                int l = 3;
                foreach (ILine li in Assemblage)
                {
                    l += li.Name.Length + li.GetStoreString().Length + li.Text.Length + 6;
                    foreach (ISub sb in li)
                    {
                        l += sb.Name.Length + sb.GetStoreString().Length + 3;
                    }
                }

                return l;
            }
        }
        /// <summary>
        /// 是否只读
        /// </summary>
        public bool IsReadOnly => ((ICollection<ILine>)Assemblage).IsReadOnly;

        /// <summary>
        /// 通过引索修改lps中Line内容
        /// </summary>
        /// <param name="index">要获得或设置的引索</param>
        /// <returns>引索指定的Line</returns>
        public ILine this[int index] { get => Assemblage[index]; set => Assemblage[index] = value; }

        #region GETER

        /// <summary>
        /// 获得bool属性的line
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的line,则为True; 否则为false</returns>
        public bool GetBool(string lineName) => FindLine(lineName)?.GetBoolean() ?? false;
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
            FindorAddLine(lineName).SetBoolean(value);
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
            ILine line = FindLine(lineName);
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
            ILine line = FindLine(lineName);
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
            ILine line = FindLine(lineName);
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
            ILine line = FindLine(lineName);
            if (line == null)
                return defaultvalue;
            return line.InfoToDouble;
        }
        /// <summary>
        /// 设置double属性的line
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <param name="value">储存进line的double值</param>
        public void SetDouble(string lineName, double value) => FindorAddLine(lineName).SetDouble(value);


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
            ILine line = FindLine(lineName);
            if (line == null)
                return defaultvalue;
            return line.GetFloat();
        }
        /// <summary>
        /// 设置double(long)属性的line 通过转换long获得更精确的小数,小数位最大保留9位
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <param name="value">储存进line的double(long)值</param>
        public void SetFloat(string lineName, double value) => FindorAddLine(lineName).SetFloat(value);

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
            ILine line = FindLine(lineName);
            if (line == null)
                return defaultvalue;
            return line.GetDateTime();
        }
        /// <summary>
        /// 设置DateTime属性的line
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <param name="value">储存进line的DateTime值</param>
        public void SetDateTime(string lineName, DateTime value) => FindorAddLine(lineName).SetDateTime(value);

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
        /// 搜索相同Line,并返回整个Assemblage中第一个匹配的Line从零开始的索引
        /// </summary>
        /// <param name="line">用于定义匹配的Line</param>
        /// <returns>如果找到相同名称的Line的第一个元素,则为该元素的从零开始的索引; 否则为 -1</returns>
        public int IndexOf(ILine line) => Assemblage.FindIndex(x => x.Equals(line));
        /// <summary>
        /// 将指定的Line添加到指定索引处
        /// </summary>
        /// <param name="index">应插入 Line 的从零开始的索引</param>
        /// <param name="newLine">要添加的Line</param>
        public void Insert(int index, ILine newLine) => InsertLine(index, newLine);
        /// <summary>
        /// 将指定的Line添加到Assemblage列表的末尾
        /// </summary>
        /// <param name="newLine">要添加的Line</param>
        public void Add(ILine newLine) => AddLine(newLine);
        /// <summary>
        /// 移除Assemblage中所有的Line
        /// </summary>
        public void Clear() => Assemblage.Clear();
        /// <summary>
        /// 将整个array复制到Assemblage
        /// </summary>
        /// <param name="array">复制到Assemblage的Line列表</param>
        /// <param name="arrayIndex">从零开始的引索,从引索处开始复制</param>
        public void CopyTo(ILine[] array, int arrayIndex) => Assemblage.CopyTo(array, arrayIndex);

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Assemblage).GetEnumerator();
        }
        /// <summary>
        /// 返回一个新List,包含所有Line
        /// </summary>
        /// <returns>所有储存的Line</returns>
        public List<ILine> ToList()
        {
            return Assemblage.ToList();
        }

        #endregion


        //就是行操作
        #region LPT操作
        int lineNode;
        /// <summary>
        /// 当前Line读取进度
        /// </summary>
        public int LineNode
        {
            get { return lineNode; }
            set
            {
                if (value > Assemblage.Count)
                    lineNode = Assemblage.Count;
                else if (value < 0)
                    lineNode = 0;
                else
                    lineNode = value;
            }
        }
        /// <summary>
        /// 读取读取进度当前Line 并且自动切换到下一Line
        /// </summary>
        /// <returns>如何有则返回当前Line,如果没有则返回null</returns>
        public ILine ReadNext()
        {
            if (LineNode == Assemblage.Count)
                return default;
            return Assemblage[LineNode++];
        }
        /// <summary>
        /// 获取读取进度当前Line
        /// </summary>
        /// <returns>如何有则返回当前Line,如果没有则返回null</returns>
        public ILine Read()
        {
            if (LineNode == Assemblage.Count)
                return default;
            return Assemblage[LineNode];
        }

        /// <summary>
        /// 将指定的Line添加到当前读取进度之后
        /// </summary>
        /// <param name="newline">要添加的Line</param>
        public void Append(Line newline)
        {
            InsertLine(LineNode + 1, newline);
        }
        /// <summary>
        /// 新建的Line添加到当前读取进度之后
        /// </summary>
        /// <param name="newlineName">要添加的行名称</param>
        /// <param name="info">行信息</param>
        /// <param name="text">行文本</param>
        /// <param name="subs">行子类</param>
        public void Append(string newlineName, string info = "", string text = "", params Sub[] subs)
        {
            Line t = new Line();
            t.Load(newlineName, info, text, subs);
            InsertLine(LineNode + 1, t);
        }
        /// <summary>
        /// 将指定的Sub添加到当前读取进度Line中
        /// </summary>
        /// <param name="newSub">要添加的子类</param>
        public void AppendSub(params Sub[] newSub)
        {
            Read()?.AddRange(newSub);
        }
        /// <summary>
        /// 将指定的Sub添加到当前读取进度Line中
        /// </summary>
        /// <param name="newSubName">要添加的行名称</param>
        /// <param name="info">要添加的行信息</param>
        public void AppendSub(string newSubName, string info = "")
        {
            Sub u = new Sub();
            u.Load(newSubName, info);
            Read()?.AddSub(u);
        }


        /// <summary>
        /// 将读取进度设置为0
        /// </summary>
        public void ReadReset()
        {
            LineNode = 0;
        }
        /// <summary>
        /// 将读取进度设置为上限 即最后一行
        /// </summary>
        public void ReadEnd()
        {
            LineNode = Assemblage.Count;
        }
        /// <summary>
        /// 判断是否能够继续读取数据
        /// </summary>
        /// <returns>如果还有下一行,返回True,否则False</returns>
        public bool ReadCanNext()
        {
            return LineNode != Assemblage.Count;
        }
        #endregion
    }
}
