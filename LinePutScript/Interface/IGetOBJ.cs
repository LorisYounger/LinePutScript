using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable enable
namespace LinePutScript
{
    /// <summary>
    /// 支持使用GOBJ获取设置的接口
    /// </summary>
    public interface IGetOBJ<T>
    {
        #region GETER
        /// <summary>
        /// 搜索与指定名称,并返回Line或整个Subs中的第一个匹配元素;若未找到,则新建并添加相同名称的Sub,并且返回这个Sub
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的第一个sub,则为该sub; 否则为新建的相同名称sub</returns>
        public T this[string subName] { get; set; }
        /// <summary>
        /// 获得bool属性的sub
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的sub,则为True; 否则为false</returns>
        public bool GetBool(string subName);
        /// <summary>
        /// 设置bool属性的sub
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <param name="value">
        /// 值
        /// </param>
        public void SetBool(string subName, bool value);
        /// <summary>
        /// 获得int属性的sub
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <param name="defaultvalue">如果没找到返回的默认值</param>
        /// <returns>
        /// 如果找到相同名称的sub,返回sub中储存的int值
        /// 如果没找到,则返回默认值
        /// </returns>
        public int GetInt(string subName, int defaultvalue = default);
        /// <summary>
        /// 设置int属性的sub
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <param name="value">储存进sub的int值</param>
        public void SetInt(string subName, int value);

        /// <summary>
        /// 获得long属性的sub
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <param name="defaultvalue">如果没找到返回的默认值</param>
        /// <returns>
        /// 如果找到相同名称的sub,返回sub中储存的long值
        /// 如果没找到,则返回默认值
        /// </returns>
        public long GetInt64(string subName, long defaultvalue = default);
        /// <summary>
        /// 设置long属性的sub
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <param name="value">储存进sub的long值</param>
        public void SetInt64(string subName, long value);

        /// <summary>
        /// 获得double(long)属性的sub 通过转换long获得更精确的小数,小数位最大保留9位
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <param name="defaultvalue">如果没找到返回的默认值</param>
        /// <returns>
        /// 如果找到相同名称的sub,返回sub中储存的double(long)值
        /// 如果没找到,则返回默认值
        /// </returns>
        public FInt64 GetFloat(string subName, FInt64 defaultvalue = default);
        /// <summary>
        /// 设置double(long)属性的sub 通过转换long获得更精确的小数,小数位最大保留9位
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <param name="value">储存进sub的double(long)值</param>
        public void SetFloat(string subName, FInt64 value);

        /// <summary>
        /// 获得DateTime属性的sub
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <param name="defaultvalue">如果没找到返回的默认值</param>
        /// <returns>
        /// 如果找到相同名称的sub,返回sub中储存的DateTime值
        /// 如果没找到,则返回默认值
        /// </returns>
        public DateTime GetDateTime(string subName, DateTime defaultvalue = default);
        /// <summary>
        /// 设置DateTime属性的sub
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <param name="value">储存进sub的DateTime值</param>
        public void SetDateTime(string subName, DateTime value);

        /// <summary>
        /// 获得String属性的sub
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <param name="defaultvalue">如果没找到返回的默认值</param>
        /// <returns>
        /// 如果找到相同名称的sub,返回sub中储存的String值
        /// 如果没找到,则返回默认值
        /// </returns>
        public string? GetString(string subName, string? defaultvalue = default);
        /// <summary>
        /// 设置String属性的sub
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <param name="value">储存进sub的String值</param>
        public void SetString(string subName, string? value);

        /// <summary>
        /// 获得double属性的sub
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <param name="defaultvalue">如果没找到返回的默认值</param>
        /// <returns>
        /// 如果找到相同名称的sub,返回sub中储存的double值
        /// 如果没找到,则返回默认值
        /// </returns>
        public double GetDouble(string subName, double defaultvalue = default);
        /// <summary>
        /// 设置double属性的sub
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <param name="value">储存进sub的double值</param>
        public void SetDouble(string subName, double value);
        #endregion
        #region GOBJ

        /// <summary>
        /// 获取或设置 String 属性的sub
        /// </summary>
        /// <param name="subName">(gstr)用于定义匹配的名称</param>
        /// <returns>获取或设置对 String 属性的Sub</returns>
        public string? this[gstr subName] { get; set; }
        /// <summary>
        /// 获取或设置 Bool 属性的sub
        /// </summary>
        /// <param name="subName">(gbol)用于定义匹配的名称</param>
        /// <returns>获取或设置对 bool 属性的Sub</returns>
        public bool this[gbol subName] { get; set; }

        /// <summary>
        /// 获取或设置 Int 属性的sub
        /// </summary>
        /// <param name="subName">(gint)用于定义匹配的名称</param>
        /// <returns>获取或设置对 int 属性的Sub</returns>
        public int this[gint subName] { get; set; }

        /// <summary>
        /// 获取或设置 Long 属性的sub
        /// </summary>
        /// <param name="subName">(gi64)用于定义匹配的名称</param>
        /// <returns>获取或设置对 long 属性的Sub</returns>
        public long this[gi64 subName] { get; set; }

        /// <summary>
        /// 获取或设置 Double 属性的sub
        /// </summary>
        /// <param name="subName">(gdbe)用于定义匹配的名称</param>
        /// <returns>获取或设置对 double 属性的Sub</returns>
        public double this[gdbe subName] { get; set; }

        /// <summary>
        /// 获取或设置 Double(long) 属性的sub  通过转换long获得更精确的小数,小数位最大保留9位
        /// </summary>
        /// <param name="subName">(gflt)用于定义匹配的名称</param>
        /// <returns>获取或设置对 double 属性的Sub</returns>
        public FInt64 this[gflt subName] { get; set; }

        /// <summary>
        /// 获取或设置 DateTime 属性的sub
        /// </summary>
        /// <param name="subName">(gdbe)用于定义匹配的名称</param>
        /// <returns>获取或设置对 double 属性的Sub</returns>
        public DateTime this[gdat subName] { get; set; }
        #endregion
    }
}
