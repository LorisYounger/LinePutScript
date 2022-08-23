using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

#nullable enable
namespace LinePutScript
{
    /// <summary>
    /// String 结构 通过String实时修改具体参数,为LineputScript提供第四层结构
    /// </summary>
    public class StringStructure : IEnumerable<KeyValuePair<string, string>>, IComparable<string>, IDictionary<string, string>, IEnumerable, IComparable<StringStructure>
    {
        protected private Action<string> setstr;
        protected private Func<string> getstr;
        /// <summary>
        /// 是否为单线程
        /// </summary>
        public bool Single;
        /// <summary>
        /// 生成可以修改的String结构
        /// </summary>
        /// <param name="setstr">设置String方法 (非转义)</param>
        /// <param name="getstr">获取String方法 (非转义)</param>
        /// <param name="single">是否为单线程,如果为单线程,将会缓存数据列表,设置为true将会提高读取效率</param>
        public StringStructure(Action<string> setstr, Func<string> getstr, bool single = true)
        {
            this.setstr = setstr;
            this.getstr = getstr;
            Single = single;
        }
        private Dictionary<string, string>? cache;
        private Dictionary<string, string> Cache
        {
            get
            {
                if (!Single || cache == null)
                {
                    cache = new Dictionary<string, string>();
                    var strs = Sub.Split(getstr(), "/n");
                    foreach (string str in strs)
                    {
                        var strs2 = str.Split(new char[1] { '=' }, 2);
                        if (strs2.Length == 2)
                            cache.Add(strs2[0], strs2[1]);
                    }
                }
                return cache;
            }
            set
            {
                StringBuilder sb = new StringBuilder();
                foreach (var kv in value)
                {
                    sb.Append(kv.Key);
                    sb.Append('=');
                    sb.Append(kv.Value);
                    sb.Append('\n');
                }
                setstr(sb.ToString());
            }
        }

        /// <summary>
        /// 获取相关项目String
        /// </summary>
        /// <param name="key">项目名称</param>
        /// <param name="defaultvalue">默认值</param>
        /// <returns>如果找到项目则返回值;否则为返回默认值</returns>
        public string? GetString(string key, string? defaultvalue = default)
        {
            if (Cache.ContainsKey(key))
            {
                return Sub.TextDeReplace(Cache[key]);
            }
            else
                return defaultvalue;
        }
        /// <summary>
        /// 设置相关项目String
        /// </summary>
        /// <param name="key">项目名称</param>
        /// <param name="value">设置项目的String值</param>
        public void SetString(string key, string? value)
        {
            var c = Cache;
            if (value == null)
                c.Remove(key);
            else
                c[key] = Sub.TextReplace(value);
            Cache = c;
        }

        #region GETER
        /// <summary>
        /// 获取相关项目String 如果为空则退回""
        /// </summary>
        public string this[string name]
        {
            get
            {
                return GetString(name) ?? "";
            }
            set
            {
                SetString(name, value);
            }
        }
        /// <summary>
        /// 获得bool属性的项目
        /// </summary>
        /// <param name="key">项目名称</param>
        /// <param name="defaultvalue">如果没找到返回的默认值</param>
        /// <returns>如果找到项目则返回项目中的值,若未找到则返回默认值</returns>
        public bool GetBool(string key, bool defaultvalue = false)
        {
            if (Cache.ContainsKey(key))
            {
                var c = Cache[key].ToLower();
                return c == "true" || c == "1" || c == "t";
            }
            else
                return defaultvalue;
        }
        /// <summary>
        /// 设置bool属性的项目
        /// </summary>
        /// <param name="key">项目名称</param>
        /// <param name="value">
        /// 如果为ture,则在没有相同name为key的sub时候添加新的sub
        /// 如果为false,则删除所有name为key的sub
        /// </param>
        public void SetBool(string key, bool value)
        {
            SetString(key, value ? "true" : "false");
        }
        /// <summary>
        /// 获得int属性的项目
        /// </summary>
        /// <param name="key">项目名称</param>
        /// <param name="defaultvalue">如果没找到返回的默认值</param>
        /// <returns>
        /// 如果找到项目则返回项目中储存的int值
        /// 如果没找到,则返回默认值
        /// </returns>
        public int GetInt(string key, int defaultvalue = default)
        {
            if (Cache.ContainsKey(key))
            {
                return Convert.ToInt32(Cache[key]);
            }
            else
                return defaultvalue;
        }
        /// <summary>
        /// 设置int属性的项目
        /// </summary>
        /// <param name="key">项目名称</param>
        /// <param name="value">设置项目的int值</param>
        public void SetInt(string key, int value) => SetString(key, value.ToString());

