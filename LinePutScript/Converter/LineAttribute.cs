using System;
using static LinePutScript.Converter.LPSConvert;
#nullable enable
namespace LinePutScript.Converter
{
    /// <summary>
    /// 将改内容转换成Line
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class LineAttribute : Attribute
    {
        /// <summary>
        /// 将该内容转换成Line
        /// </summary>
        public LineAttribute() { }
        /// <summary>
        /// 将改内容转换成Line
        /// </summary>
        /// <param name="type">转换方法</param>
        /// <param name="converter">自定义转换方法(ConvertFunction)</param>
        /// <param name="name">指定名称</param>
        /// <param name="iLineType">如果为类,指定转换ILine的类型,默认为T</param>
        /// <param name="fourceToString">强制转换内容为String (多用于当前类为Sub)</param>
        /// <param name="ignoreCase">忽略名称的大小写</param>
        /// <param name="ignore">忽略该属性</param>
        public LineAttribute(ConvertType type = ConvertType.Default, Type? converter = null, string? name = null, Type? iLineType = null, bool fourceToString = false, bool ignoreCase = false, bool ignore = false)
        {
            Type = type;
            Converter = converter;
            Name = name;
            ILineType = iLineType;
            FourceToString = fourceToString;
            IgnoreCase = ignoreCase;
            Ignore = ignore;
        }

        /// <summary>
        /// 自定义转换方法
        /// </summary>
        public Type? Converter = null;
        /// <summary>
        /// 强制转换内容为String (多用于当前类为Sub)
        /// false: 自动判断
        /// true: 强制转换String
        /// </summary>
        public bool FourceToString { get; set; } = false;
        /// <summary>
        /// 转换方法
        /// </summary>
        public ConvertType Type { get; set; } = ConvertType.Default;

        /// <summary>
        /// 指定名称
        /// </summary>
        public string? Name { get; set; } = null;

        /// <summary>
        /// 如果为类,指定转换ILine的类型,默认为T
        /// </summary>
        public Type? ILineType { get; set; } = null;
        /// <summary>
        /// 将该文本转换成Line
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="fourceToString">
        /// 强制转换内容为String (多用于当前类为Sub)
        /// false: 自动判断
        /// true: 强制转换String
        /// </param>
        /// <returns>转换结果</returns>
        public T ConvertToLine<T>(string name, object? value, bool? fourceToString = null) where T : ILine, new()
        {
            if (!fourceToString.HasValue)
            {
                fourceToString = FourceToString;
            }
            string ln = Name ?? name;
            //如果为null储存空
            if (value == null)
            {
                T t = new T();
                t.Name = ln;
                return t;
            }
            //自动判断
            Type = Type == ConvertType.Default ? LPSConvert.GetObjectConvertType(value.GetType(), this) : Type;
            if (fourceToString == true || LPSConvert.GetObjectIsString(Type))
            {
                T t = new T();
                t.Name = ln;
                t.info = LPSConvert.GetObjectString(value, Type, this);
                return t;
            }
            else
            {
                return LPSConvert.GetObjectLine<T>(value, ln, Type, this);
            }
        }
        /// <summary>
        /// 忽略大小写
        /// </summary>
        public bool IgnoreCase { get; set; } = false;
        /// <summary>
        /// 忽略该属性
        /// </summary>
        public bool Ignore { get; set; } = false;
        /// <summary>
        /// 将两个LineAttribute合并, 以第二个为准
        /// </summary>
        /// <param name="a1">第一个</param>
        /// <param name="a2">第二个</param>
        /// <returns>合并结果</returns>
        public static LineAttribute Combine(LineAttribute a1, LineAttribute a2) => new LineAttribute(
            (a2.Type == ConvertType.Default ? a1.Type : a2.Type), (a2.Converter == null ? a1.Converter : a2.Converter),
            (a2.Name == null ? a1.Name : a2.Name), (a2.ILineType == null ? a1.ILineType : a2.ILineType),
            a2.FourceToString, a2.IgnoreCase, a2.Ignore);
    }
}
