using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
#nullable enable
namespace LinePutScript
{

    /// <summary>
    /// 子类 LinePutScript最基础的类
    /// </summary>
    public class Sub : ICloneable, IEnumerable, IComparable<Sub>, IEquatable<Sub>
    {
        /// <summary>
        /// 创建一个子类
        /// </summary>
        public Sub()
        {
            Name = "";
        }
        /// <summary>
        /// 通过lpsSub文本创建一个子类
        /// </summary>
        /// <param name="lpsSub">lpsSub文本</param>
        public Sub(string lpsSub)
        {
            string[] st = lpsSub.Split(new char[1] { '#' }, 2);
            Name = st[0];
            if (st.Length > 1)
                info = st[1];
        }
        /// <summary>
        /// 通过名字和信息创建新的Sub
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="info">信息 (正常版本)</param>
        public Sub(string name, string info)
        {
            Name = name;
            Info = info;
        }
        /// <summary>
        /// 通过名字和信息创建新的Sub
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="info">多个信息 (正常版本)</param>
        public Sub(string name, params string[] info)
        {
            Name = name;
            StringBuilder sb = new StringBuilder();
            foreach (string s in info)
            {
                sb.Append(TextReplace(s));
                sb.Append(',');
            }
            this.info = sb.ToString().TrimEnd(',');
        }

        /// <summary>
        /// 通过Sub创建新的Sub
        /// </summary>
        /// <param name="sub">其他Sub</param>
        public Sub(Sub sub)
        {
            Name = sub.Name;
            info = sub.info;
        }
        /// <summary>
        /// 将其他Sub内容拷贝到本Sub
        /// </summary>
        /// <param name="sub">其他Sub</param>
        public void Set(Sub sub)
        {
            Name = sub.Name;
            info = sub.info;
        }

        /// <summary>
        /// 名称 没有唯一性
        /// </summary>
        public string Name;

        /// <summary>
        /// ID 根据Name生成 没有唯一性
        /// </summary>
        public long ID
        {
            get
            {
                if (long.TryParse(Name, out long i))
                    return i;
                return Name.GetHashCode();
            }
            set
            {
                Name = value.ToString();
            }
        }

        /// <summary>
        /// 信息 (去除关键字的文本)
        /// </summary>
        public string info = "";
        /// <summary>
        /// 信息 (正常)
        /// </summary>
        public string Info
        {
            get => TextDeReplace(info);
            set
            {
                info = TextReplace(value);
            }
        }
        /// <summary>
        /// 获得Info的String结构
        /// </summary>
        public StringStructure Infos
        {
            get => new StringStructure((x) => info = x, () => info);
        }

        /// <summary>
        /// 信息 (int)
        /// </summary>
        public int InfoToInt
        {
            get
            {
                int.TryParse(info, out int i);
                return i;
            }
            set
            {
                info = value.ToString();
            }
        }
        /// <summary>
        /// 信息 (int64)
        /// </summary>
        public long InfoToInt64
        {
            get
            {
                long.TryParse(info, out long i);
                return i;
            }
            set
            {
                info = value.ToString();
            }
        }
        /// <summary>
        /// 信息 (double)
        /// </summary>
        public double InfoToDouble
        {
            get
            {
                double.TryParse(info, out double i);
                return i;
            }
            set
            {
                info = value.ToString();
            }
        }
        /// <summary>
        /// 返回循环访问 Info集合 的枚举数。
        /// </summary>
        /// <returns>用于 Info集合 的枚举数</returns>
        public IEnumerator GetEnumerator()
        {
            return GetInfos().GetEnumerator();
        }
        /// <summary>
        /// 返回一个 Info集合 的第一个string。
        /// </summary>
        /// <returns>要返回的第一个string</returns>
        public string? First()
        {
            string[] Subs = GetInfos();
            if (Subs.Length == 0)
                return null;
            return Subs[0];
        }
        /// <summary>
        /// 返回一个 Info集合 的最后一个string。
        /// </summary>
        /// <returns>要返回的最后一个string</returns>
        public string? Last()
        {
            string[] Subs = GetInfos();
            if (Subs.Length == 0)
                return null;
            return Subs[Subs.Length - 1];
        }

        /// <summary>
        /// 退回Info的反转义文本 (正常显示)
        /// </summary>
        /// <returns>info的反转义文本 (正常显示)</returns>
        public string GetInfo()
        {
            return Info;
        }
        /// <summary>
        /// 退回Info集合的转义文本集合 (正常显示)
        /// </summary>
        /// <returns>info的转义文本集合 (正常显示)</returns>
        public string[] GetInfos()
        {
            string[] sts = info.Split(',');
            for (int i = 0; i < sts.Length; i++)
                sts[i] = TextDeReplace(sts[i]);
            return sts;
        }
        
