using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
#nullable enable
namespace LinePutScript
{

    /// <summary>
    /// 子类 LinePutScript最基础的类
    /// </summary>
    public class Sub<V> : ISub<V>, ICloneable, IEnumerable, IComparable<ISub<V>>, IEquatable<ISub<V>> where V : ISetObject, new()
    {
        /// <summary>
        /// 创建一个子类
        /// </summary>
        public Sub()
        {
            Name = "";
            info = new V();
        }
        /// <summary>
        /// 通过lpsSub文本创建一个子类
        /// </summary>
        /// <param name="lpsSub">lpsSub文本</param>
        public Sub(string lpsSub)
        {
            string[] st = lpsSub.Split(new char[1] { '#' }, 2);
            Name = st[0];
            this.info = new V();
            if (st.Length > 1)
                info.SetString(st[1]);
        }
        /// <summary>
        /// 加载 通过lps文本创建一个子类
        /// </summary>
        /// <param name="lpsSub">lps文本</param>
        public void Load(string lpsSub)
        {
            string[] st = lpsSub.Split(new char[1] { '#' }, 2);
            Name = st[0];
            this.info = new V();
            if (st.Length > 1)
                info.SetString(st[1]);
        }
        /// <summary>
        /// 通过名字和信息创建新的Sub
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="info">信息 (正常版本)</param>
        public Sub(string name, string info)
        {
            Name = name;
            this.info = new V();
            this.info.SetString(Sub.TextReplace(info));
        }
        /// <summary>
        /// 通过名字和信息创建新的Sub
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="info">信息 (正常版本)</param>
        public void Load(string name, string info)
        {
            Name = name;
            this.info = new V();
            this.info.SetString(Sub.TextReplace(info));
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
                sb.Append(Sub.TextReplace(s));
                sb.Append(',');
            }
            this.info = new V();
            this.info.SetString(sb.ToString().TrimEnd(','));
        }

        /// <summary>
        /// 通过Sub创建新的Sub
        /// </summary>
        /// <param name="sub">其他Sub</param>
        public Sub(ISub<V> sub)
        {
            Name = sub.Name;
            info = (V)sub.info.Clone();
        }
        /// <summary>
        /// 将其他Sub内容拷贝到本Sub
        /// </summary>
        /// <param name="sub">其他Sub</param>
        public void Set(ISub<V> sub)
        {
            Name = sub.Name;
            info = (V)sub.info.Clone();
        }

        /// <summary>
        /// 名称 没有唯一性
        /// </summary>
        public string Name { get; set; }

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
        public V info { get; set; }
        /// <summary>
        /// 信息 (正常)
        /// </summary>
        public string Info
        {
            get => Sub.TextDeReplace(info.GetString());
            set
            {
                info.SetString(Sub.TextReplace(value));
            }
        }
        /// <summary>
        /// 获得Info的String结构
        /// </summary>
        public StringStructure Infos
        {
            get
            {
                infos ??= new StringStructure((x) => info.SetString(x), () => info.GetString());
                return infos;
            }
        }
        private StringStructure? infos;

        /// <summary>
        /// 信息 (int)
        /// </summary>
        public int InfoToInt
        {
            get => info.GetInteger();
            set
            {
                info.SetInteger(value);
            }
        }
        /// <summary>
        /// 信息 (int64)
        /// </summary>
        public long InfoToInt64
        {
            get => info.GetInteger64();
            set
            {
                info.SetInteger64(value);
            }
        }
        /// <summary>
        /// 信息 (double)
        /// </summary>
        public double InfoToDouble
        {
            get
            {
                return info.GetDouble();
            }
            set
            {
                info.SetDouble(value);
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
            string[] sts = info.GetString().Split(',').Where(x => !string.IsNullOrEmpty(x)).ToArray();
            for (int i = 0; i < sts.Length; i++)
                sts[i] = Sub.TextDeReplace(sts[i]);
            return sts;
        }

        /// <summary>
        /// 将当前Sub转换成文本格式 (info已经被转义/去除关键字)
        /// </summary>
        /// <returns>Sub的文本格式 (info已经被转义/去除关键字)</returns>
        public override string ToString()
        {
            var infostorestring = info.GetStoreString();
            if (infostorestring == "")
                return Name + ":|";
            return Name + '#' + infostorestring + ":|";
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
        public long GetLongHashCode() => Sub.GetHashCode(Name) * 2 + Sub.GetHashCode(info.GetString()) * 3;
        /// <summary>
        /// 确认对象是否等于当前对象
        /// </summary>
        /// <param name="obj">Subs</param>
        /// <returns></returns>
        public bool Equals(ISub<V>? obj)
        {
            return obj?.GetLongHashCode() == GetLongHashCode();
        }
        /// <summary>
        /// 将当前sub与另一个sub进行比较,并退回一个整数指示在排序位置中是位于另一个对象之前之后还是相同位置
        /// </summary>
        /// <param name="other">另一个sub</param>
        /// <returns>值小于零时排在 other 之前 值等于零时出现在相同排序位置 值大于零则排在 other 之后</returns>
        public int CompareTo(ISub<V>? other)
        {
            if (other == null)
                return int.MaxValue;
            int comp = Name.CompareTo(other.Name);
            if (comp != 0)
                return comp;
            return info.CompareTo(other.info);
        }
        /// <summary>
        /// 克隆一个Sub
        /// </summary>
        /// <returns>相同的sub</returns>
        public object Clone()
        {
            return new Sub<V>(this);
        }
        #endregion

        
    }
}
