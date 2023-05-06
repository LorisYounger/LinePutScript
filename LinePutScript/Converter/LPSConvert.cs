using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LinePutScript.Converter
{
    /// <summary>
    /// 序列化相关转换操作
    /// </summary>
    public static class LPSConvert
    {
        /// <summary>
        /// 转换方法: 继承该方法并实现Convert/ConvertBack
        /// </summary>
        public abstract class ConvertFunction
        {
            /// <summary>
            /// 转换方法: 继承该方法并实现Convert/ConvertBack
            /// </summary>
            public ConvertFunction() { }
            /// <summary>
            /// 指定转换方法
            /// </summary>
            /// <param name="value">要转换的值</param>
            /// <returns>String:Info</returns>
            public abstract string Convert(dynamic value);
            /// <summary>
            /// 指定反转方法
            /// </summary>
            /// <param name="info">储存的Info</param>
            /// <returns>要转换的值</returns>
            public abstract dynamic ConvertBack(string info);
            /// <summary>
            /// 通过Type获取转换方法
            /// </summary>
            /// <param name="type">ConvertFunction</param>
            /// <param name="value">要转换的值</param>
            /// <returns>String:Info</returns>
            public static string Convert(Type type, dynamic value)
            {
#pragma warning disable CS8600
                var mi = type.GetMethod("Convert");
#pragma warning disable CS8602
                object[] args = { value };
                return (string)mi.Invoke(Activator.CreateInstance(type), args);
#pragma warning restore CS8600
#pragma warning restore CS8602
            }
            /// <summary>
            /// 通过Type获取转换方法
            /// </summary>
            /// <param name="type">ConvertFunction</param>
            /// <param name="info">储存的Info</param>
            /// <returns>要转换的值</returns>
            public static dynamic ConvertBack(Type type, string info)
            {
#pragma warning disable CS8600
                var mi = type.GetMethod("ConvertBack");
#pragma warning disable CS8602
                object[] args = { info };
                return mi.Invoke(Activator.CreateInstance(type), args);
#pragma warning restore CS8600
#pragma warning restore CS8602
            }
        }
        /// <summary>
        /// 指定转换类型(非必须)
        /// </summary>
        public enum ConvertType
        {
            /// <summary>
            /// 默认: 自动判断
            /// </summary>
            Default,
            /// <summary>
            /// 字符串: ToString直接储存
            /// </summary>
            ToString,
            /// <summary>
            /// 浮点数(long): *1000000000 ConvertToInt64
            /// </summary>
            ToFloat,
            /// <summary>
            /// 时间
            /// </summary>
            ToDateTime,
            /// <summary>
            /// 枚举: ToString/Enum.Prase
            /// </summary>
            ToEnum,
            /// <summary>
            /// 列表: 根据 id:值 进行储存
            /// </summary>
            ToArray,
            /// <summary>
            /// 列表: 根据 id:值 进行储存
            /// </summary>
            ToList,
            /// <summary>
            /// 字典: 根据 字典 结构进行储存
            /// </summary>
            ToDictionary,
            /// <summary>
            /// 类: 将进行Converter递归
            /// </summary>
            Class,
            /// <summary>
            /// 自定转换函数
            /// </summary>
            Converter,
        }
        /// <summary>
        /// 将指定的对象序列化为TLine列表
        /// </summary>
        /// <typeparam name="TLine">Line</typeparam>
        /// <param name="value">要转换的对象</param>
        /// <param name="fourceToString">
        /// 强制转换内容为String (多用于当前类为Sub)
        /// false: 自动判断
        /// true: 强制转换String
        /// </param>
        /// <returns>TLine列表</returns>
        public static List<TLine> SerializeObjectToList<TLine>(object value, bool fourceToString = false) where TLine : ILine, new()
        {
            var type = value.GetType();
            var list = new List<TLine>();
            foreach (var mi in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                var att = mi.GetCustomAttributes(typeof(LineAttribute)).FirstOrDefault();
                if (att != null && att is LineAttribute la)
                {
                    list.Add(la.ConvertToLine<TLine>(mi.Name, mi.GetValue(value), fourceToString));
                }
            }
            foreach (var mi in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                var att = mi.GetCustomAttributes(typeof(LineAttribute)).FirstOrDefault();
                if (att != null && att is LineAttribute la)
                {
                    list.Add(la.ConvertToLine<TLine>(mi.Name, mi.GetValue(value), fourceToString));
                }
            }
            return list;
        }

        /// <summary>
        /// 将指定的对象序列化为TLPS
        /// </summary>
        /// <typeparam name="TLPS">LPS</typeparam>
        /// <typeparam name="TLine">Line</typeparam>
        /// <param name="value">要转换的对象</param>
        /// <param name="fourceToString">
        /// 强制转换内容为String (多用于当前类为Sub)
        /// false: 自动判断
        /// true: 强制转换String
        /// </param>
        /// <returns>LPS</returns>
        public static TLPS SerializeObjectToLPS<TLPS, TLine>(object value, bool fourceToString = false) where TLPS : ILPS, new() where TLine : ILine, new()
        {
            var list = SerializeObjectToList<TLine>(value, fourceToString: fourceToString);
            TLPS LPS = new TLPS();
            foreach (var item in list)
            {
                LPS.AddLine(item);
            }
            return LPS;
        }
        /// <summary>
        /// 将指定的对象序列化为TLine
        /// </summary>
        /// <typeparam name="TLine">Line</typeparam>
        /// <param name="value">要转换的对象</param>
        /// <param name="linename">行名字</param>
        /// <param name="fourceToString">
        /// 强制转换内容为String (多用于当前类为Sub)
        /// false: 自动判断
        /// true: 强制转换String
        /// </param>
        /// <returns>Line</returns>
        public static TLine SerializeObjectToLine<TLine>(object value, string linename, bool fourceToString = true) where TLine : ILine, new()
        {
            var list = SerializeObjectToList<TLine>(value, fourceToString: fourceToString);
            TLine Line = new TLine();
            Line.Name = linename;
            foreach (var item in list)
            {
                Line.Add(item);
            }
            return Line;
        }
        /// <summary>
        /// 将指定的对象序列化为LPS (使用默认参数)
        /// </summary>
        /// <param name="value">要转换的对象</param>
        /// <param name="fourceToString">
        /// 强制转换内容为String (多用于当前类为Sub)
        /// false: 自动判断
        /// true: 强制转换String
        /// </param>
        /// <returns>LPS</returns>
        public static LpsDocument SerializeObject(object value, bool fourceToString = false)
        {
            var list = SerializeObjectToList<Line>(value, fourceToString: fourceToString);
            LpsDocument LPS = new LpsDocument();
            foreach (var item in list)
            {
                LPS.AddLine(item);
            }
            return LPS;
        }
        /// <summary>
        /// 将指定的对象序列化为Line (使用默认参数)
        /// </summary>
        /// <param name="value">要转换的对象</param>
        /// <param name="linename">行名字</param>
        /// <param name="fourceToString">
        /// 强制转换内容为String (多用于当前类为Sub)
        /// false: 自动判断
        /// true: 强制转换String
        /// </param>
        /// <returns>Line</returns>
        public static Line SerializeObject(object value, string linename, bool fourceToString = true)
        {
            var list = SerializeObjectToList<Line>(value, fourceToString: fourceToString);
            Line Line = new Line();
            Line.Name = linename;
            foreach (var item in list)
            {
                Line.Add(item);
            }
            return Line;
        }

        /// <summary>
        /// 获取指定对象类型的转换类型
        /// </summary>
        /// <param name="valuetype">对象类型</param>
        /// <param name="att">附加参数,若有</param>
        /// <returns>转换类型</returns>
        public static ConvertType GetObjectConvertType(Type valuetype, LineAttribute? att = null)
        {
            ConvertType Type = att == null ? ConvertType.Default : att.Type;
            //自动判断
            if (Type == ConvertType.Default)
            {
                if (att?.Converter != null)
                    Type = ConvertType.Converter;
                else if (typeof(DateTime).IsAssignableFrom(valuetype))
                    Type = ConvertType.ToDateTime;
                else if (valuetype.IsArray)
                    Type = ConvertType.ToArray;
                else if (valuetype.IsEnum)
                    Type = ConvertType.ToEnum;
                else if (typeof(string).IsAssignableFrom(valuetype) || valuetype.IsValueType)
                    Type = ConvertType.ToString;
                else if (typeof(IDictionary).IsAssignableFrom(valuetype))
                    Type = ConvertType.ToDictionary;
                else if (typeof(IEnumerable).IsAssignableFrom(valuetype))
                    Type = ConvertType.ToList;
                else
                    Type = ConvertType.Class;
            }
            return Type;
        }
        /// <summary>
        /// 获取指定对象类型的是否可以转换为String
        /// </summary>
        /// <param name="type">转换方法</param>
        public static bool GetObjectIsString(ConvertType type)
        {
            switch (type)
            {
                case ConvertType.Class:
                case ConvertType.ToDictionary:
                    return false;
                default:
                    return true;
            }
        }
        /// <summary>
        /// 将指定的对象序列化为String
        /// </summary>
        /// <param name="value">需要序列化的object</param>
        /// <param name="type">转换方法,默认自动判断</param>
        /// <param name="att">附加参数,若有</param>
        /// <returns>退回序列化的String</returns>
        public static string GetObjectString(object value, ConvertType type = ConvertType.Default, LineAttribute? att = null)
        {
            //如果为null储存空
            if (value == null)
            {
                return "";
            }
            ConvertType Type = type == ConvertType.Default ? GetObjectConvertType(value.GetType(), att) : type;
            if (Type == ConvertType.Class)
            {
                if (att?.ILineType == null)
                    return SerializeObject(value, "deflinename").ToString();
                Type ex = typeof(LPSConvert);
#pragma warning disable CS8600
                MethodInfo mi = ex.GetMethod("SerializeObjectToLine");
#pragma warning disable CS8602
                MethodInfo miConstructed = mi.MakeGenericMethod(att.ILineType);
                object[] args = { value, "deflinename" };
                return ((ILine)miConstructed.Invoke(null, args)).ToString();
#pragma warning restore CS8600
#pragma warning restore CS8602
            }
            switch (Type)
            {
                case ConvertType.Converter:
                    if (att?.Converter != null)
                        return ConvertFunction.Convert(att.Converter, value);
                    else
                        return value.ToString() ?? "";
                case ConvertType.ToDateTime:
                    return ((DateTime)value).Ticks.ToString();
                case ConvertType.ToFloat:
                    return (((double)value) * 1000000000).ToString("f0");
                case ConvertType.ToArray:
                    StringBuilder sb = new StringBuilder();
                    foreach (object obj in (Array)value)
                    {
                        sb.Append(Sub.TextReplace(GetObjectString(obj)));
                        sb.Append(',');
                    }
                    return sb.ToString().TrimEnd(',');
                case ConvertType.ToList:
                    sb = new StringBuilder();
                    foreach (object obj in (IEnumerable)value)
                    {
                        sb.Append(Sub.TextReplace(GetObjectString(obj)));
                        sb.Append(',');
                    }
                    return sb.ToString().TrimEnd(',');
                case ConvertType.ToDictionary:
                    sb = new StringBuilder();
                    foreach (DictionaryEntry obj in (IDictionary)value)
                    {
                        sb.Append(Sub.TextReplace(GetObjectString(obj.Key)));
                        sb.Append('=');
                        sb.Append(obj.Value == null ? "" : Sub.TextReplace(GetObjectString(obj.Value)));
                        sb.Append(',');
                    }
                    return sb.ToString().TrimEnd(',');

                default:
                    return value.ToString() ?? "";
            }
        }
        /// <summary>
        /// 将指定的对象序列化为TLine
        /// </summary>
        /// <typeparam name="TLine"></typeparam>
        /// <param name="value">需要序列化的object</param>
        /// <param name="type">转换方法,默认自动判断</param>
        /// <param name="att">附加参数,若有</param>
        /// <param name="linename">行名字</param>
        /// <returns></returns>
        public static TLine GetObjectLine<TLine>(object value, string linename, ConvertType type = ConvertType.Default, LineAttribute? att = null) where TLine : ILine, new()
        {
            var name = att?.Name == null ? linename : att.Name;
            TLine t = new TLine();
            t.Name = name;
            //如果为null储存空
            if (value == null)
                return t;
            ConvertType Type = type == ConvertType.Default ? GetObjectConvertType(value.GetType(), att) : type;
            switch (Type)
            {
                case ConvertType.Class:
                    if (att?.ILineType == null)
                        return SerializeObjectToLine<TLine>(value, linename);
                    Type ex = typeof(LPSConvert);
#pragma warning disable CS8600
                    MethodInfo mi = ex.GetMethod("SerializeObjectToLine");
#pragma warning disable CS8602
                    MethodInfo miConstructed = mi.MakeGenericMethod(att.ILineType);
                    object[] args = { value, linename };
                    return (TLine)miConstructed.Invoke(null, args);
#pragma warning restore CS8600
#pragma warning restore CS8602
                case ConvertType.Converter:
                    if (att?.Converter != null)
                        t.Info = ConvertFunction.Convert(att.Converter, value);
                    else
                        t.Info = value.ToString() ?? "";
                    break;
                case ConvertType.ToDateTime:
                    t.Info = ((DateTime)value).Ticks.ToString();
                    break;
                case ConvertType.ToFloat:
                    t.Info = (((double)value) * 1000000000).ToString("f0");
                    break;
                case ConvertType.ToArray:
                    StringBuilder sb = new StringBuilder();
                    foreach (object obj in (Array)value)
                    {
                        sb.Append(Sub.TextReplace(GetObjectString(obj)));
                        sb.Append(',');
                    }
                    t.info = sb.ToString().TrimEnd(',');
                    break;
                case ConvertType.ToList:
                    sb = new StringBuilder();
                    foreach (object obj in (IEnumerable)value)
                    {
                        sb.Append(Sub.TextReplace(GetObjectString(obj)));
                        sb.Append(',');
                    }
                    t.info = sb.ToString().TrimEnd(',');
                    break;
                case ConvertType.ToDictionary:
                    foreach (DictionaryEntry obj in (IDictionary)value)
                    {
                        TLine newt = new TLine();
                        newt.Name = GetObjectString(obj.Key);
                        newt.info = obj.Value == null ? "" : Sub.TextReplace(GetObjectString(obj.Value));
                        t.Add(newt);
                    }
                    break;
                default:
                    t.Info = value.ToString() ?? "";
                    break;
            }
            return t;
        }

        //public static T DeserializeObject<T>(ILPS value)
        //{
        //    throw new TypeUnloadedException();
        //}
    }
}
