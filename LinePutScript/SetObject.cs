using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable enable

namespace LinePutScript
{
    /// <summary>
    /// Load Object 可以储存任何类型的值 对性能进行优化
    /// </summary>
    public class SetObject : ISetObject, IEquatable<SetObject>, IEquatable<string>, IEquatable<long>, IEquatable<int>, IEquatable<double>, IEquatable<DateTime>, IEquatable<bool>
    {
        /// <summary>
        /// 储存Object的类型
        /// </summary>
        public enum ObjectType
        {
            /// <summary>
            /// 字符串
            /// </summary>
            String,
            /// <summary>
            /// 布尔值
            /// </summary>
            Boolean,
            /// <summary>
            /// 整数
            /// </summary>
            Integer,
            /// <summary>
            /// 整数64位
            /// </summary>
            Integer64,
            /// <summary>
            /// 浮点数
            /// </summary>
            Double,
            /// <summary>
            /// 浮点数(long)
            /// </summary>
            Float,
            /// <summary>
            /// 时间
            /// </summary>
            DateTime,
        }
        /// <summary>
        /// 类型
        /// </summary>
        public ObjectType Type;
        private dynamic[] objectvalue = new dynamic[7];
        /// <summary>
        /// 储存的数据
        /// </summary>
        public dynamic Value
        {
            get => objectvalue[(int)Type];
            set => objectvalue[(int)Type] = value;
        }
        #region 构造函数
        /// <summary>
        /// 新建 SetObject: string
        /// </summary>
        public SetObject()
        {
            Type = ObjectType.String;
            this.Value = "";
        }
        /// <summary>
        /// 新建 SetObject: string
        /// </summary>
        /// <param name="value">值</param>
        public SetObject(string value)
        {
            Type = ObjectType.String;
            this.Value = value;
        }

        /// <summary>
        /// 新建 SetObject: long
        /// </summary>
        /// <param name="value">值</param>
        public SetObject(long value)
        {
            Type = ObjectType.Integer64;
            this.Value = value;
        }

        /// <summary>
        /// 新建 SetObject: int
        /// </summary>
        /// <param name="value">值</param>
        public SetObject(int value)
        {
            Type = ObjectType.Integer;
            this.Value = value;
        }

        /// <summary>
        /// 新建 SetObject: double
        /// </summary>
        /// <param name="value">值</param>
        public SetObject(double value)
        {
            Type = ObjectType.Double;
            this.Value = value;
        }

        /// <summary>
        /// 新建 SetObject: String
        /// </summary>
        /// <param name="value">值</param>
        public SetObject(float value)
        {
            Type = ObjectType.Float;
            this.Value = (double)value;
        }

        /// <summary>
        /// 新建 SetObject: DateTime
        /// </summary>
        /// <param name="value">值</param>
        public SetObject(DateTime value)
        {
            Type = ObjectType.DateTime;
            this.Value = value;
        }
        /// <summary>
        /// 新建 SetObject: bool
        /// </summary>
        /// <param name="value">值</param>
        public SetObject(bool value)
        {
            Type = ObjectType.Boolean;
            this.Value = value;
        }
        #endregion

