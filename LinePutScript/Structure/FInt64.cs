// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using fint64 = LinePutScript.FInt64;
#nullable enable
namespace LinePutScript
{
    /// <summary>
    /// fint64 类型 提供了在 long 范围内进行的固定精度数学运算
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct FInt64
        : IComparable,
          IConvertible,
          IComparable<fint64>,
          IEquatable<fint64>, IFormattable
    {
        /// <summary>
        /// 切换标头
        /// </summary>
        public static readonly long Anchor = 1000000000;
        /// <summary>
        /// 切换标头 double
        /// </summary>
        public static readonly double Anchord = 1000000000.0;
        /// <summary>
        /// 实际储存值
        /// </summary>
        public readonly long m_value;
        /// <summary>
        /// 从实际值long创建fint64
        /// </summary>
        public FInt64(long value)
        {
            m_value = value;
        }
        /// <summary>
        /// 从数值double创建fint64
        /// </summary>
        public FInt64(double value)
        {
            m_value = (long)(value * Anchord);
        }
        /// <summary>
        /// 从类创建fint64(自动转换)
        /// </summary>
        public static fint64 FromObject(object? value)
        {
            if (value is fint64 fint64Value)
            {
                return fint64Value;
            }
            else if (value is long longValue)
            {
                return new fint64(longValue);
            }
            else if (value is int intValue)
            {
                return new fint64(intValue * Anchor);
            }
            else if (value is double doubleValue)
            {
                return new fint64(doubleValue);
            }
            else if (value is string stringValue)
            {
                return Parse(stringValue);
            }
            else
            {
                return NaN;
            }
        }
        #region 运算符
        /// <summary>
        /// +运算符
        /// </summary>
        public static fint64 operator +(fint64 a, fint64 b)
        {
            return new fint64(a.m_value + b.m_value);
        }
        /// <summary>
        /// -运算符
        /// </summary>
        public static fint64 operator -(fint64 a, fint64 b)
        {
            return new fint64(a.m_value - b.m_value);
        }
        /// <summary>
        /// *运算符
        /// </summary>
        public static fint64 operator *(fint64 a, fint64 b)
        {
            return new fint64(a.m_value / 1000 * (b.m_value / 1000) / 1000);
        }
        /// <summary>
        /// /运算符
        /// </summary>
        public static fint64 operator /(fint64 a, fint64 b)
        {
            return new fint64(a.m_value * 100000 / b.m_value * 10000);
        }
        /// <summary>
        /// +运算符
        /// </summary>
        public static fint64 operator +(fint64 a, double b)
        {
            return new fint64(a.m_value + (long)(b * Anchord));
        }
        /// <summary>
        /// -运算符
        /// </summary>
        public static fint64 operator -(fint64 a, double b)
        {
            return new fint64(a.m_value - (long)(b * Anchord));
        }
        /// <summary>
        /// *运算符
        /// </summary>
        public static fint64 operator *(fint64 a, double b)
        {
            return new fint64((long)(a.m_value * b));
        }
        /// <summary>
        /// /运算符
        /// </summary>
        public static fint64 operator /(fint64 a, double b)
        {
            return new fint64((long)(a.m_value / b));
        }
        /// <summary>
        /// +运算符
        /// </summary>
        public static fint64 operator +(double a, fint64 b)
        {
            return new fint64((long)(a * Anchord) + b.m_value);
        }
        /// <summary>
        /// -运算符
        /// </summary>
        public static fint64 operator -(double a, fint64 b)
        {
            return new fint64((long)(a * Anchord) - b.m_value);
        }
        /// <summary>
        /// *运算符
        /// </summary>
        public static fint64 operator *(double a, fint64 b)
        {
            return new fint64((long)(a * b.m_value));
        }
        /// <summary>
        /// /运算符
        /// </summary>
        public static fint64 operator /(double a, fint64 b)
        {
            return new fint64(a / b.ToDouble());
        }
        #endregion
        /// <summary>
        /// 转换为数值double
        /// </summary>
        public double ToDouble()
        {
            if (IsNaN())
            {
                return double.NaN;
            }
            return m_value / Anchord;
        }
        /// <summary>
        /// 从数值double创建fint64
        /// </summary>
        public static fint64 FromNumberDouble(double value)
        {
            return new fint64(value);
        }
        /// <summary>
        /// 从数值long创建fint64
        /// </summary>
        public static fint64 FromNumberLong(long value)
        {
            return new fint64(value * Anchor);
        }
        /// <summary>
        /// 从数值int创建fint64
        /// </summary>
        public static fint64 FromNumberInt(int value)
        {
            return new fint64(value * Anchor);
        }
        /// <summary>
        /// 转换为纯文本(数值double)
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToDouble().ToString();
        }
        /// <summary>
        /// 转换成储存字符串
        /// </summary>
        public string ToStoreString()
        {
            return m_value.ToString();
        }

