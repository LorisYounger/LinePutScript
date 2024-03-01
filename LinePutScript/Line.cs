using LinePutScript.Dictionary;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static LinePutScript.Sub;
#nullable enable
namespace LinePutScript
{
    /// <summary>
    /// 行 包含多个子类 继承自子类
    /// </summary>
    public class Line<T> : Sub, ILine, IReadOnlyList<ISub>, IReadOnlyCollection<ISub> where T : IList<ISub>, new()
    {
        /// <summary>
        /// 创建一行
        /// </summary>
        public Line() { }
        /// <summary>
        /// 通过lpsLine文本创建一行
        /// </summary>
        /// <param name="lpsLine">lpsSub文本</param>
        public Line(string lpsLine)
        {
            Load(lpsLine);
        }
        /// <summary>
        /// 通过其他Line创建新的Line
        /// </summary>
        /// <param name="line">其他line</param>
        public Line(ILine line)
        {
            Name = line.Name;
            Info = (SetObject)line.Info;
            text = line.text;
            foreach (ISub sub in line.ToList())
            {
                Subs.Add(sub);
            }
        }
        /// <summary>
        /// 通过名字和信息创建新的Line
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="info">信息 (正常)</param>
        /// <param name="text">文本 在末尾没有结束行号的文本 (正常)</param>
        /// <param name="subs">子类集合</param>
        public Line(string name, string info, string text = "", params ISub[] subs) : base(name, info)
        {
            Text = text;
            foreach (ISub sub in subs)
            {
                Subs.Add(sub);
            }
        }

        /// <summary>
        /// 通过名字和信息创建新的Line
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="info">信息 (正常)</param>
        /// <param name="text">文本 在末尾没有结束行号的文本 (正常)</param>
        /// <param name="subs">子类集合</param>
        public Line(string name, string info, IEnumerable<ISub> subs, string text = "") : base(name, info)
        {
            Text = text;
            foreach (ISub sub in subs)
            {
                Subs.Add(sub);
            }
        }

        /// <summary>
        /// 加载 通过lps文本创建一个子类
        /// </summary>
        /// <param name="lps">lps文本</param>
        public override void Load(string lps)
        {
            string[] sts = Split(lps, 2, StringSplitOptions.RemoveEmptyEntries, "///");
            if (sts.Length == 2)
                Comments = sts[1];
            sts = Split(sts[0], separatorArray: ":|");
            string[] st = sts[0].Split(new char[1] { '#' }, 2);//第一个
            Name = st[0];
            info = new SetObject();
            if (st.Length > 1)
                info.SetString(st[1]);//lpstext都是转义后(无关键字)

            text = sts.Last();//最后一个

            for (int i = 1; i < sts.Length - 1; i++)
            {
                Sub t = new Sub();
                t.Load(sts[i]);
                Subs.Add(t);
            }
        }
        /// <summary>
        /// 通过名字和信息创建新的Line
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="info">信息 (正常)</param>
        /// <param name="text">文本 在末尾没有结束行号的文本 (正常)</param>
        /// <param name="subs">子类集合</param>
        public void Load(string name, string info, IEnumerable<ISub> subs, string text = "")
        {
            base.Load(name, info);
            Text = text;
            foreach (ISub sub in subs)
            {
                Subs.Add(sub);
            }
        }
        /// <summary>
        /// 通过名字和信息创建新的Line
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="info">信息 (正常)</param>
        /// <param name="text">文本 在末尾没有结束行号的文本 (正常)</param>
        /// <param name="subs">子类集合</param>
        public void Load(string name, string info, string text = "", params ISub[] subs)
        {
            base.Load(name, info);
            Text = text;
            foreach (ISub sub in subs)
            {
                Subs.Add(sub);
            }
        }
        /// <summary>
        /// 将其他Line内容拷贝到本Line
        /// </summary>
        /// <param name="line">其他line</param>
        public void Load(ILine line)
        {
            Name = line.Name;
            Info = (SetObject)line.Info;

            text = line.text;
            foreach (ISub sub in line.ToList())
            {
                Subs.Add(sub);
            }
        }


