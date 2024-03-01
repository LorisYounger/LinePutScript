using LinePutScript.Converter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static LinePutScript.Sub;
#nullable enable
namespace LinePutScript
{
    /// <summary>
    /// 通过Line包装类
    /// </summary>
    public class Line_C<T> : Sub_C<T>, ILine
    {
        /// <summary>
        /// 获取 T.Text (若有) (去除关键字的文本)
        /// </summary>
        public string text { get => GetString("Text"); set => SetString("Text", value); }
        /// <summary>
        /// 获取 T.Text(若有) (正常)
        /// </summary>
        public string Text
        {
            get => TextDeReplace(text);
            set
            {
                text = TextReplace(value);
            }
        }
        /// <summary>
        /// 获取 T.Comments(若有)
        /// </summary>
        public string Comments { get => GetString("Comments"); set => SetString("Comments", value); }

        /// <summary>
        /// 获得Text的String结构
        /// </summary>
        public StringStructure Texts
        {
            get
            {
                texts ??= new StringStructure((x) => text = x, () => text);
                return texts;
            }
        }
        StringStructure? texts;
        /// <summary>
        /// 新建一个Line包装类
        /// </summary>
        /// <param name="value">要包装的值</param>
        /// <param name="attribute">行内容描述</param>
        /// <param name="name">类名称</param>
        public Line_C(T? value, string? name = null, LineAttribute? attribute = null) : base(value, name, attribute)
        {
        }
        /// <summary>
        /// 新建一个Line包装类
        /// </summary>
        /// <param name="line">从现有数据添加</param>
        /// <param name="attribute">行内容描述</param>
        public Line_C(ILine line, LineAttribute? attribute = null) : base(default, null, attribute)
        {
            Load(line);
        }

        /// <summary>
        /// 文本 (int)
        /// </summary>
        public int TextToInt
        {
            get
            {
                if (int.TryParse(text, out int i))
                    return i;
                else
                    return 0;
            }
            set
            {
                text = value.ToString();
            }
        }
        /// <summary>
        /// 文本 (int64)
        /// </summary>
        public long TextToInt64
        {
            get
            {
                if (long.TryParse(text, out long i))
                    return i;
                else
                    return 0;
            }
            set
            {
                text = value.ToString();
            }
        }
        /// <summary>
        /// 文本 (double)
        /// </summary>
        public double TextToDouble
        {
            get
            {
                if (double.TryParse(text, out double i))
                    return i;
                else
                    return 0;
            }
            set
            {
                text = value.ToString();
            }
        }
        /// <summary>
        /// 获取Sub数量
        /// </summary>
        public int Count => typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Length
            + typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Length;
        /// <summary>
        /// 是否只读
        /// </summary>
        public bool IsReadOnly => false;
        /// <inheritdoc/>

        public ISub this[string subName] { get => Find(subName); set => Set(value); }

        #region GOBJ

        /// <summary>
        /// 获取或设置 String 属性的sub
        /// </summary>
        /// <param name="subName">(gstr)用于定义匹配的名称</param>
        /// <returns>获取或设置对 String 属性的Sub</returns>
        public string? this[gstr subName]
        {
            get => GetString((string)subName);
            set => SetString((string)subName, value);
        }
        /// <summary>
        /// 获取或设置 Bool 属性的sub
        /// </summary>
        /// <param name="subName">(gbol)用于定义匹配的名称</param>
        /// <returns>获取或设置对 bool 属性的Sub</returns>
        public bool this[gbol subName]
        {
            get => GetBool((string)subName);
            set => SetBool((string)subName, value);
        }

        /// <summary>
        /// 获取或设置 Int 属性的sub
        /// </summary>
        /// <param name="subName">(gint)用于定义匹配的名称</param>
        /// <returns>获取或设置对 int 属性的Sub</returns>
        public int this[gint subName]
        {
            get => GetInt((string)subName);
            set => SetInt((string)subName, value);
        }

        /// <summary>
        /// 获取或设置 Long 属性的sub
        /// </summary>
        /// <param name="subName">(gi64)用于定义匹配的名称</param>
        /// <returns>获取或设置对 long 属性的Sub</returns>
        public long this[gi64 subName]
        {
            get => GetInt64((string)subName);
            set => SetInt64((string)subName, value);
        }