        /// <summary>
        /// 获得long属性的项目
        /// </summary>
        /// <param name="key">项目名称</param>
        /// <param name="defaultvalue">如果没找到返回的默认值</param>
        /// <returns>
        /// 如果找到项目则返回项目中储存的long值
        /// 如果没找到,则返回默认值
        /// </returns>
        public long GetInt64(string key, long defaultvalue = default)
        {
            if (Cache.ContainsKey(key))
            {
                return Convert.ToInt64(Cache[key]);
            }
            else
                return defaultvalue;
        }
        /// <summary>
        /// 设置long属性的项目
        /// </summary>
        /// <param name="key">项目名称</param>
        /// <param name="value">设置项目的long值</param>
        public void SetInt64(string key, long value) => SetString(key, value.ToString());

        /// <summary>
        /// 获得double(long)属性的项目 通过转换long获得更精确的小数,小数位最大保留9位
        /// </summary>
        /// <param name="key">项目名称</param>
        /// <param name="defaultvalue">如果没找到返回的默认值</param>
        /// <returns>
        /// 如果找到项目则返回项目中储存的double(long)值
        /// 如果没找到,则返回默认值
        /// </returns>
        public double GetFloat(string key, double defaultvalue = default)
        {
            if (Cache.ContainsKey(key))
            {
                return Convert.ToInt64(Cache[key]) / 1000000000.0;
            }
            else
                return defaultvalue;
        }
        /// <summary>
        /// 设置double(long)属性的项目 通过转换long获得更精确的小数,小数位最大保留9位
        /// </summary>
        /// <param name="key">项目名称</param>
        /// <param name="value">设置项目的double(long)值</param>
        public void SetFloat(string key, double value) => SetString(key, ((long)(value * 1000000000)).ToString());

        /// <summary>
        /// 获得DateTime属性的项目
        /// </summary>
        /// <param name="key">项目名称</param>
        /// <param name="defaultvalue">如果没找到返回的默认值</param>
        /// <returns>
        /// 如果找到项目则返回项目中储存的DateTime值
        /// 如果没找到,则返回默认值
        /// </returns>
        public DateTime GetDateTime(string key, DateTime defaultvalue = default)
        {
            if (Cache.ContainsKey(key))
            {
                return new DateTime(Convert.ToInt64(Cache[key]));
            }
            else
                return defaultvalue;
        }
        /// <summary>
        /// 设置DateTime属性的项目
        /// </summary>
        /// <param name="key">项目名称</param>
        /// <param name="value">设置项目的DateTime值</param>
        public void SetDateTime(string key, DateTime value) => SetString(key, value.Ticks.ToString());


        /// <summary>
        /// 获得double属性的项目
        /// </summary>
        /// <param name="key">项目名称</param>
        /// <param name="defaultvalue">如果没找到返回的默认值</param>
        /// <returns>
        /// 如果找到项目则返回项目中储存的double值
        /// 如果没找到,则返回默认值
        /// </returns>
        public double GetDouble(string key, double defaultvalue = default)
        {
            if (Cache.ContainsKey(key))
            {
                return Convert.ToDouble(Cache[key]);
            }
            else
                return defaultvalue;
        }
        /// <summary>
        /// 设置double属性的项目
        /// </summary>
        /// <param name="key">项目名称</param>
        /// <param name="value">设置项目的double值</param>
        public void SetDouble(string key, double value) => SetString(key, value.ToString());
        #endregion

        #region GOBJ

        /// <summary>
        /// 获取或设置 String 属性的项目
        /// </summary>
        /// <param name="key">(gstr)项目名称</param>
        /// <returns>获取或设置对 String 属性的项目</returns>
        public string? this[gstr key]
        {
            get => GetString((string)key);
            set => SetString((string)key, value);
        }
        /// <summary>
        /// 获取或设置 Bool 属性的项目
        /// </summary>
        /// <param name="key">(gbol)项目名称</param>
        /// <returns>获取或设置对 bool 属性的项目</returns>
        public bool this[gbol key]
        {
            get => GetBool((string)key);
            set => SetBool((string)key, value);
        }

        /// <summary>
        /// 获取或设置 Int 属性的项目
        /// </summary>
        /// <param name="key">(gint)项目名称</param>
        /// <returns>获取或设置对 int 属性的项目</returns>
        public int this[gint key]
        {
            get => GetInt((string)key);
            set => SetInt((string)key, value);
        }

        /// <summary>
        /// 获取或设置 Long 属性的项目
        /// </summary>
        /// <param name="key">(gi64)项目名称</param>
        /// <returns>获取或设置对 long 属性的项目</returns>
        public long this[gi64 key]
        {
            get => GetInt64((string)key);
            set => SetInt64((string)key, value);
        }

        /// <summary>
        /// 获取或设置 Double 属性的项目
        /// </summary>
        /// <param name="key">(gdbe)项目名称</param>
        /// <returns>获取或设置对 double 属性的项目</returns>
        public double this[gdbe key]
        {
            get => GetDouble((string)key);
            set => SetDouble((string)key, value);
        }

        /// <summary>
        /// 获取或设置 Double(long) 属性的项目  通过转换long获得更精确的小数,小数位最大保留9位
        /// </summary>
        /// <param name="key">(gflt)项目名称</param>
        /// <returns>获取或设置对 double 属性的项目</returns>
        public double this[gflt key]
        {
            get => GetFloat((string)key);
            set => SetFloat((string)key, value);
        }

