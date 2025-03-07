﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
#nullable enable
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
            public static string Convert(Type type, dynamic? value)
            {
                MethodInfo? mi = type.GetMethod("Convert");
                object?[] args = { value };
                return (string)(mi?.Invoke(Activator.CreateInstance(type), args) ?? "");
            }
            /// <summary>
            /// 通过Type获取转换方法
            /// </summary>
            /// <param name="type">ConvertFunction</param>
            /// <param name="info">储存的Info</param>
            /// <returns>要转换的值</returns>
            public static dynamic? ConvertBack(Type type, string info)
            {
                MethodInfo? mi = type.GetMethod("ConvertBack");
                object[] args = { info };
                return mi?.Invoke(Activator.CreateInstance(type), args);
            }
            /// <summary>
            /// LPS储存转换器
            /// </summary>
            public class CF_LPS<T> : ConvertFunction where T : ILPS, new()
            {
                /// <summary>
                /// 指定转换方法
                /// </summary>
                /// <param name="value">要转换的值</param>
                /// <returns>String:Info</returns>
                public override string Convert(dynamic value)
                {
                    return Sub.TextReplace(((T)value).ToString() ?? "");
                }
                /// <summary>
                /// 指定反转方法
                /// </summary>
                /// <param name="info">储存的Info</param>
                /// <returns>要转换的值</returns>
                public override dynamic ConvertBack(string info)
                {
                    T t = new T();
                    t.Load(info);
                    return t;
                }
            }
            /// <summary>
            /// Line储存转换器
            /// </summary>
            public class CF_Line<T> : ConvertFunction where T : ILine, new()
            {
                /// <summary>
                /// 指定转换方法
                /// </summary>
                /// <param name="value">要转换的值</param>
                /// <returns>String:Info</returns>
                public override string Convert(dynamic value)
                {
                    return Sub.TextReplace(((T)value).ToString() ?? "");
                }
                /// <summary>
                /// 指定反转方法
                /// </summary>
                /// <param name="info">储存的Info</param>
                /// <returns>要转换的值</returns>
                public override dynamic ConvertBack(string info)
                {
                    T t = new T();
                    t.Load(info);
                    return t;
                }
            }
            /// <summary>
            /// Sub储存转换器
            /// </summary>
            public class CF_Sub<T> : ConvertFunction where T : ISub, new()
            {
                /// <summary>
                /// 指定转换方法
                /// </summary>
                /// <param name="value">要转换的值</param>
                /// <returns>String:Info</returns>
                public override string Convert(dynamic value)
                {
                    return Sub.TextReplace(((T)value).ToString() ?? "");
                }
                /// <summary>
                /// 指定反转方法
                /// </summary>
                /// <param name="info">储存的Info</param>
                /// <returns>要转换的值</returns>
                public override dynamic ConvertBack(string info)
                {
                    T t = new T();
                    t.Load(info);
                    return t;
                }
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
            /// 浮点数(long): *Anchor ConvertToInt64
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
        /// <param name="convertNoneLineAttribute">是否转换不带LineAttribute的类</param>
        public static List<TLine> SerializeObjectToList<TLine>(object value, bool? fourceToString = null, bool convertNoneLineAttribute = false) where TLine : ILine, new()
        {
            //如果为null储存空           
            Type type = value.GetType();

            //自动判断
            var Type = LPSConvert.GetObjectConvertType(value.GetType());
            List<TLine> list = new List<TLine>();
            switch (Type)
            {
                case ConvertType.Class:
                    foreach (PropertyInfo mi in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(p => p.CanRead))
                    {
                        LineAttribute? att = mi.GetCustomAttributes<LineAttribute>().Combine();
                        if (att != null)
                        {
                            if (att.Ignore) continue;
                            list.Add(att.ConvertToLine<TLine>(mi.Name, mi.GetValue(value), fourceToString));
                        }
                        else if (convertNoneLineAttribute)
                        {
                            list.Add(LineAttribute.ConvertToLine<TLine>(mi.Name, mi.GetValue(value), fourceToString, convertNoneLineAttribute));
                        }
                    }
                    foreach (FieldInfo mi in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(f => !f.Name.StartsWith("<")))
                    {
                        LineAttribute? att = mi.GetCustomAttributes<LineAttribute>().Combine();
                        if (att != null)
                        {
                            if (att.Ignore) continue;
                            list.Add(att.ConvertToLine<TLine>(mi.Name, mi.GetValue(value), fourceToString));
                        }
                        else if (convertNoneLineAttribute)
                        {
                            list.Add(LineAttribute.ConvertToLine<TLine>(mi.Name, mi.GetValue(value), fourceToString, convertNoneLineAttribute));
                        }
                    }
                    return list;
                case ConvertType.ToList:
                    foreach (object? obj in (IEnumerable)value)
                    {
                        list.Add(GetObjectLine<TLine>(obj, "item", convertNoneLineAttribute: convertNoneLineAttribute));
                    }
                    return list;
                case ConvertType.ToArray:
                    for (int i = 0; i < ((Array)value).Length; i++)
                    {
                        list.Add(GetObjectLine<TLine>(((Array)value).GetValue(i), i.ToString(), convertNoneLineAttribute: convertNoneLineAttribute));
                    }
                    return list;
                case ConvertType.ToDictionary:
                    foreach (DictionaryEntry? obj in (IDictionary)value)
                    {
                        if (obj == null) continue;
                        list.Add(GetObjectLine<TLine>(obj.Value.Value, GetObjectString(obj.Value.Key, convertNoneLineAttribute: convertNoneLineAttribute), convertNoneLineAttribute: convertNoneLineAttribute));
                    }
                    return list;
                default:
                    throw new Exception("SerializeObjectToList only support [Class,ToList,ToDictionary]");
            }

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
        /// <param name="convertNoneLineAttribute">是否转换不带LineAttribute的类</param>
        public static TLPS SerializeObjectToLPS<TLPS, TLine>(object value, bool fourceToString = false, bool convertNoneLineAttribute = false) where TLPS : ILPS, new() where TLine : ILine, new()
        {
            List<TLine> list = SerializeObjectToList<TLine>(value, fourceToString: fourceToString, convertNoneLineAttribute);
            TLPS LPS = new TLPS();
            foreach (TLine item in list)
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
        /// <param name="convertNoneLineAttribute">是否转换不带LineAttribute的类</param>
        public static TLine SerializeObjectToLine<TLine>(object value, string linename, bool fourceToString = true, bool convertNoneLineAttribute = false) where TLine : ILine, new()
        {
            List<TLine> list = SerializeObjectToList<TLine>(value, fourceToString: fourceToString, convertNoneLineAttribute);
            TLine Line = new TLine();
            Line.Name = linename;
            foreach (TLine item in list)
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
        /// <param name="convertNoneLineAttribute">是否转换不带LineAttribute的类</param>
        public static LpsDocument SerializeObject(object value, bool fourceToString = false, bool convertNoneLineAttribute = false)
        {
            List<Line> list = SerializeObjectToList<Line>(value, fourceToString: fourceToString, convertNoneLineAttribute);
            LpsDocument LPS = new LpsDocument();
            foreach (Line item in list)
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
        /// <param name="convertNoneLineAttribute">是否转换不带LineAttribute的类</param>
        public static Line SerializeObject(object value, string linename, bool fourceToString = true, bool convertNoneLineAttribute = false)
        {
            List<Line> list = SerializeObjectToList<Line>(value, fourceToString: fourceToString, convertNoneLineAttribute);
            Line Line = new Line();
            Line.Name = linename;
            foreach (Line item in list)
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
                else if (valuetype.IsValueType && !valuetype.IsPrimitive)
                    Type = ConvertType.Class;
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
        /// <param name="convertNoneLineAttribute">是否转换不带LineAttribute的类</param>
        /// <returns>退回序列化的String</returns>
        public static string GetObjectString(object? value, ConvertType type = ConvertType.Default, LineAttribute? att = null, bool convertNoneLineAttribute = false)
        {
            //如果为null储存空
            if (value == null)
            {
                return "/null";
            }
            else if (att?.Ignore == true)
            {
                return "";
            }
            ConvertType Type = type == ConvertType.Default ? GetObjectConvertType(value.GetType(), att) : type;
            if (Type == ConvertType.Class)
            {
                convertNoneLineAttribute = att?.ConvertNoneLineAttribute ?? convertNoneLineAttribute;
                if (att?.ILineType == null)
                    return Sub.TextReplace(SerializeObject(value, att?.Name ?? "deflinename", convertNoneLineAttribute: convertNoneLineAttribute).ToString());
                Type ex = typeof(LPSConvert);
#pragma warning disable CS8600
                MethodInfo mi = ex.GetMethod("SerializeObjectToLine");
#pragma warning disable CS8602
                MethodInfo miConstructed = mi.MakeGenericMethod(att.ILineType);
                object[] args = { value, att?.Name ?? "deflinename", true, convertNoneLineAttribute };
                return Sub.TextReplace(((ILine)miConstructed.Invoke(null, args)).ToString());

            }
            switch (Type)
            {
                case ConvertType.Converter:
                    if (att?.Converter != null)
                        return ConvertFunction.Convert(att.Converter, value);
                    else
                        return value.ToString() == null ? "" : Sub.TextReplace(value.ToString());
                case ConvertType.ToDateTime:
                    return ((DateTime)value).Ticks.ToString();
                case ConvertType.ToFloat:
                    return FInt64.FromObject(value).ToStoreString();
                case ConvertType.ToArray:
                    StringBuilder sb = new StringBuilder();
                    foreach (object obj in (Array)value)
                    {
                        sb.Append(Sub.TextReplace(GetObjectString(obj, convertNoneLineAttribute: convertNoneLineAttribute)));
                        sb.Append(',');
                    }
                    return sb.ToString().TrimEnd(',');
                case ConvertType.ToList:
                    sb = new StringBuilder();
                    foreach (object obj in (IEnumerable)value)
                    {
                        sb.Append(Sub.TextReplace(GetObjectString(obj, convertNoneLineAttribute: convertNoneLineAttribute)));
                        sb.Append(',');
                    }
                    return sb.ToString().TrimEnd(',');
#pragma warning restore CS8600
#pragma warning restore CS8602
                case ConvertType.ToDictionary:
                    sb = new StringBuilder();
#pragma warning disable CS8605 // 取消装箱可能为 null 的值。
                    foreach (DictionaryEntry obj in (IDictionary)value)
                    {
                        sb.Append(Sub.TextReplace(GetObjectString(obj.Key, convertNoneLineAttribute: convertNoneLineAttribute)));
                        sb.Append('=');
                        sb.Append(obj.Value == null ? "" : Sub.TextReplace(GetObjectString(obj.Value, convertNoneLineAttribute: convertNoneLineAttribute)));
                        sb.Append("/n");
                    }
#pragma warning restore CS8605 // 取消装箱可能为 null 的值。
                    return sb.ToString().TrimEnd('/', 'n');

                default:
                    return value.ToString() == null ? "" : Sub.TextReplace(value.ToString());
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
        /// <param name="convertNoneLineAttribute">是否转换不带LineAttribute的类</param>
        public static TLine GetObjectLine<TLine>(object? value, string linename, ConvertType type = ConvertType.Default, LineAttribute? att = null, bool convertNoneLineAttribute = false) where TLine : ILine, new()
        {
            string name = att?.Name == null ? linename : att.Name;
            TLine t = new TLine();
            t.Name = name;
            //如果为null储存空
            if (value == null)
            {
                t.info = "/null";
                return t;
            }
            else if (att?.Ignore == true)
            {
                return t;
            }

            ConvertType Type = type == ConvertType.Default ? GetObjectConvertType(value.GetType(), att) : type;
            switch (Type)
            {
                case ConvertType.Class:
                    convertNoneLineAttribute = att?.ConvertNoneLineAttribute ?? convertNoneLineAttribute;
                    if (att?.ILineType == null)
                        return SerializeObjectToLine<TLine>(value, linename, convertNoneLineAttribute: convertNoneLineAttribute);
                    Type ex = typeof(LPSConvert);
#pragma warning disable CS8600
                    MethodInfo mi = ex.GetMethod("SerializeObjectToLine");
#pragma warning disable CS8602
                    MethodInfo miConstructed = mi.MakeGenericMethod(att.ILineType);
                    object[] args = { value, linename, true, convertNoneLineAttribute };
#pragma warning disable CS8603 // 可能返回 null 引用。
                    return (TLine)miConstructed.Invoke(null, args);

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
                    t.Info = FInt64.FromObject(value).ToStoreString();
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
#pragma warning restore CS8603 // 可能返回 null 引用。
#pragma warning restore CS8600
#pragma warning restore CS8602
                case ConvertType.ToDictionary:
#pragma warning disable CS8605 // 取消装箱可能为 null 的值。
                    foreach (DictionaryEntry obj in (IDictionary)value)
                    {
                        TLine newt = new TLine();
                        newt.Name = GetObjectString(obj.Key);
                        newt.info = obj.Value == null ? "" : Sub.TextReplace(GetObjectString(obj.Value));
                        t.Add(newt);
                    }
#pragma warning restore CS8605 // 取消装箱可能为 null 的值。
                    break;
                default:
                    t.Info = value.ToString() ?? "";
                    break;
            }
            return t;
        }

#pragma warning disable CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。
#pragma warning disable CS8602 // 解引用可能出现空引用。
        /// <summary>
        /// 将String转换为指定Type的Object
        /// </summary>
        /// <param name="value">String值</param>
        /// <param name="type">要转换的Type</param>
        /// <param name="convtype">转换方法,默认自动判断</param>
        /// <param name="att">附加参数,若有</param>
        /// <returns>指定Type的Object</returns>
        /// <param name="convertNoneLineAttribute">是否转换不带LineAttribute的类</param>
        public static object? GetStringObject(string value, Type type, ConvertType convtype = ConvertType.Default, LineAttribute? att = null, bool? convertNoneLineAttribute = null)
        {
            if (att?.Ignore == true)
                return null;
            convertNoneLineAttribute = att?.ConvertNoneLineAttribute ?? convertNoneLineAttribute;
            if (value == "")
            {
                if (type.IsValueType)
                {
                    return Activator.CreateInstance(type);
                }
                else if (type == typeof(string))
                {
                    return string.Empty;
                }
                else if (type.IsArray)
                {
                    return Array.CreateInstance(type.GetElementType(), 0);
                }
                else if (type.IsClass)
                {
                    return Activator.CreateInstance(type);
                }
                else
                {
                    return null;
                }
            }
            else if (value == "/null")
            {
                return null;
            }
            ConvertType ct = convtype == ConvertType.Default ? GetObjectConvertType(type, att) : convtype;
            switch (ct)
            {
                case ConvertType.ToList:
                    IList list = (IList)Activator.CreateInstance(type);//type.GetConstructor(Array.Empty<Type>()).Invoke(null, null);
                    Type? subtype = type.GetGenericArguments().First();
                    foreach (string str in value.Split(','))
                    {
                        list.Add(GetStringObject(Sub.TextDeReplace(str), subtype, convertNoneLineAttribute: convertNoneLineAttribute ?? true));
                    }
                    return list;
                case ConvertType.ToArray:
                    string[] strs = value.Split(',');
                    subtype = type.GetElementType();
#pragma warning disable CS8604 // 引用类型参数可能为 null。
                    Array arr = Array.CreateInstance(subtype, strs.Length);
#pragma warning restore CS8604 // 引用类型参数可能为 null。
                    for (int i = 0; i < strs.Length; i++)
                    {
                        arr.SetValue(GetStringObject(Sub.TextDeReplace(strs[i]), subtype, convertNoneLineAttribute: convertNoneLineAttribute ?? true), i);
                    }
                    return arr;
                case ConvertType.ToDictionary:
                    Type[] subtypes = type.GetGenericArguments();
                    IDictionary dict = (IDictionary)Activator.CreateInstance(type);
                    foreach (string str in value.Replace("/n", "\n").Split('\n'))
                    {
                        strs = str.Split('=');
                        object? k = GetStringObject(Sub.TextDeReplace(strs[0]), subtypes[0], convertNoneLineAttribute: convertNoneLineAttribute ?? true);
                        if (k != null)
                            dict.Add(k, GetStringObject(Sub.TextDeReplace(strs[1]), subtypes[1], convertNoneLineAttribute: convertNoneLineAttribute ?? true));
                    }
                    return dict;
                case ConvertType.ToDateTime:
                    if (long.TryParse(value, out long l))
                    {
                        return new DateTime(l);
                    }
                    if (SetObject.ConverterSetObject.TryGetValue(value.GetType(), out Dictionary<Type, Func<object, object>>? conv))
                    {
                        if (conv?.TryGetValue(type, out Func<object, object>? fun) == true)
                        {
                            return fun(value);
                        }
                    }
                    return DateTime.MinValue;
                case ConvertType.ToFloat:
                    return FInt64.Parse(value);
                case ConvertType.Converter:
                    if (att?.Converter != null)
                        return ConvertFunction.ConvertBack(att.Converter, Sub.TextDeReplace(value));
                    else
                        return Convert.ChangeType(Sub.TextDeReplace(value), type);
                case ConvertType.ToEnum:
                    return Enum.Parse(type, value);
                default:
                case ConvertType.ToString:
                    return Convert.ChangeType(Sub.TextDeReplace(value), type);
                case ConvertType.Class:
                    Line line = new Line(Sub.TextDeReplace(value));
                    object? obj = Activator.CreateInstance(type);
                    if (obj == null)
                        return null;
                    foreach (PropertyInfo mi in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(p => p.CanWrite))
                    {
                        LineAttribute latt = mi.GetCustomAttributes<LineAttribute>().Combine();
                        if (latt != null)
                        {
                            if (latt.Ignore)
                                continue;
                            string name = latt.Name ?? mi.Name;
                            ISub? s = line.Find(name);
                            if (s != null)
                                mi.SetValueSafe(obj, GetSubObject(s, mi.PropertyType, att: latt, convertNoneLineAttribute: convertNoneLineAttribute ?? true));
                        }
                        else if (convertNoneLineAttribute == true)
                        {
                            ISub? s = line.Find(mi.Name);
                            if (s != null)
                                mi.SetValueSafe(obj, GetSubObject(s, mi.PropertyType, convertNoneLineAttribute: true));
                        }
                    }
                    foreach (FieldInfo mi in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(f => !f.Name.StartsWith("<")))
                    {
                        LineAttribute latt = mi.GetCustomAttributes<LineAttribute>().Combine();
                        if (latt != null)
                        {
                            if (latt.Ignore)
                                continue;
                            string name = latt.Name ?? mi.Name;
                            ISub? s = line.Find(name);
                            if (s != null)
                                mi.SetValueSafe(obj, GetSubObject(s, mi.FieldType, att: latt, convertNoneLineAttribute: convertNoneLineAttribute ?? true));
                        }
                        else if (convertNoneLineAttribute == true)
                        {
                            ISub? s = line.Find(mi.Name);
                            if (s != null)
                                mi.SetValueSafe(obj, GetSubObject(s, mi.FieldType, convertNoneLineAttribute: true));
                        }
                    }
                    return obj;
            }
        }
        /// <summary>
        /// 将ISub转换为指定Type的Object
        /// </summary>
        /// <param name="sub">ISub</param>
        /// <param name="type">要转换的Type</param>
        /// <param name="convtype">转换方法,默认自动判断</param>
        /// <param name="att">附加参数,若有</param>
        /// <param name="convertNoneLineAttribute">是否转换不带LineAttribute的类</param>
        /// <returns>指定Type的Object</returns>
        public static object? GetSubObject(ISub sub, Type type, ConvertType convtype = ConvertType.Default, LineAttribute? att = null, bool? convertNoneLineAttribute = null)
        {
            if (att?.Ignore == true)
                return null;
            convertNoneLineAttribute = att?.ConvertNoneLineAttribute ?? convertNoneLineAttribute;
            ConvertType ct = convtype == ConvertType.Default ? GetObjectConvertType(type, att) : convtype;
            if (sub is ILine line && line.info.Length == 0)
                switch (ct)
                {
                    case ConvertType.ToDictionary:
                        Type[] subtypes = type.GetGenericArguments();
                        IDictionary dict = (IDictionary)Activator.CreateInstance(type);
                        foreach (ISub s in line)
                        {
                            object? k = GetStringObject(s.Name, subtypes[0]);
                            if (k != null)
                                dict.Add(k, GetStringObject(s.info, subtypes[1]));
                        }
                        return dict;
                    case ConvertType.Class:
                        object obj = Activator.CreateInstance(type);
                        if (obj == null)
                            return null;
                        foreach (PropertyInfo mi in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(f => f.CanWrite))
                        {
                            LineAttribute? latt = mi.GetCustomAttributes<LineAttribute>().Combine();
                            if (latt != null)
                            {
                                if (latt.Ignore)
                                    continue;
                                string name = latt.Name ?? mi.Name;
                                ISub? s;
                                if (latt.IgnoreCase)
                                {
                                    name = name.ToLower();
                                    s = line.FirstOrDefault(s => s.Name.ToLower() == name);
                                }
                                else
                                    s = line.Find(name);
                                if (s != null)
                                    mi.SetValueSafe(obj, GetSubObject(s, mi.PropertyType, att: latt, convertNoneLineAttribute: convertNoneLineAttribute ?? true));
                            }
                            else if (convertNoneLineAttribute == true)
                            {
                                ISub? s = line.Find(mi.Name);
                                if (s != null)
                                    mi.SetValueSafe(obj, GetSubObject(s, mi.PropertyType, convertNoneLineAttribute: true));
                            }
                        }
                        foreach (FieldInfo mi in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(f => !f.Name.StartsWith("<")))
                        {
                            LineAttribute? latt = mi.GetCustomAttributes<LineAttribute>().Combine();
                            if (latt != null)
                            {
                                if (latt.Ignore)
                                    continue;
                                string name = latt.Name ?? mi.Name;
                                ISub? s;
                                if (latt.IgnoreCase)
                                {
                                    name = name.ToLower();
                                    s = line.FirstOrDefault(s => s.Name.ToLower() == name);
                                }
                                else
                                    s = line.Find(name);
                                if (s != null)
                                    mi.SetValueSafe(obj, GetSubObject(s, mi.FieldType, att: latt, convertNoneLineAttribute: convertNoneLineAttribute ?? true));
                            }
                            else if (convertNoneLineAttribute == true)
                            {
                                ISub? s = line.Find(mi.Name);
                                if (s != null)
                                    mi.SetValueSafe(obj, GetSubObject(s, mi.FieldType, convertNoneLineAttribute: true));
                            }
                        }
                        return obj;
                    case ConvertType.ToArray:
                        Type subtype = type.GetElementType()!;
                        Array arr = Array.CreateInstance(subtype, line.Count);
                        for (int i = 0; i < line.Count; i++)
                        {
                            arr.SetValue(GetSubObject(line[i], subtype, convertNoneLineAttribute: convertNoneLineAttribute ?? true), i);
                        }
                        return arr;

                    case ConvertType.ToList:
                        var list = (IList)Activator.CreateInstance(type);
                        subtype = type.GetGenericArguments().First();
                        foreach (ISub s in line)
                        {
                            list.Add(GetSubObject(s, subtype, convertNoneLineAttribute: convertNoneLineAttribute ?? true));
                        }
                        return list;

                    default:
                        return GetStringObject(sub.info, type, ct, att);
                }
            else
                return GetStringObject(sub.info, type, ct, att);
        }
        /// <summary>
        /// 安全设置字段值
        /// </summary>
        public static void SetValueSafe(this FieldInfo mi, object obj, object? value)
        {
            // Check if the types match
            if (value != null && !mi.FieldType.IsAssignableFrom(value.GetType()))
            {
                // Try to convert the value
                if (value is IConvertible convertible)
                {
                    value = Convert.ChangeType(convertible, mi.FieldType);
                }
                else
                {
                    // Try to use a possible implicit conversion
                    var implicitConversionMethod = value?.GetType().GetMethod("op_Implicit", new[] { mi.FieldType });
                    if (implicitConversionMethod != null)
                    {
                        value = implicitConversionMethod.Invoke(null, new[] { value });
                    }
                    else
                    {
                        return;
                        //throw new ArgumentException("The provided value cannot be converted to the field type.");
                    }
                }
            }

            // Now you can set the value
            mi.SetValue(obj, value);
        }
        /// <summary>
        /// 安全设置字段值
        /// </summary>
        public static void SetValueSafe(this PropertyInfo mi, object obj, object? value)
        {
            // Check if the types match
            if (value != null && !mi.PropertyType.IsAssignableFrom(value.GetType()))
            {
                // Try to convert the value
                if (value is IConvertible convertible)
                {
                    value = Convert.ChangeType(convertible, mi.PropertyType);
                }
                else
                {
                    // Try to use a possible implicit conversion
                    var implicitConversionMethod = value?.GetType().GetMethod("op_Implicit", new[] { mi.PropertyType });
                    if (implicitConversionMethod != null)
                    {
                        value = implicitConversionMethod.Invoke(null, new[] { value });
                    }
                    else
                    {
                        return;
                        //throw new ArgumentException("The provided value cannot be converted to the field type.");
                    }
                }
            }

            // Now you can set the value
            mi.SetValue(obj, value);
        }
#pragma warning restore CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。
#pragma warning restore CS8602 // 解引用可能出现空引用。
        /// <summary>
        /// 将指定的LPS反序列化为T对象
        /// </summary>
        /// <typeparam name="T">想要获得的类型</typeparam>
        /// <param name="lps">ILPS</param>
        /// <returns>生成的对象</returns>
        /// <param name="convertNoneLineAttribute">是否转换不带LineAttribute的类</param>
        public static T? DeserializeObject<T>(ILPS lps, bool convertNoneLineAttribute = false) where T : new()
        {
            return DeserializeObject<T>(lps.ToArray(), convertNoneLineAttribute);
        }
        /// <summary>
        /// 将指定的ISub/ILine反序列化为T对象
        /// </summary>
        /// <typeparam name="T">想要获得的类型</typeparam>
        /// <param name="value">ISub/ILine</param>
        /// <returns>生成的对象</returns>
        /// <param name="convertNoneLineAttribute">是否转换不带LineAttribute的类</param>
        public static T? DeserializeObject<T>(ISub value, bool convertNoneLineAttribute = false) where T : new()
        {
            object? o = GetSubObject(value, typeof(T), convertNoneLineAttribute: convertNoneLineAttribute);
            return o == null ? default : (T)o;
        }
        /// <summary>
        /// 将指定的ILine列表反序列化为T对象
        /// </summary>
        /// <typeparam name="T">想要获得的类型</typeparam>
        /// <param name="value">ILine列表</param>
        /// <returns>生成的对象</returns>
        /// <param name="convertNoneLineAttribute">是否转换不带LineAttribute的类</param>
        public static T? DeserializeObject<T>(ILine[] value, bool convertNoneLineAttribute = false) where T : new()
        {
            Line l = new Line();
            l.AddRange(value);
            return DeserializeObject<T>(l, convertNoneLineAttribute);
        }
        /// <summary>
        /// 快速转换为指定类型
        /// </summary>
        /// <typeparam name="T">任何类型</typeparam>
        /// <param name="value">需要转换的值</param>
        /// <param name="defvalue">默认值</param>
        /// <returns>转换结果</returns>
        public static T TryConvertValue<T>(this object? value, T defvalue)
        {
            if (value == null)
            {
                return defvalue;
            }
            else if (value is T t)
            {
                return t;
            }
            else
            {
                object? v = SetObject.ConvertTo<T>(value);
                if (v != null)
                {
                    return (T)v;
                }
                else
                    return defvalue;
            }
        }
        /// <summary>
        /// 快速转换回指定类型
        /// </summary>
        /// <typeparam name="T">任何类型</typeparam>
        /// <param name="value">需要转换的值</param>
        /// <returns>转换结果</returns>
        public static T? TryConvertToValue<T>(this object? value)
        {
            if (value == null)
                return default;
            if (value is T t)
            {
                return t;
            }
            else
            {
                object? v = SetObject.ConvertTo<T>(value);
                if (v != null)
                {
                    return (T)v;
                }
                else
                    return default;
            }
        }
        /// <summary>
        /// 组合所有的LineAttribute, 后面覆盖前面
        /// </summary>
        /// <param name="lineAttributes">LineAttribute 列表</param>
        /// <returns>LineAttribute</returns>
        public static LineAttribute? Combine(this IEnumerable<LineAttribute> lineAttributes)
        {
            LineAttribute? la = lineAttributes.FirstOrDefault();
            foreach (LineAttribute item in lineAttributes.Skip(1))
            {
#pragma warning disable CS8604
                la = LineAttribute.Combine(la, item);
#pragma warning restore CS8604
            }
            return la;
        }
    }
}