        /// <summary>
        /// 获取或设置 Double 属性的sub
        /// </summary>
        /// <param name="subName">(gdbe)用于定义匹配的名称</param>
        /// <returns>获取或设置对 double 属性的Sub</returns>
        public double this[gdbe subName]
        {
            get => GetDouble((string)subName);
            set => SetDouble((string)subName, value);
        }

        /// <summary>
        /// 获取或设置 Double(long) 属性的sub  通过转换long获得更精确的小数,小数位最大保留9位
        /// </summary>
        /// <param name="subName">(gflt)用于定义匹配的名称</param>
        /// <returns>获取或设置对 double 属性的Sub</returns>
        public FInt64 this[gflt subName]
        {
            get => GetFloat((string)subName);
            set => SetFloat((string)subName, value);
        }

        /// <summary>
        /// 获取或设置 DateTime 属性的sub
        /// </summary>
        /// <param name="subName">(gdbe)用于定义匹配的名称</param>
        /// <returns>获取或设置对 double 属性的Sub</returns>
        public DateTime this[gdat subName]
        {
            get => GetDateTime((string)subName);
            set => SetDateTime((string)subName, value);
        }
        #endregion


        /// <summary>
        /// 通过引索修改Line中Sub内容(错误:类没有引索)
        /// </summary>
        /// <param name="index">要获得或设置的引索</param>
        /// <returns>引索指定的Sub</returns>
        [Obsolete("错误:类没有引索")]
        public ISub this[int index] { get => throw new ArrayTypeMismatchException(); set => throw new ArrayTypeMismatchException(); }


        /// <inheritdoc/>
        public void Load(string name, string info, string text = "", params ISub[] subs)
        {
            Name = name;
            this.info = info;
            this.text = text;
            AddRange(subs);
        }
        /// <inheritdoc/>
        public void Load(string name, string info, IEnumerable<ISub> subs, string text = "")
        {
            Name = name;
            if (Value != null)
            {
                this.info = info;
                this.text = text;
                AddRange(subs);
            }
        }
        /// <inheritdoc/>
        public void Load(ILine line)
        {
            Name = line.Name;
            if (Value != null)
            {
                info = line.info;
                text = line.text;
                AddRange(line);
            }
            else
            {
                Value = LPSConvert.GetStringObject(line.info, typeof(T)).TryConvertToValue<T>();
            }
        }


        /// <inheritdoc/>
        public string GetText() => Text;
        /// <inheritdoc/>
        public void AddSub(ISub newSub) => Set(newSub);
        /// <inheritdoc/>
        public void AddorReplaceSub(ISub newSub) => Set(newSub);
        /// <inheritdoc/>
        public void AddRange(IEnumerable<ISub> newSubs)
        {
            foreach (var item in newSubs)
            {
                Set(item);
            }
        }

