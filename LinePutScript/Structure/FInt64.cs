// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Buffers.Binary;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using fint64 = LinePutScript.Structure.FInt64;

namespace LinePutScript.Structure
{
    /// <summary>
    /// Represents a fint64-precision floating-point number.
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
        public static readonly double Anchord = 1000000000.0;

        public readonly long m_value;

        public FInt64(long value)
        {
            m_value = value;
        }
        public FInt64(double value)
        {
            m_value = (long)(value * Anchord);
        }
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

        public static fint64 operator +(fint64 a, fint64 b)
        {
            return new fint64(a.m_value + b.m_value);
        }

        public static fint64 operator -(fint64 a, fint64 b)
        {
            return new fint64(a.m_value - b.m_value);
        }

        public static fint64 operator *(fint64 a, fint64 b)
        {
            return new fint64(a.m_value / 1000 * (b.m_value / 1000) / 1000);
        }

        public static fint64 operator /(fint64 a, fint64 b)
        {
            return new fint64(a.m_value * 100000 / b.m_value * 10000);
        }

        public static fint64 operator +(fint64 a, double b)
        {
            return new fint64(a.m_value + (long)(b * Anchord));
        }

        public static fint64 operator -(fint64 a, double b)
        {
            return new fint64(a.m_value - (long)(b * Anchord));
        }

        public static fint64 operator *(fint64 a, double b)
        {
            return new fint64((long)(a.m_value * b));
        }

        public static fint64 operator /(fint64 a, double b)
        {
            return new fint64((long)(a.m_value / b));
        }

        public static fint64 operator +(double a, fint64 b)
        {
            return new fint64((long)(a * Anchord) + b.m_value);
        }

        public static fint64 operator -(double a, fint64 b)
        {
            return new fint64((long)(a * Anchord) - b.m_value);
        }

        public static fint64 operator *(double a, fint64 b)
        {
            return new fint64((long)(a * b.m_value));
        }

        public static fint64 operator /(double a, fint64 b)
        {
            return new fint64(a / b.ToDouble());
        }

        public double ToDouble()
        {
            if (IsNaN())
            {
                return double.NaN;
            }
            return m_value / Anchord;
        }

        public static fint64 FromNumberDouble(double value)
        {
            return new fint64(value);
        }
        public static fint64 FromNumberLong(long value)
        {
            return new fint64(value * Anchor);
        }
        public static fint64 FromNumberInt(int value)
        {
            return new fint64(value * Anchor);
        }

        public override string ToString()
        {
            return ToDouble().ToString();
        }

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
        public static explicit operator double(fint64 v) => v.ToDouble();

        //
        // Public Constants
        //
        public static readonly fint64 MinValue = new fint64(long.MinValue);
        public static readonly fint64 MaxValue = new fint64(long.MaxValue);
        public static readonly fint64 NaN = new fint64(long.MaxValue - 1);
        public static readonly fint64 NegativeInfinity = new fint64(long.MinValue + 1);
        public static readonly fint64 PositiveInfinity = new fint64(long.MaxValue);

        public bool IsNaN()
        {
            return m_value == NaN.m_value;
        }

        public bool IsNegativeInfinity()
        {
            return m_value == NegativeInfinity.m_value;
        }

        public bool IsPositiveInfinity()
        {
            return m_value == PositiveInfinity.m_value;
        }

        /// <summary>Represents the additive identity (0).</summary>
        internal static readonly fint64 AdditiveIdentity = new fint64(0);

        /// <summary>Represents the multiplicative identity (1).</summary>
        internal static readonly fint64 MultiplicativeIdentity = new fint64(Anchor);

        /// <summary>Represents the number one (1).</summary>
        internal static readonly fint64 One = new fint64(Anchor);

        /// <summary>Represents the number zero (0).</summary>
        internal static readonly fint64 Zero = new fint64(0);

        /// <summary>Represents the number negative one (-1).</summary>
        internal static readonly fint64 NegativeOne = -1.0;

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

        // Compares this object to another object, returning an instance of System.Relation.
        // Null is considered less than any instance.
        //
        // If object is not of type FInt64, this method throws an ArgumentException.
        //
        // Returns a value less than zero if this  object
        //
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

