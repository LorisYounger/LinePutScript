using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fint64 = LinePutScript.FInt64;

#nullable enable

namespace LinePutScript
{
    /// <summary>
    /// Set Object 可以储存任何类型的值 对性能进行优化
    /// </summary>
    public class SetObject : ISetObject, IEquatable<SetObject>, IEquatable<string>, IEquatable<long>, IEquatable<int>, IEquatable<double>, IEquatable<DateTime>, IEquatable<bool>
    {
        /// <summary>
        /// 数据转换器 原类型,目标类型,转换函数
        /// </summary>
        public static Dictionary<Type, Dictionary<Type, Func<object, object>>> ConverterSetObject = GetDefaultSetObjectConverts();

        /// <summary>
        /// 默认转换器包含 int,long,double,fint64(int64),string,bool,DateTime,TimeSpan
        /// </summary>
        public static Dictionary<Type, Dictionary<Type, Func<object, object>>> GetDefaultSetObjectConverts()
        {
            Dictionary<Type, Dictionary<Type, Func<object, object>>> result = new Dictionary<Type, Dictionary<Type, Func<object, object>>>();
            Dictionary<Type, Func<object, object>> intTo = new Dictionary<Type, Func<object, object>>
            {
                { typeof(int), (object v) => (int)v },
                { typeof(long), (object v) => (long)(int)v },
                { typeof(DateTime), (object v) => new DateTime((int)v) },
                { typeof(TimeSpan), (object v) => new TimeSpan((int)v) },
                { typeof(double), (object v) => (double)(int)v },
                { typeof(fint64), (object v) => (fint64)(int)v },
                { typeof(string), (object v) => ((int)v).ToString() },
                { typeof(bool), (object v) => (int)v >= 1 }
            };
            Dictionary<Type, Func<object, object>> longTo = new Dictionary<Type, Func<object, object>>
            {
                { typeof(int), (object v) => (int)(long)v },
                { typeof(long), (object v) => (long)v },
                { typeof(DateTime), (object v) => new DateTime((long)v) },
                 { typeof(TimeSpan), (object v) => new TimeSpan((long)v) },
                { typeof(double), (object v) => (double)(long)v },
                { typeof(fint64), (object v) => (fint64)(long)v },
                { typeof(string), (object v) => ((long)v).ToString() },
                { typeof(bool), (object v) => (long)v >= 1 }
            };
            Dictionary<Type, Func<object, object>> dbeTo = new Dictionary<Type, Func<object, object>>
            {
                { typeof(int), (object v) => (int)(double)v },
                { typeof(long), (object v) => (long)(double)v },
                { typeof(DateTime), (object v) => new DateTime((long)(double)v) },
                { typeof(TimeSpan), (object v) => new TimeSpan((long)(double)v) },
                { typeof(double), (object v) => (double)v },
                { typeof(fint64), (object v) => (fint64)(double)v },
                { typeof(string), (object v) => ((double)v).ToString() },
                { typeof(bool), (object v) => (double)v >= 1 }
            };
            Dictionary<Type, Func<object, object>> fltTo = new Dictionary<Type, Func<object, object>>
            {
                { typeof(int), (object v) => (int)(fint64)v },
                { typeof(long), (object v) => (long)(fint64)v },
                { typeof(DateTime), (object v) => new DateTime((long)(fint64)v) },
                { typeof(TimeSpan), (object v) => new TimeSpan((long)(fint64)v) },
                { typeof(double), (object v) => (double)((fint64)v) },
                { typeof(fint64), (object v) => (fint64)v },
                { typeof(string), (object v) => ((fint64)v).ToString() },
                { typeof(bool), (object v) => ((fint64)v).ToBoolean(null) }
            };
            Dictionary<Type, Func<object, object>> strTo = new Dictionary<Type, Func<object, object>>
            {
                { typeof(int), (object v) => {
                    if (int.TryParse((string)v, out int result))
                        return result;
                    else if (long.TryParse((string)v, out long resultl))
                        return (int)resultl;
                    else if (double.TryParse((string)v, out double resultdb))
                        return (int)resultdb;
                    else if (DateTime.TryParseExact((string)v,"yyyy-MM-dd HH:mm:ss",CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime resultd))
                    {
                        return (int)resultd.Ticks;
                    }
                    else if (DateTime.TryParse((string)v, out resultd))
                    {
                        return (int)resultd.Ticks;
                    }
                    else if(TimeSpan.TryParse((string)v, out var resultt))
                    {
                        return (int)resultt.Ticks;
                    }
                    else
                        return 0;
                } },
                { typeof(long), (object v) => {
                    if (long.TryParse((string)v, out long result))
                        return result;
                    else if (double.TryParse((string)v, out double resultdb))
                        return (long)resultdb;
                    else if (DateTime.TryParseExact((string)v,"yyyy-MM-dd HH:mm:ss",CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime resultd))
                    {
                        return resultd.Ticks;
                    }
                    else if (DateTime.TryParse((string)v, out resultd))
                    {
                        return resultd.Ticks;
                    }
                    else if(TimeSpan.TryParse((string)v, out var resultt))
                    {
                        return resultt.Ticks;
                    }
                    else
                        return 0;} },
                { typeof(DateTime), (object v) => {
                    if (long.TryParse((string)v, out long r))
                    {
                        return new DateTime(r);
                    }
                    else if (DateTime.TryParseExact((string)v,"yyyy-MM-dd HH:mm:ss",CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                    {
                        return result;
                    }
                    else if (DateTime.TryParse((string)v, out result))
                    {
                        return result;
                    }
                    else
                    {
                        return DateTime.MinValue;
                    }
                } },
                { typeof(TimeSpan), (object v) => {
                    if (long.TryParse((string)v, out long r))
                    {
                        return new TimeSpan(r);
                    }
                    else if (TimeSpan.TryParse((string)v, out var result))
                    {
                        return result;
                    }
                    else
                    {
                        return TimeSpan.MinValue;
                    }
                } },
                { typeof(double), (object v) => {
                    if (double.TryParse((string)v, out double result))
                        return result;
                    else
                        return 0;}},
                { typeof(fint64), (object v) => {return fint64.Parse((string)v);
                } },
                { typeof(string), (object v) => (string)v },
                { typeof(bool), (object v) => {
                    switch (((string)v).ToLower())
                    {
                        case "t":
                        case "true":
                        case "1":
                            return true;
                        default:
                            return false;
                    }} }
            };
            Dictionary<Type, Func<object, object>> boolTo = new Dictionary<Type, Func<object, object>>
            {
                { typeof(int), (object v) => (bool)v ? 1 : 0 },
                { typeof(long), (object v) => (bool)v ? 1 : 0 },
                { typeof(DateTime), (object v) => (bool)v ? DateTime.MaxValue : DateTime.MinValue },
                { typeof(TimeSpan), (object v) => (bool)v ? TimeSpan.MaxValue : TimeSpan.MinValue },
                { typeof(double), (object v) => (bool)v ? 1 : 0 },
                { typeof(fint64), (object v) => (bool)v ? 1 : 0 },
                { typeof(string), (object v) => ((bool)v).ToString() },
                { typeof(bool), (object v) => (bool)v }
            };
            Dictionary<Type, Func<object, object>> dtTo = new Dictionary<Type, Func<object, object>>
            {
                { typeof(int), (object v) => (int)((DateTime)v).Ticks },
                { typeof(long), (object v) => ((DateTime)v).Ticks },
                { typeof(DateTime), (object v) => (DateTime)v },
                { typeof(TimeSpan), (object v) => new DateTime(((TimeSpan)v).Ticks) },
                { typeof(double), (object v) => ((DateTime)v).Ticks },
                { typeof(fint64), (object v) => ((DateTime)v).Ticks },
                { typeof(string), (object v) => ((DateTime)v).ToString("yyyy-MM-dd HH:mm:ss") },
                { typeof(bool), (object v) => ((DateTime)v).Ticks >= 1 }
            };
            Dictionary<Type, Func<object, object>> tsTo = new Dictionary<Type, Func<object, object>>
            {
                { typeof(int), (object v) => (int)((TimeSpan)v).Ticks },
                { typeof(long), (object v) => ((TimeSpan)v).Ticks },
                { typeof(DateTime), (object v) => new DateTime(((TimeSpan)v).Ticks) },
                { typeof(TimeSpan), (object v) => (TimeSpan)v },
                { typeof(double), (object v) => ((TimeSpan)v).Ticks },
                { typeof(fint64), (object v) => ((TimeSpan)v).Ticks },
                { typeof(string), (object v) => ((TimeSpan)v).ToString() },
                { typeof(bool), (object v) => ((TimeSpan)v).Ticks >= 1 }
            };
            result.Add(typeof(int), intTo);
            result.Add(typeof(long), longTo);
            result.Add(typeof(double), dbeTo);
            result.Add(typeof(fint64), fltTo);
            result.Add(typeof(string), strTo);
            result.Add(typeof(bool), boolTo);
            result.Add(typeof(DateTime), dtTo);
            result.Add(typeof(TimeSpan), tsTo);
            return result;
        }
        /// <summary>
        /// 数据转换储存格式 原类型,转换函数
        /// </summary>
        public static Dictionary<Type, Func<object, string>> ConverterSetObjectToStoreString = GetDefaultSetObjectStoreStringConverts();
        /// <summary>
        /// 获取默认的储存转换器
        /// </summary>
        /// <returns></returns>
        public static Dictionary<Type, Func<object, string>> GetDefaultSetObjectStoreStringConverts()
        {
            Dictionary<Type, Func<object, string>> result = new Dictionary<Type, Func<object, string>>
            {
                { typeof(int), (object v) => ((int)v).ToString() },
                { typeof(long), (object v) => ((long)v).ToString() },
                { typeof(DateTime), (object v) => ((DateTime)v).Ticks.ToString() },
                { typeof(TimeSpan), (object v) => ((TimeSpan)v).Ticks.ToString() },
                { typeof(double), (object v) => ((double)v).ToString() },
                { typeof(fint64), (object v) => ((fint64)v).ToStoreString() },
                { typeof(string), (object v) => (string)v },
                { typeof(bool), (object v) => ((bool)v).ToString() }
            };
            return result;
        }
        /// <summary>
        /// 储存的数据
        /// </summary>
        public object Value { get; set; }
        dynamic ISetObject.Value { get => Value; set => Value = value; }
        #region 构造函数
        /// <summary>
        /// 新建 SetObject: string
        /// </summary>
        public SetObject()
        {
            Value = "";
        }
        /// <summary>
        /// 新建 SetObject: string
        /// </summary>
        public SetObject(object value)
        {
            Value = value;
        }
        /// <summary>
        /// 新建 SetObject: string
        /// </summary>
        /// <param name="value">值</param>
        public SetObject(string value)
        {
            this.Value = value;
        }

        /// <summary>
        /// 新建 SetObject: long
        /// </summary>
        /// <param name="value">值</param>
        public SetObject(long value)
        {
            this.Value = value;
        }

        /// <summary>
        /// 新建 SetObject: int
        /// </summary>
        /// <param name="value">值</param>
        public SetObject(int value)
        {
            this.Value = value;
        }

        /// <summary>
        /// 新建 SetObject: double
        /// </summary>
        /// <param name="value">值</param>
        public SetObject(double value)
        {
            this.Value = value;
        }

        /// <summary>
        /// 新建 SetObject: String
        /// </summary>
        /// <param name="value">值</param>
        public SetObject(fint64 value)
        {
            this.Value = value;
        }

        /// <summary>
        /// 新建 SetObject: DateTime
        /// </summary>
        /// <param name="value">值</param>
        public SetObject(DateTime value)
        {
            this.Value = value;
        }
        /// <summary>
        /// 新建 SetObject: bool
        /// </summary>
        /// <param name="value">值</param>
        public SetObject(bool value)
        {
            this.Value = value;
        }
        #endregion

        #region 转换函数
        /// <summary>
        /// 转换成指定类型
        /// </summary>
        /// <param name="type">指定类型呢</param>
        public dynamic? GetObjectByType(Type type)
        {
            if (type.IsAssignableFrom(Value.GetType()))
            {
                return Value;
            }
            if (ConverterSetObject.TryGetValue(Value.GetType(), out Dictionary<Type, Func<object, object>>? conv))
            {
                if (conv?.TryGetValue(type, out Func<object, object>? fun) == true)
                {
                    return fun(Value);
                }
            }
            return null;
        }
        /// <summary>
        /// 转换成为储存String类型
        /// </summary>
        public string GetStoreString()
        {
            if (ConverterSetObjectToStoreString.TryGetValue(Value.GetType(), out Func<object, string>? fun))
            {
                return fun?.Invoke(Value) ?? string.Empty;
            }
            return string.Empty;
        }
        /// <summary>
        /// 转换成 String 类型
        /// </summary>
        public string GetString() => GetObjectByType(typeof(string)) as string ?? string.Empty;
        /// <summary>
        /// 转换成 long 类型
        /// </summary>
        public long GetInteger64() => GetObjectByType(typeof(long)) as long? ?? 0;
        /// <summary>
        /// 转换成 int 类型
        /// </summary>
        public int GetInteger() => GetObjectByType(typeof(int)) as int? ?? 0;
        /// <summary>
        /// 转换成 double 类型
        /// </summary>
        public double GetDouble() => GetObjectByType(typeof(double)) as double? ?? 0;
        /// <summary>
        /// 转换成 double(int64) 类型
        /// </summary>
        public fint64 GetFloat() => GetObjectByType(typeof(fint64)) as fint64? ?? 0;
        /// <summary>
        /// 转换成 DateTime 类型
        /// </summary>
        public DateTime GetDateTime() => GetObjectByType(typeof(DateTime)) as DateTime? ?? DateTime.MinValue;
        /// <summary>
        /// 转换成 bool 类型
        /// </summary>
        public bool GetBoolean() => GetObjectByType(typeof(bool)) as bool? ?? false;
        /// <summary>
        /// 设置 string 值
        /// </summary>
        public void SetString(string value)
        {
            this.Value = value;
        }
        /// <summary>
        /// 设置 int 值
        /// </summary>
        public void SetInteger(int value)
        {
            this.Value = value;
        }
        /// <summary>
        /// 设置 long 值
        /// </summary>
        public void SetInteger64(long value)
        {
            this.Value = value;
        }
        /// <summary>
        /// 设置 double 值
        /// </summary>
        public void SetDouble(double value)
        {
            this.Value = value;
        }
        /// <summary>
        /// 设置 fint64 值
        /// </summary>
        public void SetFloat(fint64 value)
        {
            this.Value = value;
        }
        /// <summary>
        /// 设置 DateTime 值
        /// </summary>
        public void SetDateTime(DateTime value)
        {
            this.Value = value;
        }
        /// <summary>
        /// 设置 bool 值
        /// </summary>
        public void SetBoolean(bool value)
        {
            this.Value = value;
        }
        #endregion

        #region 隐式转换
        /// <summary>
        /// 转换 String 为 SetObject
        /// </summary>
        public static implicit operator SetObject(string v) => new SetObject(v);
        /// <summary>
        /// 转换 SetObject 为 String
        /// </summary>
        public static implicit operator string(SetObject v) => v.GetString();

        /// <summary>
        /// 转换 int 为 SetObject
        /// </summary>
        public static implicit operator SetObject(int v) => new SetObject(v);
        /// <summary>
        /// 转换 SetObject 为 int
        /// </summary>
        public static explicit operator int(SetObject v) => v.GetInteger();

        /// <summary>
        /// 转换 long 为 SetObject
        /// </summary>
        public static implicit operator SetObject(long v) => new SetObject(v);
        /// <summary>
        /// 转换 SetObject 为 long
        /// </summary>
        public static explicit operator long(SetObject v) => v.GetInteger64();

        /// <summary>
        /// 转换 double 为 SetObject
        /// </summary>
        public static implicit operator SetObject(double v) => new SetObject(v);
        /// <summary>
        /// 转换 SetObject 为 double
        /// </summary>
        public static explicit operator double(SetObject v) => v.GetDouble();

        /// <summary>
        /// 转换 bool 为 SetObject
        /// </summary>
        public static implicit operator SetObject(bool v) => new SetObject(v);
        /// <summary>
        /// 转换 SetObject 为 bool
        /// </summary>
        public static explicit operator bool(SetObject v) => v.GetBoolean();

        /// <summary>
        /// 转换 DateTime 为 SetObject
        /// </summary>
        public static implicit operator SetObject(DateTime v) => new SetObject(v);
        /// <summary>
        /// 转换 SetObject 为 DateTime
        /// </summary>
        public static explicit operator DateTime(SetObject v) => v.GetDateTime();

        /// <summary>
        /// 转换 fint64 为 SetObject
        /// </summary>
        public static implicit operator SetObject(fint64 v) => new SetObject(v);
        /// <summary>
        /// 转换 SetObject 为 fint64
        /// </summary>
        public static explicit operator fint64(SetObject v) => v.GetFloat();
        #endregion

        /// <summary>
        /// 比较两个 SetObject 对象差距
        /// </summary>
        public int CompareTo(ISetObject? other)
        {
            if (other == null)
                return -1;
            try
            {
                return ((IComparable)Value).CompareTo(other.Value);
            }
            catch
            {
                return ToString().CompareTo(other?.ToString());
            }      
        }
        /// <summary>
        /// 比较两个 SetObject 对象差距
        /// </summary>
        public int CompareTo(object? obj)
        {
            if (obj == null)
                return -1;
            else if (obj.GetType().Equals(Value.GetType()))
                return CompareTo((SetObject)obj);
            else if (typeof(IComparable).IsAssignableFrom(obj.GetType()))
                return ((IComparable)Value).CompareTo(obj);
            return ToString().CompareTo(obj?.ToString());
        }
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="other">其他参数</param>
        /// <returns></returns>
        public override bool Equals(object? other)
        {
            if (other == null)
            {
                return false;
            }
            if (other.GetType().IsAssignableFrom(Value.GetType()))
            {
                return Value.Equals(other);
            }
            if (other is ISetObject)
            {
                return Equals((ISetObject)other);
            }
            return false;
        }
        /// <summary>
        /// 比较两个 SetObject 对象是否相等
        /// </summary>
        public bool Equals(ISetObject? other)
        {
            return CompareTo(other) == 0;
        }
        /// <summary>
        /// 比较两个 SetObject 对象是否相等
        /// </summary>
        public bool Equals(SetObject? other)
        {
            return CompareTo(other) == 0;
        }
        /// <summary>
        /// 克隆新的 SetObject
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            if (Value is ICloneable)
            {
                return new SetObject(((ICloneable)Value).Clone());
            }
            return new SetObject(Value);
        }
        /// <summary>
        /// 比较 SetObject 和 string 是否相等
        /// </summary>
        public bool Equals(string? other)
        {
            return GetString().Equals(other);
        }
        /// <summary>
        /// 比较 SetObject 和 long 是否相等
        /// </summary>
        public bool Equals(long other)
        {
            return GetInteger64().Equals(other);
        }
        /// <summary>
        /// 比较 SetObject 和 int 是否相等
        /// </summary>
        public bool Equals(int other)
        {
            return GetInteger().Equals(other);
        }
        /// <summary>
        /// 比较 SetObject 和 double 是否相等
        /// </summary>
        public bool Equals(double other)
        {
            return GetDouble().Equals(other);
        }
        /// <summary>
        /// 比较 SetObject 和 DateTime 是否相等
        /// </summary>
        public bool Equals(DateTime other)
        {
            return GetDateTime().Equals(other);
        }
        /// <summary>
        /// 比较 SetObject 和 bool 是否相等
        /// </summary>
        public bool Equals(bool other)
        {
            return GetBoolean().Equals(other);
        }
        /// <summary>
        /// 获取 SetObject 的 HashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (int)Sub.GetHashCode(ToString());
        }
        /// <summary>
        /// 转换成文本形式
        /// </summary>
        public override string ToString()
        {
            return GetString();
        }
    }

}