        /// <summary>
        /// 获取或设置 DateTime 属性的项目
        /// </summary>
        /// <param name="key">(gdbe)项目名称</param>
        /// <returns>获取或设置对 double 属性的项目</returns>
        public DateTime this[gdat key]
        {
            get => GetDateTime((string)key);
            set => SetDateTime((string)key, value);
        }
        #endregion


        #region Interface
        /// <summary>
        /// 获取Key集合
        /// </summary>
        public ICollection<string> Keys => Cache.Keys;

        /// <summary>
        /// 获取Values集合(原始值)
        /// </summary>
        public ICollection<string> Values => Cache.Values;

        /// <summary>
        /// 获取集合大小
        /// </summary>
        public int Count => Cache.Count;
        /// <summary>
        /// IDictionary
        /// </summary>
        public bool IsReadOnly => ((IDictionary)Cache).IsReadOnly;
        /// <summary>
        /// IDictionary
        /// </summary>
        public bool IsFixedSize => ((IDictionary)Cache).IsFixedSize;
        /// <summary>
        /// IDictionary
        /// </summary>
        public object SyncRoot => ((ICollection)Cache).SyncRoot;
        /// <summary>
        /// IDictionary
        /// </summary>
        public bool IsSynchronized => ((ICollection)Cache).IsSynchronized;
        /// <summary>
        /// 是否包含指定的键
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>true为包含,false为不包含</returns>
        public bool ContainsKey(string key)
        {
            return ((IDictionary<string, string>)Cache).ContainsKey(key);
        }
        /// <summary>
        /// 设置相关项目String
        /// </summary>
        /// <param name="key">项目名称</param>
        /// <param name="value">设置项目的String值</param>
        public void Add(string key, string value)
        {
            var c = Cache;
            if (value == null)
                c.Remove(key);
            else
                c[key] = Sub.TextReplace(value);
            Cache = c;
        }
        /// <summary>
        /// 从项目列表中删除指定的键
        /// </summary>
        /// <param name="key">项目名称</param>
        /// <returns>是否删除成功</returns>
        public bool Remove(string key)
        {
            var c = Cache;
            var b = c.Remove(key);
            Cache = c;
            return b;
        }


        /// <summary>
        /// 设置相关项目String
        /// </summary>
        /// <param name="item">配对(原始值)</param>
        public void Add(KeyValuePair<string, string> item)
        {
            SetString(item.Key, item.Value);
        }
        /// <summary>
        /// 移除所有项目
        /// </summary>
        public void Clear()
        {
            cache = null;
            setstr("");
        }
        /// <summary>
        /// 查看是否包含特定的配对
        /// </summary>
        /// <param name="item">配对(原始值)</param>
        /// <returns></returns>
        public bool Contains(KeyValuePair<string, string> item)
        {
            return ((ICollection<KeyValuePair<string, string>>)Cache).Contains(item);
        }
        /// <summary>
        /// 从引索开始复制匹配
        /// </summary>
        /// <param name="array">配对列表(原始值)</param>
        /// <param name="arrayIndex">引索</param>
        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            var c = Cache;
            ((ICollection<KeyValuePair<string, string>>)c).CopyTo(array, arrayIndex);
            Cache = c;
        }
        /// <summary>
        /// 移除第一个匹配的配对
        /// </summary>
        /// <param name="item">配对</param>
        /// <returns>是否删除成功</returns>
        public bool Remove(KeyValuePair<string, string> item)
        {
            var c = Cache;
            var b = ((ICollection<KeyValuePair<string, string>>)Cache).Remove(item);
            Cache = c;
            return b;
        }
        /// <summary>
        /// 获取迭代(原始值)
        /// </summary>
        /// <returns>迭代器</returns>
        IEnumerator<KeyValuePair<string, string>> IEnumerable<KeyValuePair<string, string>>.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, string>>)Cache).GetEnumerator();
        }
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="other">其他String</param>
        /// <returns>如果相等则为0</returns>
        public int CompareTo(string? other)
        {
            return getstr().CompareTo(other);
        }
        /// <summary>
        /// 获取迭代(原始值)
        /// </summary>
        /// <returns>迭代器</returns>
        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)Cache).GetEnumerator();
        }
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="other">其他StringStructure</param>
        /// <returns>如果相等则为0</returns>
        public int CompareTo(StringStructure? other)
        {
            return getstr().CompareTo(other?.getstr());
        }
        /// <summary>
        /// 获取相关项目String
        /// </summary>
        /// <param name="key">项目名称</param>
        /// <param name="value">如果找到项目则返回值(原始值)</param>
        public bool TryGetValue(string key, out string value)
        {
            var ret = ((IDictionary<string, string>)Cache).TryGetValue(key, out var o);
            value = o ?? "";
            return ret;
        }
        #endregion
    }
}