        /// <summary>
        /// 转换 double 为 FInt64
        /// </summary>
        public static implicit operator fint64(double v) => new fint64(v);
        /// <summary>
        /// 转换 FInt64 为 double
        /// </summary>
        public static implicit operator double(fint64 v) => v.ToDouble();

        #region 公共方法
        //
        // Public Constants
        //
        /// <summary>
        /// 最小值
        /// </summary>
        public static readonly fint64 MinValue = new fint64(long.MinValue);
        /// <summary>
        /// 最大值
        /// </summary>
        public static readonly fint64 MaxValue = new fint64(long.MaxValue);
        /// <summary>
        /// NaN
        /// </summary>
        public static readonly fint64 NaN = new fint64(long.MaxValue - 1);
        /// <summary>
        /// 正无穷大
        /// </summary>
        public static readonly fint64 NegativeInfinity = new fint64(long.MinValue + 1);
        /// <summary>
        /// 负无穷大
        /// </summary>
        public static readonly fint64 PositiveInfinity = new fint64(long.MaxValue);
        /// <summary>
        /// 判断是否是NaN
        /// </summary>
        public bool IsNaN()
        {
            return m_value == NaN.m_value;
        }
        /// <summary>
        /// 是否是负无穷大
        /// </summary>
        public bool IsNegativeInfinity()
        {
            return m_value == NegativeInfinity.m_value;
        }
        /// <summary>
        /// 是否是正无穷大
        /// </summary>
        public bool IsPositiveInfinity()
        {
            return m_value == PositiveInfinity.m_value;
        }

        /// <summary>Represents the additive identity (0).</summary>
        public static readonly fint64 AdditiveIdentity = new fint64(0);

        /// <summary>Represents the multiplicative identity (1).</summary>
        public static readonly fint64 MultiplicativeIdentity = new fint64(Anchor);

        /// <summary>Represents the number one (1).</summary>
        public static readonly fint64 One = new fint64(Anchor);

        /// <summary>Represents the number zero (0).</summary>
        public static readonly fint64 Zero = new fint64(0);

        /// <summary>Represents the number negative one (-1).</summary>
        public static readonly fint64 NegativeOne = -1.0;

        /// <summary>Represents the number negative zero (-0).</summary>
        public static readonly fint64 NegativeZero = -0.0;

        /// <summary>Represents the natural logarithmic base, specified by the constant, e.</summary>
        /// <remarks>Euler's number is approximately 2.7182818284590452354.</remarks>
        public static readonly fint64 E = Math.E;

        /// <summary>Represents the ratio of the circumference of a circle to its diameter, specified by the constant, PI.</summary>
        /// <remarks>Pi is approximately 3.1415926535897932385.</remarks>
        public static readonly fint64 Pi = Math.PI;

        /// <summary>Determines whether the specified value is finite (zero, subnormal, or normal).</summary>

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFinite(fint64 d)
        {
            return d.m_value != NaN.m_value && d.m_value != NegativeInfinity.m_value && d.m_value != PositiveInfinity.m_value;
        }

        /// <summary>Determines whether the specified value is infinite.</summary>

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInfinity(fint64 d)
        {
            return d == PositiveInfinity || d == NegativeInfinity;
        }

        /// <summary>Determines whether the specified value is NaN.</summary>

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNaN(fint64 d)
        {
            return d == NaN;
        }

        /// <summary>Determines whether the specified value is negative.</summary>

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNegative(fint64 d)
        {
            return d < Zero;
        }

        /// <summary>Determines whether the specified value is negative infinity.</summary>

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNegativeInfinity(fint64 d)
        {
            return d == NegativeInfinity;
        }