        // True if obj is another FInt64 with the same value as the current instance.  This is
        // a method of object equality, that only returns true if obj is also a fint64.
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is fint64 other)
            {
                if (IsNaN() || other.IsNaN())
                {
                    return false; // NaN 不等于任何值，包括其自身
                }
                return m_value == other.m_value;
            }
            return false;
        }
        public bool Equals(fint64 other)
        {
            if (IsNaN() || other.IsNaN())
            {
                return false; // NaN 不等于任何值，包括其自身
            }
            return m_value == other.m_value;
        }
        public override int GetHashCode()
        {
            return m_value.GetHashCode();
        }

        public static bool operator ==(fint64 a, fint64 b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(fint64 a, fint64 b)
        {
            return !a.Equals(b);
        }

        public static bool operator <(fint64 a, fint64 b)
        {
            return a.CompareTo(b) < 0;
        }

        public static bool operator >(fint64 a, fint64 b)
        {
            return a.CompareTo(b) > 0;
        }

        public static bool operator <=(fint64 a, fint64 b)
        {
            return a.CompareTo(b) <= 0;
        }

        public static bool operator >=(fint64 a, fint64 b)
        {
            return a.CompareTo(b) >= 0;
        }



        public string ToString(IFormatProvider? provider)
        {
            return ToDouble().ToString(provider);
        }

        public string ToString(string? format, IFormatProvider? provider)
        {
            return ToDouble().ToString(format, provider);
        }

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
        {
            return ToDouble().TryFormat(destination, out charsWritten, format, provider);
        }

        public static fint64 Parse(string s) => Parse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider: null);

        public static fint64 Parse(string s, NumberStyles style) => Parse(s, style, provider: null);

        public static fint64 Parse(string s, IFormatProvider? provider) => Parse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider);

        public static fint64 Parse(string s, NumberStyles style, IFormatProvider? provider)
        {
            return Parse(s.AsSpan(), style, provider);
        }

        // Parses a fint64 from a String in the given style.  If
        // a NumberFormatInfo isn't specified, the current culture's
        // NumberFormatInfo is assumed.
        //
        // This method will not throw an OverflowException, but will return
        // PositiveInfinity or NegativeInfinity for a number that is too
        // large or too small.

        public static fint64 Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands, IFormatProvider? provider = null)
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

        public static bool TryParse([NotNullWhen(true)] string? s, out fint64 result) => TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider: null, out result);

        public static bool TryParse(ReadOnlySpan<char> s, out fint64 result) => TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider: null, out result);

        public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out fint64 result)
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

        public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out fint64 result)
        {
            return TryParse(s, style, provider, out result);
        }

        //
        // IConvertible implementation
        //

        public TypeCode GetTypeCode()
        {
            return TypeCode.Object;
        }

        bool IConvertible.ToBoolean(IFormatProvider? provider)
        {
            return m_value > Anchor;
        }

        char IConvertible.ToChar(IFormatProvider? provider)
        {
            throw new InvalidCastException("Cannot convert FInt64 to Char.");
        }

        sbyte IConvertible.ToSByte(IFormatProvider? provider)
        {
            return ((IConvertible)ToDouble()).ToSByte(provider);
        }

        byte IConvertible.ToByte(IFormatProvider? provider)
        {
            return ((IConvertible)ToDouble()).ToByte(provider);
        }

        short IConvertible.ToInt16(IFormatProvider? provider)
        {
            return ((IConvertible)ToDouble()).ToInt16(provider);
        }

        ushort IConvertible.ToUInt16(IFormatProvider? provider)
        {
            return ((IConvertible)ToDouble()).ToUInt16(provider);
        }

        int IConvertible.ToInt32(IFormatProvider? provider)
        {
            return ((IConvertible)ToDouble()).ToInt32(provider);
        }

        uint IConvertible.ToUInt32(IFormatProvider? provider)
        {
            return ((IConvertible)ToDouble()).ToUInt32(provider);
        }

        long IConvertible.ToInt64(IFormatProvider? provider)
        {
            return ((IConvertible)ToDouble()).ToInt64(provider);
        }

        ulong IConvertible.ToUInt64(IFormatProvider? provider)
        {
            return ((IConvertible)ToDouble()).ToUInt64(provider);
        }

        float IConvertible.ToSingle(IFormatProvider? provider)
        {
            return ((IConvertible)ToDouble()).ToSingle(provider);
        }

        double IConvertible.ToDouble(IFormatProvider? provider)
        {
            return ToDouble();
        }

        decimal IConvertible.ToDecimal(IFormatProvider? provider)
        {
            return ((IConvertible)ToDouble()).ToDecimal(provider);
        }

        DateTime IConvertible.ToDateTime(IFormatProvider? provider)
        {
            return ((IConvertible)m_value).ToDateTime(provider);
        }

        object IConvertible.ToType(Type type, IFormatProvider? provider)
        {
            return ((IConvertible)ToDouble()).ToDecimal(provider);
        }

        public static fint64 Log2(fint64 value) => Math.Log2((double)value);

        //
        // IBitwiseOperators
        //

        public static fint64 operator &(fint64 a, fint64 b)
        {
            return new fint64(a.m_value & b.m_value);
        }

        public static fint64 operator |(fint64 a, fint64 b)
        {
            return new fint64(a.m_value | b.m_value);
        }

        public static fint64 operator ^(fint64 a, fint64 b)
        {
            return new fint64(a.m_value ^ b.m_value);
        }

        public static fint64 operator ~(fint64 a)
        {
            return new fint64(~a.m_value);
        }
        public static fint64 operator --(fint64 a)
        {
            return new fint64(a.m_value - Anchor);
        }
        public static fint64 operator ++(fint64 a)
        {
            return new fint64(a.m_value + Anchor);
        }

    }
}
