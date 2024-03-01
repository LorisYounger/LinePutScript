using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
#nullable enable
namespace LinePutScript
{
    /// <summary>
    /// 子类 LinePutScript最基础的类
    /// </summary>
    public class Sub : ISub, ICloneable, IEnumerable, IComparable<ISub>, IEquatable<ISub>
    {
        /// <summary>
        /// 创建一个子类
        /// </summary>
        public Sub()
        {
            Name = "";
            info = new SetObject();
        }
        /// <summary>
        /// 通过lpsSub文本创建一个子类
        /// </summary>
        /// <param name="lpsSub">lpsSub文本</param>
        public Sub(string lpsSub)
        {
            string[] st = lpsSub.Split(new char[1] { '#' }, 2);
            Name = st[0];
            this.info = new SetObject();
            if (st.Length > 1)
                info.SetString(st[1]);
        }
        /// <summary>
        /// 加载 通过lps文本创建一个子类
        /// </summary>
        /// <param name="lpsSub">lps文本</param>
        public virtual void Load(string lpsSub)
        {
            string[] st = lpsSub.Split(new char[1] { '#' }, 2);
            Name = st[0];
            this.info = new SetObject();
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
            this.info = new SetObject();
            this.info.SetString(Sub.TextReplace(info));
        }
        /// <summary>
        /// 通过名字和信息创建新的Sub
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="info">信息 (SetObject)</param>
        public Sub(string name, SetObject info)
        {
            Name = name;
            this.info = info;
        }
        /// <summary>
        /// 通过名字和信息创建新的Sub
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="info">信息 (正常版本)</param>
        public void Load(string name, string info)
        {
            Name = name;
            this.info = new SetObject();
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
            this.info = new SetObject();
            this.info.SetString(sb.ToString().TrimEnd(','));
        }

        /// <summary>
        /// 通过Sub创建新的Sub
        /// </summary>
        /// <param name="sub">其他Sub</param>
        public Sub(ISub sub)
        {
            Name = sub.Name;
            info = (SetObject)sub.Info;
        }
        /// <summary>
        /// 将其他Sub内容拷贝到本Sub
        /// </summary>
        /// <param name="sub">其他Sub</param>
        public void Set(ISub sub)
        {
            Name = sub.Name;
            Info = (SetObject)sub.Info;
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
                return GetHashCode(Name);
            }
            set
            {
                Name = value.ToString();
            }
        }