        /// <summary>
        /// 文本 在末尾没有结束行号的文本 (去除关键字的文本)
        /// </summary>
        public string text { get; set; } = "";
        /// <summary>
        /// 文本 在末尾没有结束行号的文本 (正常)
        /// </summary>
        public string Text
        {
            get => TextDeReplace(text);
            set
            {
                text = TextReplace(value);
            }
        }
        /// <summary>
        /// 获得Text的String结构
        /// </summary>
        public StringStructure Texts
        {
            get
            {
                texts ??= new StringStructure((x) => text = x, () => text);
                return texts;
            }
        }
        StringStructure? texts;

        /// <summary>
        /// 注释 ///为注释
        /// </summary>
        public string Comments { get; set; } = "";
        /// <summary>
        /// 文本 (int)
        /// </summary>
        public int TextToInt
        {
            get
            {
                if (int.TryParse(text, out int i))
                    return i;
                else
                    return 0;
            }
            set
            {
                text = value.ToString();
            }
        }
        /// <summary>
        /// 文本 (int64)
        /// </summary>
        public long TextToInt64
        {
            get
            {
                if (long.TryParse(text, out long i))
                    return i;
                else
                    return 0;
            }
            set
            {
                text = value.ToString();
            }
        }
        /// <summary>
        /// 文本 (double)
        /// </summary>
        public double TextToDouble
        {
            get
            {
                if (double.TryParse(text, out double i))
                    return i;
                else
                    return 0;
            }
            set
            {
                text = value.ToString();
            }
        }
        /// <summary>
        /// 退回Text的反转义文本 (正常显示)
        /// </summary>
        /// <returns>Text的反转义文本 (正常显示)</returns>
        public string GetText()
        {
            return Text;
        }
        /// <summary>
        /// 子项目
        /// </summary>
        public T Subs { get; set; } = new T();

        #region List操作
        /// <summary>
        /// 将指定的Sub添加到Subs列表的末尾
        /// </summary>
        /// <param name="newSub">要添加的Sub</param>
        public void AddSub(ISub newSub)
        {
            Subs.Add(newSub);
        }
        /// <summary>
        /// 若无相同名称(Name)的Sub,则将指定的Sub添加到Subs列表的末尾
        /// 若有,则替换成要添加的Sub
        /// </summary>
        /// <param name="newSub">要添加的Sub</param>
        public void AddorReplaceSub(ISub newSub)
        {
            ISub? oldsub = Find(newSub.Name);
            if (oldsub != null)
            {
                oldsub.Set(newSub);
            }
            else
                Subs.Add(newSub);
        }
        /// <summary>
        /// 将指定Sub的元素添加到Subs的末尾
        /// </summary>
        /// <param name="newSubs">要添加的多个Sub</param>
        public void AddRange(IEnumerable<ISub> newSubs)
        {
            foreach (ISub sub in newSubs)
            {
                Subs.Add(sub);
            }
        }