        /// <summary>
        /// 将指定的Sub添加到指定索引处(失效:类没有顺序)
        /// </summary>
        /// <param name="index">应插入 Sub 的从零开始的索引(失效)</param>
        /// <param name="newSub">要添加的Sub</param>
        [Obsolete("失效:类没有顺序")]
        public void InsertSub(int index, ISub newSub)
        {
            Add(newSub);
        }
        /// <summary>
        /// 将指定Sub的元素添加指定索引处(失效:类没有顺序)
        /// </summary>
        /// <param name="index">应插入 Sub 的从零开始的索引</param>
        /// <param name="newSubs">要添加的多个Sub</param>
        [Obsolete("失效:类没有顺序")]
        public void InsertRange(int index, IEnumerable<ISub> newSubs)
        {
            AddRange(newSubs);
        }
        /// <summary>
        /// 清除指定项数据, 一般是为null或者0
        /// </summary>
        public bool Remove(string SubName)
        {
            var s = Find(SubName);
            if (s != null)
            {
                s.Info = "";
                return true;
            }
            return false;
        }
        /// <summary>
        /// 从Subs中移除特定名称的所有元素(失效:类为单一性)
        /// </summary>
        /// <param name="SubName">要从Subs中删除的Sub的名称</param>
        [Obsolete("失效:类没有顺序")]
        public void RemoveAll(string SubName) => Remove(SubName);
        /// <inheritdoc/>
        public bool Contains(string value) => Find(value) != null;
        /// <summary>
        /// 匹配拥有相同名称的Line或sub的所有元素(注意:在类中,信息是唯一的)
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的sub,其中所有元素均与指定谓词定义的条件匹配,则为该数组; 否则为一个空的Array</returns>
        [Obsolete("注意:在类中,信息是唯一的")]
        public ISub[] FindAll(string subName)
        {
            var sub = Find(subName);
            if (sub != null)
                return new ISub[] { sub };
            return new ISub[] { };
        }
        /// <summary>
        /// 匹配拥有相同名称和信息的Line或sub的所有元素(注意:在类中,信息是唯一的)
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <param name="subinfo">用于定义匹配的信息 (去除关键字的文本)</param>
        /// <returns>如果找到相同名称和信息的sub,其中所有元素均与指定谓词定义的条件匹配,则为该数组; 否则为一个空的Array</returns>
        [Obsolete("注意:在类中,信息是唯一的")]
        public ISub[] FindAll(string subName, string subinfo)
        {
            var sub = Find(subName, subinfo);
            if (sub != null)
                return new ISub[] { sub };
            return new ISub[] { };
        }
        /// <summary>
        /// 匹配拥有相同信息的Line或sub的所有元素
        /// </summary>
        /// <param name="subinfo">用于定义匹配的信息 (去除关键字的文本)</param>
        /// <returns>如果找到相同信息的sub,其中所有元素均与指定谓词定义的条件匹配,则为该数组; 否则为一个空的Array</returns>
        public ISub[] FindAllInfo(string subinfo) => Subs().Where(x => x.info == subinfo).ToArray();
        /// <inheritdoc/>
        public ISub? Find(string subName)
        {
            foreach (var mi in typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                LineAttribute? latt = mi.GetCustomAttribute<LineAttribute>();
                string name = latt?.Name ?? mi.Name;
                if (name == subName)
                {
                    Type subc = typeof(Sub_C<>).MakeGenericType(mi.PropertyType);
#pragma warning disable CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。
#pragma warning disable CS8602 // 解引用可能出现空引用。

                    INotifyCollectionChanged sub = (INotifyCollectionChanged)Activator.CreateInstance(subc, new object?[] { mi.GetValue(Value), name, latt });
                    sub.CollectionChanged += (x, y) => mi.SetValue(Value, y.NewItems[0]);
                    return (ISub)sub;
                }
            }
            foreach (var mi in typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                LineAttribute? latt = mi.GetCustomAttribute<LineAttribute>();
                string name = latt?.Name ?? mi.Name;
                if (name == subName)
                {
                    Type subc = typeof(Sub_C<>).MakeGenericType(mi.FieldType);
                    INotifyCollectionChanged sub = (INotifyCollectionChanged)Activator.CreateInstance(subc, new object?[] { mi.GetValue(Value), name, latt });
                    sub.CollectionChanged += (x, y) => mi.SetValue(Value, y.NewItems[0]);
                    return (ISub)sub;
                }
            }
            return null;
        }
#pragma warning restore CS8602 // 解引用可能出现空引用。
#pragma warning restore CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。
        /// <inheritdoc/>
        public ISub? Find(string subName, string subinfo)
        {
            var sub = Find(subName);
            if (sub != null && sub.info == subinfo)
                return sub;
            return null;
        }
        /// <inheritdoc/>
        public ISub? FindInfo(string subinfo) => Subs().FirstOrDefault(x => x.info == subinfo);
        /// <inheritdoc/>
        public ISub FindorAdd(string subName) => Find(subName) ?? throw new NullReferenceException();
        /// <inheritdoc/>   
        public ISub[] SeachALL(string value) => Subs().Where(x => x.Name.Contains(value)).ToArray();
        /// <inheritdoc/>
        public ISub? Seach(string value) => Subs().FirstOrDefault(x => x.Name.Contains(value));

