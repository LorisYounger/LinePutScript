using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinePutScript.Converter;
#nullable enable
namespace LinePutScript
{
    /// <summary>
    /// 通过Sub包装类
    /// </summary>
    public class Sub_C<T> : ISub, ICloneable, IComparable<ISub>, IEquatable<ISub>, INotifyCollectionChanged
    {
        /// <summary>
        /// 类内容
        /// </summary>
        public T? Value
        {
            get => tvalue; set
            {
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, tvalue));
                tvalue = value;
            }
        }
        /// <summary>
        /// 行内容描述
        /// </summary>
        public LineAttribute? LineAttribute { get; set; }
        /// <summary>
        /// 类名称 (用于给父类的属性赋值)
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 新建一个Sub包装类
        /// </summary>
        /// <param name="value">要包装的值</param>
        /// <param name="attribute">行内容描述</param>
        /// <param name="name">类名称</param>
        public Sub_C(T? value, string? name = null, LineAttribute? attribute = null)
        {
            Value = value;
            if (attribute != null)
            {
                LineAttribute = attribute;
                Name = attribute.Name ?? name ?? typeof(T).Name;
            }
            else
            {
                Name = name ?? typeof(T).Name;
            }
        }
        /// <summary>
        /// 将 Value 转换成序列化值
        /// </summary>
        public string info
        {
            get => Value == null ? string.Empty : SetObject.ConvertToStoreString(Value, LineAttribute);
            set => Value = SetObject.ConvertTo(value, typeof(T), LineAttribute).TryConvertToValue<T>();
        }
        /// <summary>
        /// 将 Value 转换成序列化值
        /// </summary>
        public string Info
        {
            get => Value == null ? string.Empty : SetObject.ConvertToString(Value, LineAttribute);
            set => Value = SetObject.ConvertTo(value, typeof(T), LineAttribute).TryConvertToValue<T>();
        }
        /// <summary>
        /// 如果可克隆,则返回可克隆的本体,否则返回自身
        /// </summary>
        public ICloneable infoCloneable
        {
            get
            {
                if (Value is ICloneable)
                {
                    return (ICloneable)Value;
                }
                else
                {
                    return this;
                }
            }
        }
        /// <summary>
        /// 如果可比较,则返回可比较的本体,否则返回自身
        /// </summary>
        public IComparable infoComparable
        {
            get
            {
                if (Value is IComparable)
                {
                    return (IComparable)Value;
                }
                else
                {
                    return this;
                }
            }
        }
        /// <summary>
        /// 信息 (int)
        /// </summary>
        public int InfoToInt { get => Value.TryConvertValue(0); set => Value = value.TryConvertToValue<T>(); }
        /// <summary>
        /// 信息 (int64)
        /// </summary>
        public long InfoToInt64 { get => Value.TryConvertValue<long>(0); set => Value = value.TryConvertToValue<T>(); }
        /// <summary>
        /// 信息 (double)
        /// </summary>
        public double InfoToDouble { get => Value.TryConvertValue(0.0); set => Value = value.TryConvertToValue<T>(); }
        /// <summary>
        /// 信息 (bool)
        /// </summary>
        public bool InfoToBoolean { get => Value.TryConvertValue(false); set => Value = value.TryConvertToValue<T>(); }

        /// <summary>
        /// 获得Info的String结构
        /// </summary>
        public StringStructure Infos
        {
            get
            {
                infos ??= new StringStructure((x) => SetString(x), () => GetString());
                return infos;
            }
        }
        private StringStructure? infos;
        private T? tvalue;
        /// <inheritdoc/>

        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        dynamic ISetObject.Value { get => Value; set => Value = value; }
        /// <summary>
        /// 克隆一个Sub
        /// </summary>
        /// <returns>相同的sub</returns>
        public object Clone() => new Sub_C<T>(Value, Name, LineAttribute);
        /// <summary>
        /// 比较差距
        /// </summary>
        public int CompareTo(object? obj)
        {
            if (Value == null)
            {
                if (obj == null)
                    return 0;
                return -1;
            }
            if (obj == null)
            {
                return 1;
            }

            if (obj is ISub sub)
            {
                return CompareTo(sub);
            }
            else if (obj is ISetObject set)
            {
                return CompareTo(set);
            }
            else if (obj is T t)
            {
                return CompareTo(t);
            }
            else
            {
                //尝试都转换成字符串比较
                var a = GetStoreString();
                var b = SetObject.ConvertToStoreString(obj, LineAttribute);
                return a.CompareTo(b);
            }
        }
        /// <summary>
        /// 比较差距
        /// </summary>
        public int CompareTo(ISetObject? other)
        {
            if (Value == null)
            {
                return -1;
            }
            if (other == null)
            {
                return 1;
            }
            return -other.CompareTo(Value);
        }
        /// <summary>
        /// 判断是否一致
        /// </summary>
        public bool Equals(ISetObject? other) => CompareTo(other) == 0;
        /// <summary>
        /// 比较差距
        /// </summary>
        public int CompareTo(ISub? other)
        {
            if (other == null)
            {
                return 1;
            }
            if (Value == null)
            {
                if (other.Value == null)
                    return 0;
                return -1;
            }
            if (other is Sub_C<T> sub)
            {
                if (sub.Value == null)
                    return 1;
                return CompareTo(sub.Value);
            }
            else
            {
                //尝试都转换成字符串比较
                var a = GetStoreString();
                var b = other.GetStoreString();
                return a.CompareTo(b);
            }
        }
        /// <summary>
        /// 判断是否一致
        /// </summary>
        public bool Equals(ISub? other) => CompareTo(other) == 0;
        /// <summary>
        /// 比较差距
        /// </summary>
        public int CompareTo(T other)
        {
            if (Value == null)
            {
                return -1;
            }
            if (other == null)
            {
                return 1;
            }
            if (other is IComparable<T> comparable)
            {
                return comparable.CompareTo(other);
            }
            else
            {
                //尝试都转换成字符串比较
                var a = GetStoreString();
                var b = SetObject.ConvertToStoreString(other, LineAttribute);
                return a.CompareTo(b);
            }
        }
        /// <summary>
        /// 判断是否一致
        /// </summary>
        public bool Equals(T other) => CompareTo(other) == 0;



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


        #region IGetOBject
        /// <inheritdoc/>
        public string GetStoreString() => Value == null ? "" : SetObject.ConvertToStoreString(Value, LineAttribute);
        /// <inheritdoc/>
        public string GetString() => Info;
        /// <inheritdoc/>
        public long GetInteger64() => Value.TryConvertValue<long>(0);
        /// <inheritdoc/>
        public int GetInteger() => Value.TryConvertValue(0);
        /// <inheritdoc/>
        public double GetDouble() => Value.TryConvertValue(0.0);
        /// <inheritdoc/>
        public FInt64 GetFloat() => Value.TryConvertValue<FInt64>(0.0);
        /// <inheritdoc/>
        public DateTime GetDateTime() => Value.TryConvertValue(DateTime.MinValue);
        /// <inheritdoc/>
        public bool GetBoolean() => Value.TryConvertValue(false);
        /// <inheritdoc/>
        public void SetString(string value) => Info = value;
        /// <inheritdoc/>
        public void SetInteger(int value) => Value = value.TryConvertToValue<T>();
        /// <inheritdoc/>
        public void SetInteger64(long value) => Value = value.TryConvertToValue<T>();
        /// <inheritdoc/>
        public void SetDouble(double value) => Value = value.TryConvertToValue<T>();
        /// <inheritdoc/>
        public void SetFloat(FInt64 value) => Value = value.TryConvertToValue<T>();
        /// <inheritdoc/>
        public void SetDateTime(DateTime value) => Value = value.TryConvertToValue<T>();
        /// <inheritdoc/>
        public void SetBoolean(bool value) => Value = value.TryConvertToValue<T>();

        #endregion
        /// <summary>
        /// 加载 通过lps文本创建一个子类
        /// </summary>
        /// <param name="lps">lps文本</param>
        public void Load(string lps)
        {
            if (lps.Contains('#'))
            {
                var spl = Sub.Split(lps, 1, separatorArray: "#");
                Name = spl[0];
                Info = spl[1];
            }
            else
            {
                Info = lps;
            }
        }
        /// <summary>
        /// 通过名字和信息创建新的Sub
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="info">信息 (正常版本)</param>
        public void Load(string name, string info)
        {
            Name = name;
            Info = info;
        }
        /// <summary>
        /// 通过Sub创建新的Sub
        /// </summary>
        /// <param name="sub">其他Sub</param>
        public void Set(ISub sub)
        {
            Name = sub.Name;
            if (sub is Sub_C<T> subc)
            {
                Value = subc.Value;
                LineAttribute = subc.LineAttribute;
            }
            else
            {
                Info = sub.Info;
            }
        }
        /// <inheritdoc/>
        public string GetInfo()
        {
            return Info;
        }
        /// <inheritdoc/>
        public string[] GetInfos()
        {
            if (Value is IEnumerable enumerable)
            {
                return enumerable.Cast<object>().Select(x => SetObject.ConvertToStoreString(x, LineAttribute)).ToArray();
            }
            else
            {
                return new string[] { Info };
            }
        }
        /// <inheritdoc/>
        public long GetLongHashCode() => Sub.GetHashCode(Name) + Value?.GetHashCode() ?? 0;

        /// <summary>
        /// 将当前Sub转换成文本格式 (info已经被转义/去除关键字)
        /// </summary>
        /// <returns>Sub的文本格式 (info已经被转义/去除关键字)</returns>
        public override string ToString()
        {
            string infostorestring = GetStoreString();
            if (infostorestring == "")
                return Name + ":|";
            return Name + '#' + infostorestring + ":|";
        }
    }
}