        /// <summary>
        /// 将当前Sub转换成文本格式 (info已经被转义/去除关键字)
        /// </summary>
        /// <returns>Sub的文本格式 (info已经被转义/去除关键字)</returns>
        public new string ToString()//不能继承
        {
            if (info == "")
                return TextReplace(Name) + ":|";
            return TextReplace(Name) + '#' + info + ":|";
        }

        #region Interface

        /// <summary>
        /// 获得该Sub的哈希代码
        /// </summary>
        /// <returns>32位哈希代码</returns>
        public override int GetHashCode() => (int)GetLongHashCode();
        /// <summary>
        /// 获得该Sub的长哈希代码
        /// </summary>
        /// <returns>64位哈希代码</returns>
        public long GetLongHashCode() => Name.GetHashCode() * 2 + info.GetHashCode() * 3;
        /// <summary>
        /// 确认对象是否等于当前对象
        /// </summary>
        /// <param name="obj">Subs</param>
        /// <returns></returns>
        public bool Equals(Sub? obj)
        {
            return obj?.GetLongHashCode() == GetLongHashCode();
        }
        /// <summary>
        /// 将当前sub与另一个sub进行比较,并退回一个整数指示在排序位置中是位于另一个对象之前之后还是相同位置
        /// </summary>
        /// <param name="other">另一个sub</param>
        /// <returns>值小于零时排在 other 之前 值等于零时出现在相同排序位置 值大于零则排在 other 之后</returns>
        public int CompareTo(Sub? other)
        {
            int comp = Name.CompareTo(other?.Name);
            if (comp != 0)
                return comp;
            return info.CompareTo(other?.info);
        }
        /// <summary>
        /// 克隆一个Sub
        /// </summary>
        /// <returns>相同的sub</returns>
        public object Clone()
        {
            return new Sub(this);
        }
        #endregion

        #region Function
        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="text">需要分割的文本</param>
        /// <param name="separator">分割符号</param>
        /// <param name="count">分割次数 -1 为无限次数</param>
        /// <returns></returns>
        public static List<string> Split(string text, string separator, int count = -1)
        {
            List<string> list = new List<string>();
            string lasttext = text;
            for (int i = 0; i < count || count == -1; i++)
            {
                int iof = lasttext.IndexOf(separator);
                if (iof == -1)
                {
                    break;
                }
                else
                {
                    list.Add(lasttext.Substring(0, iof));
                    lasttext = lasttext.Substring(iof + separator.Length);
                }
            }
            list.Add(lasttext);
            return list;
        }
        
        /// <summary>
        /// 将文本进行反转义处理(成为正常显示的文本)
        /// </summary>
        /// <param name="Reptex">要反转义的文本</param>
        /// <returns>反转义后的文本 正常显示的文本</returns>
        public static string TextDeReplace(string Reptex)
        {
            if (Reptex == null)
                return "";
            Reptex = Reptex.Replace("/stop", ":|");
            Reptex = Reptex.Replace("/equ", "=");
            Reptex = Reptex.Replace("/tab", "\t");
            Reptex = Reptex.Replace("/n", "\n");
            Reptex = Reptex.Replace("/r", "\r");
            Reptex = Reptex.Replace("/id", "#");
            Reptex = Reptex.Replace("/com", ",");
            Reptex = Reptex.Replace("/!", "/");
            Reptex = Reptex.Replace("/|", "|");
            return Reptex;
        }
        
        /// <summary>
        /// 将文本进行转义处理(成为去除关键字的文本)
        /// </summary>
        /// <param name="Reptex">要转义的文本</param>
        /// <returns>转义后的文本 (去除关键字的文本)</returns>
        public static string TextReplace(string Reptex)
        {
            if (Reptex == null)
                return "";
            Reptex = Reptex.Replace("|", "/|");
            Reptex = Reptex.Replace("/", "/!");
            Reptex = Reptex.Replace(":|", "/stop");
            Reptex = Reptex.Replace("\t", "/tab");
            Reptex = Reptex.Replace("\n", "/n");
            Reptex = Reptex.Replace("\r", "/r");
            Reptex = Reptex.Replace("#", "/id");
            Reptex = Reptex.Replace(",", "/com");
            Reptex = Reptex.Replace("=", "/equ");
            return Reptex;
        }
        #endregion
    }
}