        /// <summary>
        /// 信息 (去除关键字的文本)
        /// </summary>
        public SetObject info { get; set; }
        string ISub.info { get => info; set => info = value; }
        ICloneable ISub.infoCloneable { get => info; }
        IComparable ISub.infoComparable { get => info; }

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
        /// 信息 (bool)
        /// </summary>
        public bool InfoToBoolean
        {
            get
            {
                return info.GetBoolean();
            }
            set
            {
                info.SetBoolean(value);
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
            string infostorestring = info.GetStoreString();
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
        public virtual long GetLongHashCode() => GetHashCode(Name) * 2 + GetHashCode(info.GetStoreString()) * 3;
        /// <summary>
        /// 确认对象是否等于当前对象
        /// </summary>
        /// <param name="obj">Subs</param>
        /// <returns></returns>
        public bool Equals(ISub? obj)
        {
            return obj?.GetLongHashCode() == GetLongHashCode();
        }
        /// <summary>
        /// 将当前sub与另一个sub进行比较,并退回一个整数指示在排序位置中是位于另一个对象之前之后还是相同位置
        /// </summary>
        /// <param name="other">另一个sub</param>
        /// <returns>值小于零时排在 other 之前 值等于零时出现在相同排序位置 值大于零则排在 other 之后</returns>
        public int CompareTo(ISub? other)
        {
            if (other == null)
                return -1;
            int comp = Name.CompareTo(other.Name);
            if (comp != 0)
                return comp;
            return Info.CompareTo(other.Info);
        }
        /// <summary>
        /// 克隆一个Sub
        /// </summary>
        /// <returns>相同的sub</returns>
        public virtual object Clone()
        {
            return new Sub(this);
        }
        #endregion

        #region IGetOBject
        /// <inheritdoc/>
        dynamic ISetObject.Value { get => info.Value; set => info.Value = value; }

        /// <inheritdoc/>
        public string GetStoreString() => info.GetStoreString();
        /// <inheritdoc/>
        public string GetString() => Info;
        /// <inheritdoc/>
        public long GetInteger64() => info.GetInteger64();
        /// <inheritdoc/>
        public int GetInteger() => info.GetInteger();
        /// <inheritdoc/>
        public double GetDouble() => info.GetDouble();
        /// <inheritdoc/>
        public FInt64 GetFloat() => info.GetFloat();
        /// <inheritdoc/>
        public DateTime GetDateTime() => info.GetDateTime();
        /// <inheritdoc/>
        public bool GetBoolean() => info.GetBoolean();
        /// <inheritdoc/>
        public void SetString(string value) => Info = value;
        /// <inheritdoc/>
        public void SetInteger(int value) => info.SetInteger(value);
        /// <inheritdoc/>
        public void SetInteger64(long value) => info.SetInteger64(value);
        /// <inheritdoc/>
        public void SetDouble(double value) => info.SetDouble(value);
        /// <inheritdoc/>
        public void SetFloat(FInt64 value) => info.SetFloat(value);
        /// <inheritdoc/>
        public void SetDateTime(DateTime value) => info.SetDateTime(value);
        /// <inheritdoc/>
        public void SetBoolean(bool value) => info.SetBoolean(value);
        /// <inheritdoc/>
        public int CompareTo(object? obj) => info.CompareTo(obj);
        /// <inheritdoc/>
        public bool Equals(ISetObject? other) => info.Equals(other);
        /// <inheritdoc/>
        public int CompareTo(ISetObject? other) => info.CompareTo(other);
        #endregion

        #region static Function
        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="text">需要分割的文本</param>
        /// <param name="separator">分割符号</param>
        /// <param name="count">分割次数 -1 为无限次数</param>
        /// <returns></returns>
        public static List<string> Split(string text, string separator, int count = -1)
        {
            return Split(text, separator, StringSplitOptions.None, count);
        }
        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="text">需要分割的文本</param>
        /// <param name="separator">分割符号</param>
        /// <param name="count">分割次数 -1 为无限次数</param>
        /// <param name="options">分割选项</param>
        /// <returns></returns>
        public static List<string> Split(string text, string separator, StringSplitOptions options, int count = -1)
        {
            string[] separatorArray = new string[] { separator };
            if (count == -1)
            {
                return new List<string>(text.Split(separatorArray, options));
            }
            else
            {
                return new List<string>(text.Split(separatorArray, count, options));
            }
        }
        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="text">需要分割的文本</param>
        /// <param name="count">分割次数 -1 为无限次数</param>
        /// <param name="options">分割选项</param>
        /// <param name="separatorArray">分割符号</param>
        /// <returns></returns>
        public static string[] Split(string text, int count = -1, StringSplitOptions options = StringSplitOptions.None, params string[] separatorArray)
        {
            if (count == -1)
            {
                return text.Split(separatorArray, options);
            }
            else
            {
                return text.Split(separatorArray, count, options);
            }
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
            //Reptex = Reptex.Replace("=", "/equ");
            return Reptex;
        }
        /// <summary>
        /// 获取String的HashCode(SHA512)
        /// </summary>
        /// <param name="text">String</param>
        /// <returns>HashCode</returns>
        public static long GetHashCode(string text)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(text);
                byte[] hashBytes = sha512.ComputeHash(bytes);

                // Take the first 8 bytes of the hash for the long value.
                long hashValue = BitConverter.ToInt64(hashBytes, 0);

                return hashValue;
            }
        }

        #endregion
    }
}