        #region 转换函数
        /// <summary>
        /// 转换成为储存String类型
        /// </summary>
        public string GetStoreString()
        {
            switch (Type)
            {
                case ObjectType.Integer:
                    return ((int)Value).ToString();
                case ObjectType.Integer64:
                    return ((long)Value).ToString();
                case ObjectType.DateTime:
                    return ((DateTime)Value).Ticks.ToString();
                case ObjectType.Double:
                    return ((double)Value).ToString();
                case ObjectType.Float:
                    return ((long)((double)Value * 1000000000)).ToString();
                case ObjectType.String:
                    return (string)Value;
                case ObjectType.Boolean:
                    return ((bool)Value).ToString();
                default:
                    return String.Empty;
            }
        }
        /// <summary>
        /// 转换成 String 类型
        /// </summary>
        public string GetString()
        {
            switch (Type)
            {
                case ObjectType.Integer:
                    return ((int)Value).ToString();
                case ObjectType.Integer64:
                    return ((long)Value).ToString();
                case ObjectType.Double:
                case ObjectType.Float:
                    return ((double)Value).ToString();
                case ObjectType.String:
                    return (string)Value;
                case ObjectType.Boolean:
                    return ((bool)Value).ToString();
                case ObjectType.DateTime:
                    return ((DateTime)Value).ToString("yyyy-MM-dd HH:mm:ss");
                default:
                    return String.Empty;
            }
        }
        /// <summary>
        /// 转换成 long 类型
        /// </summary>
        public long GetInteger64()
        {
            switch (Type)
            {
                case ObjectType.Integer:
                    return (long)Value;
                case ObjectType.Integer64:
                    return (long)Value;
                case ObjectType.DateTime:
                    return ((DateTime)Value).Ticks;
                case ObjectType.Double:
                case ObjectType.Float:
                    return (long)(double)Value;
                case ObjectType.String:
                    if (long.TryParse((string)Value, out long result))                    
                        return result;                    
                    else                    
                        return 0;                    
                case ObjectType.Boolean:
                    return (bool)Value ? 1 : 0;
                default:
                    return 0;
            }
        }
        /// <summary>
        /// 转换成 int 类型
        /// </summary>
        public int GetInteger()
        {
            switch (Type)
            {
                case ObjectType.Integer:
                    return (int)Value;
                case ObjectType.Integer64:
                    return (int)(long)Value;
                case ObjectType.DateTime:
                    return (int)((DateTime)Value).Ticks;
                case ObjectType.Double:
                case ObjectType.Float:
                    return (int)(double)Value;
                case ObjectType.String:
                    if (int.TryParse((string)Value, out var result))
                        return result;
                    else
                        return 0;
                case ObjectType.Boolean:
                    return (bool)Value ? 1 : 0;
                default:
                    return 0;
            }
        }
        /// <summary>
        /// 转换成 double 类型
        /// </summary>
        public double GetDouble()
        {
            switch (Type)
            {
                case ObjectType.Integer:
                    return (int)Value;
                case ObjectType.Integer64:
                    return (long)Value;
                case ObjectType.DateTime:
                    return ((DateTime)Value).Ticks;
                case ObjectType.Double:
                case ObjectType.Float:
                    return (double)Value;
                case ObjectType.String:
                    if (double.TryParse((string)Value, out var result))
                        return result;
                    else
                        return 0;
                case ObjectType.Boolean:
                    return (bool)Value ? 1 : 0;
                default:
                    return 0;
            }
        }
        /// <summary>
        /// 转换成 double(int64) 类型
        /// </summary>
        public double GetFloat()
        {
            switch (Type)
            {
                case ObjectType.Integer:
                    return (int)Value;
                case ObjectType.Integer64:
                    return (long)Value;
                case ObjectType.DateTime:
                    return ((DateTime)Value).Ticks;
                case ObjectType.Double:
                case ObjectType.Float:
                    return (double)Value;
                case ObjectType.String:
                    if (long.TryParse((string)Value, out long result))
                    {
                        return result / 1000000000.0;
                    }
                    else
                    {
                        return 0;
                    }
                case ObjectType.Boolean:
                    return (bool)Value ? 1 : 0;
                default:
                    return 0;
            }
        }
        /// <summary>
        /// 转换成 DateTime 类型
        /// </summary>
        public DateTime GetDateTime()
        {
            switch (Type)
            {
                case ObjectType.Integer:
                    return new DateTime((int)Value);
                case ObjectType.Integer64:
                    return new DateTime((long)Value);
                case ObjectType.Double:
                case ObjectType.Float:
                    return new DateTime((long)(double)Value);
                case ObjectType.String:
                    if (long.TryParse((string)Value, out long r))
                    {
                        return new DateTime(r);
                    }
                    else if (DateTime.TryParse((string)Value, out DateTime result))
                    {
                        return result;
                    }
                    else
                    {
                        return DateTime.MinValue;
                    }
                case ObjectType.Boolean:
                    return (bool)Value ? DateTime.MaxValue : DateTime.MinValue;
                case ObjectType.DateTime:
                    return (DateTime)Value;
                default:
                    return new DateTime(0);
            }
        }
        /// <summary>
        /// 转换成 bool 类型
        /// </summary>
        public bool GetBoolean()
        {
            switch (Type)
            {
                case ObjectType.Integer:
                case ObjectType.Integer64:
                    return (long)Value >= 1;
                case ObjectType.DateTime:
                    return ((DateTime)Value).Ticks >= 1;
                case ObjectType.Double:
                case ObjectType.Float:
                    return (double)Value >= 1;
                case ObjectType.String:
                    switch (((string)Value).ToLower())
                    {
                        case "t":
                        case "":
                        case "true":
                        case "1":
                            return true;
                        default:
                            return false;
                    }
                case ObjectType.Boolean:
                    return (bool)Value;
                default:
                    return false;
            }
        }
        /// <summary>
        /// 设置 string 值
        /// </summary>
        public void SetString(string value)
        {
            Type = ObjectType.String;
            this.Value = value;
        }
        /// <summary>
        /// 设置 int 值
        /// </summary>
        public void SetInteger(int value)
        {
            Type = ObjectType.Integer;
            this.Value = value;
        }
        /// <summary>
        /// 设置 long 值
        /// </summary>
        public void SetInteger64(long value)
        {
            Type = ObjectType.Integer64;
            this.Value = value;
        }
        /// <summary>
        /// 设置 double 值
        /// </summary>
        public void SetDouble(double value)
        {
            if (Type == ObjectType.Float)
            {
                this.Value = value;
            }
            Type = ObjectType.Double;
            this.Value = value;
        }
        /// <summary>
        /// 设置 float 值
        /// </summary>
        public void SetFloat(double value)
        {
            Type = ObjectType.Float;
            this.Value = value;
        }
        /// <summary>
        /// 设置 DateTime 值
        /// </summary>
        public void SetDateTime(DateTime value)
        {
            Type = ObjectType.DateTime;
            this.Value = value;
        }
        /// <summary>
        /// 设置 bool 值
        /// </summary>
        public void SetBoolean(bool value)
        {
            Type = ObjectType.Boolean;
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
        #endregion

        /// <summary>
        /// 比较两个 SetObject 对象差距
        /// </summary>
        public int CompareTo(ISetObject? other)
        {
            if (other == null)
                return int.MinValue;
            return ((IComparable)Value).CompareTo(other.Value);
            //if (typeof(IComparable).IsAssignableFrom(Value.GetType()))
            //{
            //    return ((IComparable)Value).CompareTo(other.Value);
            //}
            //else
            //{
            //    return int.MaxValue;
            //}
            //switch (Type)
            //{
            //    case ObjectType.Integer:
            //        return ((int)Value).CompareTo((int)other.Value);
            //    case ObjectType.Integer64:
            //    case ObjectType.DateTime:
            //        return ((long)Value).CompareTo((int)other.Value);
            //    case ObjectType.Double:
            //    case ObjectType.Float:
            //        return ((double)Value).CompareTo((double)other.Value);
            //    case ObjectType.String:
            //        return ((string)Value).CompareTo((string)other.Value);
            //    case ObjectType.Boolean:
            //        return ((bool)Value).CompareTo((bool)other.Value);
            //    default:
            //        return int.MaxValue;
            //}            
        }
        /// <summary>
        /// 比较两个 SetObject 对象差距
        /// </summary>
        public int CompareTo(object? obj)
        {
            if (obj == null)
                return int.MinValue;
            else if (obj.GetType().Equals(GetType()))
                return CompareTo((SetObject)obj);
            else if (typeof(IComparable).IsAssignableFrom(obj.GetType()))
                return ((IComparable)Value).CompareTo(obj);
            else return int.MaxValue;
        }
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="other">其他参数</param>
        /// <returns></returns>
        public override bool Equals(object? other)
        {
            switch (other?.GetType().Name)
            {
                case "String":
                    return GetString() == (string)other;
                case "Int32":
                    return GetInteger() == (int)other;
                case "Int64":
                    return GetInteger64() == (long)other;
                case "Double":
                    return GetDouble() == (double)other;
                case "Boolean":
                    return GetBoolean() == (bool)other;
                case "DateTime":
                    return GetDateTime() == (DateTime)other;
                default:
                    return false;
            }
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
            switch (Type)
            {
                case ObjectType.Integer:
                    return new SetObject((int)Value);
                case ObjectType.Integer64:
                    return new SetObject((long)Value);
                case ObjectType.DateTime:
                    return new SetObject((DateTime)Value);
                case ObjectType.Double:
                    return new SetObject((double)Value);
                case ObjectType.Float:
                    return new SetObject((float)Value);
                case ObjectType.String:
                    return new SetObject((string)Value);
                case ObjectType.Boolean:
                    return new SetObject((bool)Value);
                default:
                    return new SetObject("");
            }
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