        /// <summary>Determines whether the specified value is normal.</summary>

        // This is probably not worth inlining, it has branches and should be rarely called
        public static bool IsNormal(fint64 d)
        {
            return d.m_value != 0 && IsFinite(d);
        }

        /// <summary>Determines whether the specified value is positive infinity.</summary>

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPositiveInfinity(fint64 d)
        {
            return d == PositiveInfinity;
        }
        #endregion

        // Compares this object to another object, returning an instance of System.Relation.
        // Null is considered less than any instance.
        //
        // If object is not of type FInt64, this method throws an ArgumentException.
        //
        // Returns a value less than zero if this  object
        //
        /// <summary>
        /// 对比fint64和object差异
        /// </summary>
        /// <param name="value">值</param>
        /// <exception cref="ArgumentException">如果不是可对比的数据,则报错</exception>
        public int CompareTo(object? value)
        {
            if (value == null)
            {
                return -1;
            }
            if (value is fint64 d)
            {
                return CompareTo(d);
            }
            if (value is long longvalue)
            {
                return CompareTo(FromNumberLong(longvalue));
            }
            if (value is int intvalue)
            {
                return CompareTo(FromNumberInt(intvalue));
            }
            if (value is double doubleValue)
            {
                return CompareTo(FromNumberDouble(doubleValue));
            }
            throw new ArgumentException("Object must be a FInt64 or number");
        }
        /// <summary>
        /// 对比fint64差异
        /// </summary>
        /// <param name="value">值</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(fint64 value)
        {
            // 对于 NaN，我们遵循 IEEE 754 标准，认为它小于任何数（包括负无穷大）
            if (IsNaN())
            {
                return -1;
            }
            // 如果 this 是正无穷大，那么它大于任何非 NaN 的数
            if (IsPositiveInfinity())
            {
                return value.IsNaN() ? 1 : -1;
            }
            // 如果 this 是负无穷大，那么它小于任何非 NaN 的数
            if (IsNegativeInfinity())
            {
                return value.IsNaN() ? -1 : 1;
            }
            return m_value.CompareTo(value.m_value);
        }
        /// <summary>
        /// 判断是否相等
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj is fint64 other)
            {
                return Equals(other);
            }
            else if (obj is long longvalue)
            {
                return m_value == longvalue * Anchor;
            }
            else if (obj is int intvalue)
            {
                return m_value == intvalue * Anchor;
            }
            else if (obj is double doubleValue)
            {
                return m_value == (long)(doubleValue * Anchord);
            }
            return false;
        }
        /// <summary>
        /// 判断是否相等
        /// </summary>
        public bool Equals(fint64 other)
        {
            if (IsNaN() || other.IsNaN())
            {
                return false; // NaN 不等于任何值，包括其自身
            }
            return m_value == other.m_value;
        }
        /// <summary>
        /// 获取HashCode
        /// </summary>
        public override int GetHashCode()
        {
            return m_value.GetHashCode();
        }
        /// <summary>
        /// ==运算符
        /// </summary>
        public static bool operator ==(fint64 a, fint64 b)
        {
            return a.m_value == b.m_value;
        }
        /// <summary>
        /// !=运算符
        /// </summary>
        public static bool operator !=(fint64 a, fint64 b)
        {
            return a.m_value != b.m_value;
        }
        /// <summary>
        /// 小于运算符
        /// </summary>
        public static bool operator <(fint64 a, fint64 b)
        {
            return a.m_value < b.m_value;
        }
        /// <summary>
        /// 大于运算符
        /// </summary>
        public static bool operator >(fint64 a, fint64 b)
        {
            return a.m_value > b.m_value;
        }
        /// <summary>
        /// 小于等于运算符
        /// </summary>
        public static bool operator <=(fint64 a, fint64 b)
        {
            return a.m_value <= b.m_value;
        }
        /// <summary>
        /// 大于等于运算符
        /// </summary>
        public static bool operator >=(fint64 a, fint64 b)
        {
            return a.m_value >= b.m_value;
        }

        /// <summary>
        /// ==运算符
        /// </summary>
        public static bool operator ==(fint64 a, double b)
        {
            return a.m_value == (long)(b * Anchord);
        }
        /// <summary>
        /// !=运算符
        /// </summary>
        public static bool operator !=(fint64 a, double b)
        {
            return a.m_value != (long)(b * Anchord);
        }
        /// <summary>
        /// 小于运算符
        /// </summary>
        public static bool operator <(fint64 a, double b)
        {
            return a.m_value < b * Anchord;
        }
        /// <summary>
        /// 大于运算符
        /// </summary>
        public static bool operator >(fint64 a, double b)
        {
            return a.m_value > b * Anchord;
        }
        /// <summary>
        /// 小于等于运算符
        /// </summary>
        public static bool operator <=(fint64 a, double b)
        {
            return a.m_value <= b * Anchord;
        }
        /// <summary>
        /// 大于等于运算符
        /// </summary>
        public static bool operator >=(fint64 a, double b)
        {
            return a.m_value >= b * Anchord;
        }

        /// <summary>
        /// ==运算符
        /// </summary>
        public static bool operator ==(double a, fint64 b)
        {
            return (long)(a * Anchord) == b.m_value;
        }
        /// <summary>
        /// !=运算符
        /// </summary>
        public static bool operator !=(double a, fint64 b)
        {
            return (long)(a * Anchord) != b.m_value;
        }
        /// <summary>
        /// 小于运算符
        /// </summary>
        public static bool operator <(double a, fint64 b)
        {
            return a * Anchord < b.m_value;
        }
        /// <summary>
        /// 大于运算符
        /// </summary>
        public static bool operator >(double a, fint64 b)
        {
            return a * Anchord > b.m_value;
        }
        /// <summary>
        /// 小于等于运算符
        /// </summary>
        public static bool operator <=(double a, fint64 b)
        {
            return a * Anchord <= b.m_value;
        }
        /// <summary>
        /// 大于等于运算符
        /// </summary>
        public static bool operator >=(double a, fint64 b)
        {
            return a * Anchord >= b.m_value;
        }
        /// <summary>
        /// 按格式进行转换(数值double)
        /// </summary>
        public string ToString(IFormatProvider? provider)
        {
            return ToDouble().ToString(provider);
        }
        /// <summary>
        /// 按格式进行转换(数值double)
        /// </summary>
        public string ToString(string? format, IFormatProvider? provider)
        {
            return ToDouble().ToString(format, provider);
        }
        /// <summary>
        /// 文本转换成fint64
        /// </summary>
        public static fint64 Parse(string s) => Parse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider: null);
        /// <summary>
        /// 文本转换成fint64
        /// </summary>
        public static fint64 Parse(string s, NumberStyles style) => Parse(s, style, provider: null);
        /// <summary>
        /// 文本转换成fint64
        /// </summary>
        public static fint64 Parse(string s, IFormatProvider? provider) => Parse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider);

        // Parses a fint64 from a String in the given style.  If
        // a NumberFormatInfo isn't specified, the current culture's
        // NumberFormatInfo is assumed.
        //
        // This method will not throw an OverflowException, but will return
        // PositiveInfinity or NegativeInfinity for a number that is too
        // large or too small.
        /// <summary>
        /// 文本转换成fint64
        /// </summary>
        public static fint64 Parse(string s, NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands, IFormatProvider? provider = null)
        {
            if (long.TryParse(s, style, provider, out long result))
            {
                return new fint64(result);
            }
            if (double.TryParse(s, style, provider, out double result2))
            {
                return new fint64(result2);
            }
            else
            {
                return NaN;
            }
        }
        /// <summary>
        /// 文本转换成fint64
        /// </summary>
        public static bool TryParse(string? s, out fint64 result) => TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider: null, out result);
        /// <summary>
        /// 文本转换成fint64
        /// </summary>
        public static bool TryParse(string? s, NumberStyles style, IFormatProvider? provider, out fint64 result)
        {
            if (s == null)
            {
                result = Zero;
                return false;
            }
            fint64 res = Parse(s, style, provider);
            if (res.IsNaN())
            {
                result = Zero;
                return false;
            }
            result = res;
            return true;
        }
        #region 转换成其他类型
        //
        // IConvertible implementation
        //
        /// <summary>
        /// 获取类型
        /// </summary>
        TypeCode IConvertible.GetTypeCode()
        {
            return TypeCode.Object;
        }
        /// <summary>
        /// 转换成bool
        /// </summary>
        public bool ToBoolean(IFormatProvider? provider = null)
        {
            return m_value > Anchor;
        }
        /// <summary>
        /// 转换成Char
        /// </summary>
        char IConvertible.ToChar(IFormatProvider? provider)
        {
            throw new InvalidCastException("Cannot convert FInt64 to Char.");
        }
        /// <summary>
        /// 转换成SByte(数值double)
        /// </summary>
        public sbyte ToSByte(IFormatProvider? provider)
        {
            return ((IConvertible)ToDouble()).ToSByte(provider);
        }
        /// <summary>
        /// 转换成Byte(数值double)
        /// </summary>
        public byte ToByte(IFormatProvider? provider)
        {
            return ((IConvertible)ToDouble()).ToByte(provider);
        }
        /// <summary>
        /// 转换成Int16(数值double)
        /// </summary>
        public short ToInt16(IFormatProvider? provider)
        {
            return ((IConvertible)ToDouble()).ToInt16(provider);
        }
        /// <summary>
        /// 转换成Uint16(数值double)
        /// </summary>
        public ushort ToUInt16(IFormatProvider? provider)
        {
            return ((IConvertible)ToDouble()).ToUInt16(provider);
        }
        /// <summary>
        /// 转换成Int32(数值double)
        /// </summary>
        public int ToInt32(IFormatProvider? provider)
        {
            return ((IConvertible)ToDouble()).ToInt32(provider);
        }
        /// <summary>
        /// 转换成Uint32(数值double)
        /// </summary>
        public uint ToUInt32(IFormatProvider? provider)
        {
            return ((IConvertible)ToDouble()).ToUInt32(provider);
        }
        /// <summary>
        /// 转换成Int64(数值double)
        /// </summary>
        public long ToInt64(IFormatProvider? provider)
        {
            return ((IConvertible)ToDouble()).ToInt64(provider);
        }
        /// <summary>
        /// 转换成Uint64(数值double)
        /// </summary>
        public ulong ToUInt64(IFormatProvider? provider)
        {
            return ((IConvertible)ToDouble()).ToUInt64(provider);
        }
        /// <summary>
        /// 转换成Single(数值double)
        /// </summary>
        public float ToSingle(IFormatProvider? provider)
        {
            return ((IConvertible)ToDouble()).ToSingle(provider);
        }
        /// <summary>
        /// 转换成Double(数值double)
        /// </summary>
        double IConvertible.ToDouble(IFormatProvider? provider)
        {
            return ToDouble();
        }
        /// <summary>
        /// 转换成Decimal(数值double)
        /// </summary>
        public decimal ToDecimal(IFormatProvider? provider)
        {
            return ((IConvertible)ToDouble()).ToDecimal(provider);
        }
        /// <summary>
        /// 转换成DateTime
        /// </summary>
        public DateTime ToDateTime(IFormatProvider? provider)
        {
            return new DateTime(m_value);
        }
        /// <summary>
        /// 转换成指定类型
        /// </summary>
        object IConvertible.ToType(Type type, IFormatProvider? provider)
        {
            return ((IConvertible)ToDouble()).ToDecimal(provider);
        }
        #endregion
        #region 运算符
        //
        // IBitwiseOperators
        //
        /// <summary>
        /// 运算符
        /// </summary>
        public static fint64 operator &(fint64 a, fint64 b)
        {
            return new fint64(a.m_value & b.m_value);
        }
        /// <summary>
        /// 运算符
        /// </summary>
        public static fint64 operator |(fint64 a, fint64 b)
        {
            return new fint64(a.m_value | b.m_value);
        }
        /// <summary>
        /// 运算符
        /// </summary>
        public static fint64 operator ^(fint64 a, fint64 b)
        {
            return new fint64(a.m_value ^ b.m_value);
        }
        /// <summary>
        /// 运算符
        /// </summary>
        public static fint64 operator ~(fint64 a)
        {
            return new fint64(~a.m_value);
        }
        /// <summary>
        /// 运算符
        /// </summary>
        public static fint64 operator --(fint64 a)
        {
            return new fint64(a.m_value - Anchor);
        }
        /// <summary>
        /// 运算符
        /// </summary>
        public static fint64 operator ++(fint64 a)
        {
            return new fint64(a.m_value + Anchor);
        }
        #endregion
    }
}