        /// <summary>
        /// 将指定的Sub添加到指定索引处
        /// </summary>
        /// <param name="index">应插入 Sub 的从零开始的索引</param>
        /// <param name="newSub">要添加的Sub</param>
        public void InsertSub(int index, ISub newSub)
        {
            Subs.Insert(index, newSub);
        }
        /// <summary>
        /// 将指定Sub的元素添加指定索引处
        /// </summary>
        /// <param name="index">应插入 Sub 的从零开始的索引</param>
        /// <param name="newSubs">要添加的多个Sub</param>
        public void InsertRange(int index, IEnumerable<ISub> newSubs)
        {
            foreach (ISub sub in newSubs)
            {
                Subs.Insert(index, sub);
                index++;
            }
        }
        /// <summary>
        /// 从Subs中移除特定对象的第一个匹配项
        /// </summary>
        /// <param name="Sub">要从Subs中删除的Sub</param>
        /// <returns>如果成功移除了Sub,则为 true; 否则为 false</returns>
        public bool Remove(ISub Sub)
        {
            return Subs.Remove(Sub);
        }
        /// <summary>
        /// 从Subs中移除特定名称的第一个匹配项
        /// </summary>
        /// <param name="SubName">要从Subs中删除的Sub的名称</param>
        /// <returns>如果成功移除了Sub,则为 true; 否则为 false</returns>
        public bool Remove(string SubName)
        {
            for (int i = 0; i < Subs.Count; i++)
            {
                if (Subs[i].Name == SubName)
                {
                    Subs.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 从Subs中移除特定名称的所有元素
        /// </summary>
        /// <param name="SubName">要从Subs中删除的Sub的名称</param>
        public void RemoveAll(string SubName)
        {
            for (int i = 0; i < Subs.Count; i++)
            {
                if (Subs[i].Name == SubName)
                {
                    Subs.RemoveAt(i--);
                }
            }
        }
        /// <summary>
        /// 确定某Sub是否在Line集合中
        /// </summary>
        /// <param name="sub">要在Line集合中定位的Sub</param>
        /// <returns>如果在Line集合中找到sub,则为True; 否则为false</returns>
        public bool Contains(ISub sub)
        {
            return Subs.Contains(sub);
        }
        /// <summary>
        /// 返回一个值,确定某Sub是否在Line集合中
        /// </summary>
        /// <param name="value">要在Line集合中定位的Sub的名字</param>
        /// <returns>如果在Line集合中找到符合的名字,则为True; 否则为false</returns>
        public bool Contains(string value)
        {
            return (Subs.FirstOrDefault(x => x.Name == value) != null);
        }

        /// <summary>
        /// 匹配拥有相同名称的Line或sub的所有元素
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的sub,其中所有元素均与指定谓词定义的条件匹配,则为该数组; 否则为一个空的Array</returns>
        public ISub[] FindAll(string subName) => Subs.Where(x => x.Name == subName).ToArray();
        /// <summary>
        /// 匹配拥有相同名称和信息的Line或sub的所有元素
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <param name="subinfo">用于定义匹配的信息 (去除关键字的文本)</param>
        /// <returns>如果找到相同名称和信息的sub,其中所有元素均与指定谓词定义的条件匹配,则为该数组; 否则为一个空的Array</returns>
        public ISub[] FindAll(string subName, string subinfo) => Subs.Where(x => x.Name == subName && x.infoComparable.Equals(subinfo)).ToArray();
        /// <summary>
        /// 匹配拥有相同信息的Line或sub的所有元素
        /// </summary>
        /// <param name="subinfo">用于定义匹配的信息 (去除关键字的文本)</param>
        /// <returns>如果找到相同信息的sub,其中所有元素均与指定谓词定义的条件匹配,则为该数组; 否则为一个空的Array</returns>
        public ISub[] FindAllInfo(string subinfo) => Subs.Where(x => x.infoComparable.Equals(subinfo)).ToArray();
        /// <summary>
        /// 搜索与指定名称,并返回Line或整个Subs中的第一个匹配元素
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的第一个sub,则为该sub; 否则为null</returns>
        public ISub? Find(string subName)
        {
            return Subs.FirstOrDefault(x => x.Name == subName);
        }
        /// <summary>
        /// 搜索与指定名称,并返回整个Assemblage中的第一个匹配元素
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <param name="subinfo">用于定义匹配的信息 (去除关键字的文本)</param>
        /// <returns>如果找到相同名称和信息的第一个Line,则为该Line; 否则为null</returns>
        public ISub? Find(string subName, string subinfo)
        {
            return Subs.FirstOrDefault(x => x.Name == subName && x.infoComparable.Equals(subinfo));
        }
        /// <summary>
        /// 搜索与指定信息,并返回整个Assemblage中的第一个匹配元素
        /// </summary>
        /// <param name="subinfo">用于定义匹配的信息 (去除关键字的文本)</param>
        /// <returns>如果找到相同信息的第一个Line,则为该Line; 否则为null</returns>
        public ISub? FindInfo(string subinfo)
        {
            return Subs.FirstOrDefault(x => x.infoComparable.Equals(subinfo));
        }
        /// <summary>
        /// 搜索与指定名称,并返回Line或整个Subs中的第一个匹配元素;若未找到,则新建并添加相同名称的Sub,并且返回这个Sub
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的第一个sub,则为该sub; 否则为新建的相同名称sub</returns>
        public ISub FindorAdd(string subName)
        {
            ISub? sub = Find(subName);
            if (sub == null)
            {
                sub = new Sub();
                sub.Name = subName;
                AddSub(sub);
                return sub;
            }
            else
            {
                return sub;
            }
        }


        /// <summary>
        /// 搜索全部相似名称的Sub的所有元素
        /// </summary>
        /// <param name="value">%字段%</param>
        /// <returns>如果找到相似名称的Sub,则为数组; 否则为一个空的Array</returns>
        public ISub[] SeachALL(string value) => Subs.Where(x => x.Name.Contains(value)).ToArray();
        /// <summary>
        /// 搜索字段是否出现在Line名称,并返回整个Subs中的第一个匹配元素
        /// </summary>
        /// <param name="value">%字段%</param>
        /// <returns>如果找到相似名称的第一个Sub,则为该Sub; 否则为null</returns>
        public ISub? Seach(string value)
        {
            return Subs.FirstOrDefault(x => x.Name.Contains(value));
        }
        /// <summary>
        /// 搜索相同名称的Sub,并返回整个Subs中第一个匹配的sub从零开始的索引
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的sub的第一个元素,则为该元素的从零开始的索引; 否则为 -1</returns>
        public int IndexOf(string subName)
        {
            ISub? sub = Subs.FirstOrDefault(x => x.Name == subName);
            if (sub == null)
            {
                return -1;
            }
            else
            {
                return Subs.IndexOf(sub);
            }
        }
        /// <summary>
        /// 搜索相同名称的Sub,并返回整个Sub中全部匹配的sub从零开始的索引
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的sub的元素,则为该元素的从零开始的索引组; 否则为空的Array</returns>
        public int[] IndexsOf(string subName)
        {
            List<int> lines = new List<int>();
            for (int i = 0; i < Subs.Count; i++)
            {
                if (Subs[i].Name == subName)
                    lines.Add(i);
            }
            return lines.ToArray();
        }
        #endregion

        /// <summary>
        /// 将当前Line转换成文本格式 (info已经被转义/去除关键字)
        /// </summary>
        /// <returns>Line的文本格式 (info已经被转义/去除关键字)</returns>
        public override string ToString()
        {
            StringBuilder str = new StringBuilder(Name);
            string infostorestr = info.GetStoreString();
            if (infostorestr != "")
                str.Append('#' + infostorestr);
            if (str.Length != 0)
                str.Append(":|");
            foreach (ISub su in Subs)
                str.Append(su.ToString());
            str.Append(text);
            if (Comments != "")
            {
                str.Append("///");
                str.Append(Comments);
            }
            return str.ToString();
        }
        /// <summary>
        /// 将当前Line转换成文本格式 (info已经被转义/去除关键字) 将输出储存到StringBuilder
        /// </summary>
        /// <param name="str">储存到的 StringBuilder</param>
        /// <returns>Line的文本格式 (info已经被转义/去除关键字)</returns>
        public void ToString(StringBuilder str)
        {
            str.Append('\n' + Name);
            string infostorestr = info.GetStoreString();
            if (infostorestr != "")
                str.Append('#' + infostorestr);
            if (str.Length != 0)
                str.Append(":|");
            foreach (ISub su in Subs)
                str.Append(su.ToString());
            str.Append(text);
            if (Comments != "")
            {
                str.Append("///");
                str.Append(Comments);
            }
        }

        /// <summary>
        /// 获得该Line的长哈希代码
        /// </summary>
        /// <returns>64位哈希代码</returns>
        public override long GetLongHashCode()
        {
            long hash = GetHashCode(Name) * 2 + GetHashCode(info) * 3 + GetHashCode(text) * 4;
            foreach (ISub su in Subs)
            {
                hash += su.GetLongHashCode();
            }
            return hash;
        }
        /// <summary>
        /// 获得该Line的哈希代码
        /// </summary>
        /// <returns>32位哈希代码</returns>
        public override int GetHashCode() => (int)GetLongHashCode();
        /// <summary>
        /// 返回循环访问 Subs 的枚举数。
        /// </summary>
        /// <returns>用于 Subs 的枚举数</returns>
        public new IEnumerator<ISub> GetEnumerator()
        {
            return Subs.GetEnumerator();
        }
        /// <summary>
        /// 返回一个 Subs 的第一个元素。
        /// </summary>
        /// <returns>要返回的第一个元素</returns>
        public new ISub? First()
        {
            if (Subs.Count == 0)
                return default;
            return Subs[0];
        }
        /// <summary>
        /// 返回一个 Subs 的最后一个元素。
        /// </summary>
        /// <returns>要返回的最后一个元素</returns>
        public new ISub? Last()
        {
            if (Subs.Count == 0)
                return default;
            return Subs[Subs.Count - 1];
        }

        #region GETER
        /// <summary>
        /// 搜索与指定名称,并返回Line或整个Subs中的第一个匹配元素;若未找到,则新建并添加相同名称的Sub,并且返回这个Sub
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的第一个sub,则为该sub; 否则为新建的相同名称sub</returns>
        public ISub this[string subName]
        {
            get
            {
                return FindorAdd(subName);
            }
            set
            {
                AddorReplaceSub(value);
            }
        }
        /// <summary>
        /// 获得bool属性的sub
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的sub,则为True; 否则为false</returns>
        public bool GetBool(string subName)
        {
            ISub? sub = Find(subName);
            if (sub == null)
                return false;
            return sub.InfoToBoolean;
        }
        /// <summary>
        /// 设置bool属性的sub
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <param name="value">
        /// 值
        /// </param>
        public void SetBool(string subName, bool value)
        {
            FindorAdd(subName).InfoToBoolean = value;
        }
        /// <summary>
        /// 获得int属性的sub
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <param name="defaultvalue">如果没找到返回的默认值</param>
        /// <returns>
        /// 如果找到相同名称的sub,返回sub中储存的int值
        /// 如果没找到,则返回默认值
        /// </returns>
        public int GetInt(string subName, int defaultvalue = default)
        {
            ISub? sub = Find(subName);
            if (sub == null)
                return defaultvalue;
            return sub.InfoToInt;
        }
        /// <summary>
        /// 设置int属性的sub
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <param name="value">储存进sub的int值</param>
        public void SetInt(string subName, int value) => FindorAdd(subName).InfoToInt = value;

        /// <summary>
        /// 获得long属性的sub
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <param name="defaultvalue">如果没找到返回的默认值</param>
        /// <returns>
        /// 如果找到相同名称的sub,返回sub中储存的long值
        /// 如果没找到,则返回默认值
        /// </returns>
        public long GetInt64(string subName, long defaultvalue = default)
        {
            ISub? sub = Find(subName);
            if (sub == null)
                return defaultvalue;
            return sub.InfoToInt64;
        }
        /// <summary>
        /// 设置long属性的sub
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <param name="value">储存进sub的long值</param>
        public void SetInt64(string subName, long value) => FindorAdd(subName).InfoToInt64 = value;

        /// <summary>
        /// 获得double(long)属性的sub 通过转换long获得更精确的小数,小数位最大保留9位
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <param name="defaultvalue">如果没找到返回的默认值</param>
        /// <returns>
        /// 如果找到相同名称的sub,返回sub中储存的double(long)值
        /// 如果没找到,则返回默认值
        /// </returns>
        public FInt64 GetFloat(string subName, FInt64 defaultvalue = default)
        {
            ISub? sub = Find(subName);
            if (sub == null)
                return defaultvalue;
            return sub.GetFloat();
        }
        /// <summary>
        /// 设置double(long)属性的sub 通过转换long获得更精确的小数,小数位最大保留9位
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <param name="value">储存进sub的double(long)值</param>
        public void SetFloat(string subName, FInt64 value) => FindorAdd(subName).SetFloat(value);

        /// <summary>
        /// 获得DateTime属性的sub
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <param name="defaultvalue">如果没找到返回的默认值</param>
        /// <returns>
        /// 如果找到相同名称的sub,返回sub中储存的DateTime值
        /// 如果没找到,则返回默认值
        /// </returns>
        public DateTime GetDateTime(string subName, DateTime defaultvalue = default)
        {
            ISub? sub = Find(subName);
            if (sub == null)
                return defaultvalue;
            return sub.GetDateTime();
        }
        /// <summary>
        /// 设置DateTime属性的sub
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <param name="value">储存进sub的DateTime值</param>
        public void SetDateTime(string subName, DateTime value) => FindorAdd(subName).SetDateTime(value);

        /// <summary>
        /// 获得String属性的sub
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <param name="defaultvalue">如果没找到返回的默认值</param>
        /// <returns>
        /// 如果找到相同名称的sub,返回sub中储存的String值
        /// 如果没找到,则返回默认值
        /// </returns>
        public string? GetString(string subName, string? defaultvalue = default)
        {
            ISub? sub = Find(subName);
            if (sub == null)
                return defaultvalue;
            return sub.Info;
        }
        /// <summary>
        /// 设置String属性的sub
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <param name="value">储存进sub的String值</param>
        public void SetString(string subName, string? value) => FindorAdd(subName).Info = value ?? "";

        /// <summary>
        /// 获得double属性的sub
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <param name="defaultvalue">如果没找到返回的默认值</param>
        /// <returns>
        /// 如果找到相同名称的sub,返回sub中储存的double值
        /// 如果没找到,则返回默认值
        /// </returns>
        public double GetDouble(string subName, double defaultvalue = default)
        {
            ISub? sub = Find(subName);
            if (sub == null)
                return defaultvalue;
            return sub.InfoToDouble;
        }
        /// <summary>
        /// 设置double属性的sub
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <param name="value">储存进sub的double值</param>
        public void SetDouble(string subName, double value) => FindorAdd(subName).InfoToDouble = value;
        #endregion

        #region GOBJ

        /// <summary>
        /// 获取或设置 String 属性的sub
        /// </summary>
        /// <param name="subName">(gstr)用于定义匹配的名称</param>
        /// <returns>获取或设置对 String 属性的Sub</returns>
        public string? this[gstr subName]
        {
            get => GetString((string)subName);
            set => SetString((string)subName, value);
        }
        /// <summary>
        /// 获取或设置 Bool 属性的sub
        /// </summary>
        /// <param name="subName">(gbol)用于定义匹配的名称</param>
        /// <returns>获取或设置对 bool 属性的Sub</returns>
        public bool this[gbol subName]
        {
            get => GetBool((string)subName);
            set => SetBool((string)subName, value);
        }

        /// <summary>
        /// 获取或设置 Int 属性的sub
        /// </summary>
        /// <param name="subName">(gint)用于定义匹配的名称</param>
        /// <returns>获取或设置对 int 属性的Sub</returns>
        public int this[gint subName]
        {
            get => GetInt((string)subName);
            set => SetInt((string)subName, value);
        }

        /// <summary>
        /// 获取或设置 Long 属性的sub
        /// </summary>
        /// <param name="subName">(gi64)用于定义匹配的名称</param>
        /// <returns>获取或设置对 long 属性的Sub</returns>
        public long this[gi64 subName]
        {
            get => GetInt64((string)subName);
            set => SetInt64((string)subName, value);
        }

        /// <summary>
        /// 获取或设置 Double 属性的sub
        /// </summary>
        /// <param name="subName">(gdbe)用于定义匹配的名称</param>
        /// <returns>获取或设置对 double 属性的Sub</returns>
        public double this[gdbe subName]
        {
            get => GetDouble((string)subName);
            set => SetDouble((string)subName, value);
        }

        /// <summary>
        /// 获取或设置 Double(long) 属性的sub  通过转换long获得更精确的小数,小数位最大保留9位
        /// </summary>
        /// <param name="subName">(gflt)用于定义匹配的名称</param>
        /// <returns>获取或设置对 double 属性的Sub</returns>
        public FInt64 this[gflt subName]
        {
            get => GetFloat((string)subName);
            set => SetFloat((string)subName, value);
        }

        /// <summary>
        /// 获取或设置 DateTime 属性的sub
        /// </summary>
        /// <param name="subName">(gdbe)用于定义匹配的名称</param>
        /// <returns>获取或设置对 double 属性的Sub</returns>
        public DateTime this[gdat subName]
        {
            get => GetDateTime((string)subName);
            set => SetDateTime((string)subName, value);
        }
        #endregion

        #region Enumerable

        /// <summary>
        /// 获取Sub数量
        /// </summary>
        public int Count => Subs.Count;
        /// <summary>
        /// 是否只读
        /// </summary>
        public bool IsReadOnly => ((ICollection<Sub>)Subs).IsReadOnly;
        /// <summary>
        /// 通过引索修改Line中Sub内容
        /// </summary>
        /// <param name="index">要获得或设置的引索</param>
        /// <returns>引索指定的Sub</returns>
        public ISub this[int index] { get => Subs[index]; set => Subs[index] = value; }
        /// <summary>
        /// 搜索相同名称的Sub,并返回整个Subs中第一个匹配的Sub从零开始的索引
        /// </summary>
        /// <param name="sub">用于定义匹配的Sub</param>
        /// <returns>如果找到相同名称的Sub的第一个元素,则为该元素的从零开始的索引; 否则为 -1</returns>
        public int IndexOf(ISub sub) => Subs.IndexOf(sub);
        /// <summary>
        /// 将指定的Sub添加到指定索引处
        /// </summary>
        /// <param name="index">应插入 Sub 的从零开始的索引</param>
        /// <param name="newSub">要添加的Sub</param>
        public void Insert(int index, ISub newSub) => Subs.Insert(index, newSub);
        /// <summary>
        /// 从Subs中移除特定引索的Sub
        /// </summary>
        /// <param name="index">要删除Sub的引索</param>
        public void RemoveAt(int index) => Subs.RemoveAt(index);
        /// <summary>
        /// 将指定的Sub添加到Subs列表的末尾
        /// </summary>
        /// <param name="newSub">要添加的Sub</param>
        public void Add(ISub newSub) => AddSub(newSub);
        /// <summary>
        /// 移除Line中所有的Sub
        /// </summary>
        public void Clear() => Subs.Clear();
        /// <summary>
        /// 将整个array复制到Line的Subs
        /// </summary>
        /// <param name="array">复制到Subs的Sub列表</param>
        /// <param name="arrayIndex">从零开始的引索,从引索处开始复制</param>
        public void CopyTo(ISub[] array, int arrayIndex) => Subs.CopyTo(array, arrayIndex);



        /// <summary>
        /// 将当前line与另一个line进行比较,并退回一个整数指示在排序位置中是位于另一个对象之前之后还是相同位置
        /// </summary>
        /// <param name="other">另一个line</param>
        /// <returns>值小于零时排在 other 之前 值等于零时出现在相同排序位置 值大于零则排在 other 之后</returns>
        public int CompareTo(ILine? other)
        {
            if (other == null)
                return -1;
            int comp = Name.CompareTo(other.Name);
            if (comp != 0)
                return comp;
            try
            {
                return other.infoComparable.CompareTo(info);
            }
            catch
            {
                return ToString().CompareTo(other?.ToString());
            }
        }
        /// <summary>
        /// 将当前line与另一个line进行比较, 判断是否内容相同
        /// </summary>
        /// <param name="other">另一个line</param>
        /// <returns>如果为True则内容相同</returns>
        public bool Equals(ILine? other) => CompareTo(other) == 0;
        /// <summary>
        /// 克隆一个Line
        /// </summary>
        /// <returns>相同的Line</returns>
        public override object Clone()
        {
            return new Line<T>(this);
        }
        /// <summary>
        /// 返回一个新List,包含所有Subs
        /// </summary>
        /// <returns>所有储存的Subs</returns>
        public List<ISub> ToList()
        {
            return Subs.ToList();
        }
        #endregion
    }
    /// <summary>
    /// 行 包含多个子类 继承自子类
    /// </summary>
    public class Line : Line<List<ISub>>
    {
        /// <summary>
        /// 创建一行
        /// </summary>
        public Line() { }
        /// <summary>
        /// 通过lpsLine文本创建一行
        /// </summary>
        /// <param name="lpsLine">lpsSub文本</param>
        public Line(string lpsLine) : base(lpsLine) { }
        /// <summary>
        /// 通过其他Line创建新的Line
        /// </summary>
        /// <param name="line">其他line</param>
        public Line(ILine line) : base(line) { }
        /// <summary>
        /// 通过名字和信息创建新的Line
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="info">信息 (正常)</param>
        /// <param name="text">文本 在末尾没有结束行号的文本 (正常)</param>
        /// <param name="subs">子类集合</param>
        public Line(string name, string info, string text = "", params ISub[] subs) : base(name, info, text, subs) { }

        /// <summary>
        /// 通过名字和信息创建新的Line
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="info">信息 (正常)</param>
        /// <param name="text">文本 在末尾没有结束行号的文本 (正常)</param>
        /// <param name="subs">子类集合</param>
        public Line(string name, string info, IEnumerable<ISub> subs, string text = "") : base(name, info, subs, text) { }
    }
}