        /// <summary>
        /// 搜索相同名称的Sub,并返回整个Subs中第一个匹配的sub从零开始的索引(错误:类没有引索)
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的sub的第一个元素,则为该元素的从零开始的索引; 否则为 -1</returns>
        [Obsolete("错误:类没有引索")]
        public int IndexOf(string subName)
        {
            throw new ArrayTypeMismatchException();
        }
        /// <summary>
        /// 搜索相同名称的Sub,并返回整个Subs中第一个匹配的Sub从零开始的索引(错误:类没有引索)
        /// </summary>
        /// <param name="item">用于定义匹配的Sub</param>
        /// <returns>如果找到相同名称的Sub的第一个元素,则为该元素的从零开始的索引; 否则为 -1</returns>
        [Obsolete("错误:类没有引索")]
        public int IndexOf(ISub item)
        {
            throw new ArrayTypeMismatchException();
        }
        /// <summary>
        /// 搜索相同名称的Sub,并返回整个Sub中全部匹配的sub从零开始的索引(错误:类没有引索)
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的sub的元素,则为该元素的从零开始的索引组; 否则为空的Array</returns>
        [Obsolete("错误:类没有引索")]
        public int[] IndexsOf(string subName)
        {
            throw new ArrayTypeMismatchException();
        }
        ISub ILine.First() => Subs().First();

        ISub ILine.Last() => Subs().Last();
        /// <inheritdoc/>
        public List<ISub> ToList() => Subs();

        /// <summary>
        /// 将指定的Sub添加到指定索引处 (失效:类没有顺序)
        /// </summary>
        /// <param name="index">应插入 Sub 的从零开始的索引(失效)</param>
        /// <param name="item">要添加的Sub</param>
        [Obsolete("失效:类没有顺序")]
        public void Insert(int index, ISub item) => Set(item);

        /// <summary>
        /// 从Subs中移除特定引索的Sub (错误:类没有顺序)
        /// </summary>
        /// <param name="index">要删除Sub的引索</param>
        [Obsolete("错误:类没有顺序")]
        public void RemoveAt(int index)
        {
            throw new ArrayTypeMismatchException();
        }
        /// <inheritdoc/>
        public void Add(ISub item) => Set(item);

        /// <summary>
        /// 清除所有数据, 慎用!
        /// </summary>
        public void Clear()
        {
            foreach (var mi in typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                mi.SetValue(Value, default);
            }
            foreach (var mi in typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                mi.SetValue(Value, default);
            }
        }
        /// <inheritdoc/>
        public bool Contains(ISub item)
        {
            var sub = Find(item.Name);
            return sub != null;
        }

        /// <summary>
        /// 将整个array复制到Line的Subs
        /// </summary>
        /// <param name="array">复制到Subs的Sub列表</param>
        /// <param name="arrayIndex">从零开始的引索,从引索处开始复制</param>
        public void CopyTo(ISub[] array, int arrayIndex)
        {
            for (int i = arrayIndex; i < array.Length; i++)
                Set(array[i]);
        }
        /// <summary>
        /// 清除该属性数据, 一般是为null或者0
        /// </summary>
        public bool Remove(ISub item) => Remove(item.Name);

        IEnumerator<ISub> IEnumerable<ISub>.GetEnumerator() => Subs().GetEnumerator();
        /// <inheritdoc/>
        public bool GetBool(string subName) => Find(subName)?.GetBoolean() ?? false;
        /// <inheritdoc/>
        public void SetBool(string subName, bool value) => Find(subName)?.SetBoolean(value);
        /// <inheritdoc/>
        public int GetInt(string subName, int defaultvalue = 0) => Find(subName)?.GetInteger() ?? defaultvalue;
        /// <inheritdoc/>
        public void SetInt(string subName, int value) => Find(subName)?.SetInteger(value);
        /// <inheritdoc/>
        public long GetInt64(string subName, long defaultvalue = 0) => Find(subName)?.GetInteger64() ?? defaultvalue;
        /// <inheritdoc/>
        public void SetInt64(string subName, long value) => Find(subName)?.SetInteger64(value);
        /// <inheritdoc/>
        public FInt64 GetFloat(string subName, FInt64 defaultvalue = default) => Find(subName)?.GetFloat() ?? defaultvalue;
        /// <inheritdoc/>
        public void SetFloat(string subName, FInt64 value) => Find(subName)?.SetFloat(value);
        /// <inheritdoc/>
        public DateTime GetDateTime(string subName, DateTime defaultvalue = default) => Find(subName)?.GetDateTime() ?? defaultvalue;
        /// <inheritdoc/>
        public void SetDateTime(string subName, DateTime value) => Find(subName)?.SetDateTime(value);
        /// <inheritdoc/>
        public string? GetString(string subName, string? defaultvalue = null) => Find(subName)?.GetString() ?? defaultvalue;
        /// <inheritdoc/>
        public void SetString(string subName, string? value) => Find(subName)?.SetString(value ?? string.Empty);
        /// <inheritdoc/>
        public double GetDouble(string subName, double defaultvalue = 0) => Find(subName)?.GetDouble() ?? defaultvalue;
        /// <inheritdoc/>
        public void SetDouble(string subName, double value) => Find(subName)?.SetDouble(value);
        /// <inheritdoc/>
        public int CompareTo(ILine? other)
        {
            if (Value == null)
            {
                return -1;
            }
            if (other == null)
            {
                return 1;
            }
            if (other is Line_C<T> line)
            {
                if (line.Value is IComparable<T> comparable)
                {
                    return -comparable.CompareTo(Value);
                }
            }
            if (other is ISub sub)
            {
                return CompareTo(sub);
            }
            if (other is T t)
            {
                CompareTo(t);
            }
            return -1;
        }
        /// <inheritdoc/>
        public bool Equals(ILine? other) => CompareTo(other) == 0;
#pragma warning disable CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。
#pragma warning disable CS8602 // 解引用可能出现空引用。
        /// <summary>
        /// 获取所有可能的Subs
        /// </summary>
        public List<ISub> Subs()
        {
            List<ISub> list = new List<ISub>();
            foreach (var mi in typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                LineAttribute? latt = mi.GetCustomAttribute<LineAttribute>();
                string name = mi.Name;
                Type subc = typeof(Sub_C<>).MakeGenericType(mi.PropertyType);
                INotifyCollectionChanged sub = (INotifyCollectionChanged)Activator.CreateInstance(subc, new object?[] { mi.GetValue(Value), name, latt });
                sub.CollectionChanged += (x, y) => mi.SetValue(Value, y.NewItems[0]);
                list.Add((ISub)sub);
            }
            foreach (var mi in typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                LineAttribute? latt = mi.GetCustomAttribute<LineAttribute>();
                string name = mi.Name;
                Type subc = typeof(Sub_C<>).MakeGenericType(mi.FieldType);
                INotifyCollectionChanged sub = (INotifyCollectionChanged)Activator.CreateInstance(subc, new object?[] { mi.GetValue(Value), name, latt });
                sub.CollectionChanged += (x, y) => mi.SetValue(Value, y.NewItems[0]);
                list.Add((ISub)sub);
            }
            return list;
        }
#pragma warning restore CS8602 // 解引用可能出现空引用。
#pragma warning restore CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。
        /// <inheritdoc/>
        public IEnumerator GetEnumerator() => Subs().GetEnumerator();
        /// <summary>
        /// 将当前Line转换成文本格式 (info已经被转义/去除关键字)
        /// </summary>
        /// <returns>Line的文本格式 (info已经被转义/去除关键字)</returns>
        public override string ToString()
        {
            StringBuilder str = new StringBuilder(Name);
            if (str.Length != 0)
                str.Append(":|");
            foreach (ISub su in Subs())
                str.Append(su.ToString());
            return str.ToString();
        }

        /// <summary>
        /// 将当前Line转换成文本格式 (info已经被转义/去除关键字) 将输出储存到StringBuilder
        /// </summary>
        /// <param name="str">储存到的 StringBuilder</param>
        /// <returns>Line的文本格式 (info已经被转义/去除关键字)</returns>
        public void ToString(StringBuilder str)
        {
            str.Append('\n' + Name);
            if (str.Length != 0)
                str.Append(":|");
            foreach (ISub su in Subs())
                str.Append(su.ToString());
        }
    }
}
