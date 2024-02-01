using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LinePutScript.SetObject;
using fint64 = LinePutScript.FInt64;

#nullable enable
namespace LinePutScript
{
    /// <summary>
    /// 任何类型的值均可储存的接口
    /// </summary>
    public interface ISetObject : ICloneable, IComparable, IEquatable<ISetObject>, IComparable<ISetObject>
    {
        /// <summary>
        /// 储存的数据
        /// </summary>
        dynamic Value { get; set; }

        #region 转换函数
        /// <summary>
        /// 转换成为储存String类型
        /// </summary>
        public string GetStoreString();
        /// <summary>
        /// 转换成 String 类型
        /// </summary>
        public string GetString();
        /// <summary>
        /// 转换成 long 类型
        /// </summary>
        public long GetInteger64();
        /// <summary>
        /// 转换成 int 类型
        /// </summary>
        public int GetInteger();
        /// <summary>
        /// 转换成 double 类型
        /// </summary>
        public double GetDouble();
        /// <summary>
        /// 转换成 double(int64) 类型
        /// </summary>
        public fint64 GetFloat();
        /// <summary>
        /// 转换成 DateTime 类型
        /// </summary>
        public DateTime GetDateTime();
        /// <summary>
        /// 转换成 bool 类型
        /// </summary>
        public bool GetBoolean();
        /// <summary>
        /// 设置 string 值
        /// </summary>
        public void SetString(string value);
        /// <summary>
        /// 设置 int 值
        /// </summary>
        public void SetInteger(int value);
        /// <summary>
        /// 设置 long 值
        /// </summary>
        public void SetInteger64(long value);
        /// <summary>
        /// 设置 double 值
        /// </summary>
        public void SetDouble(double value);
        /// <summary>
        /// 设置 float 值
        /// </summary>
        public void SetFloat(fint64 value);
        /// <summary>
        /// 设置 DateTime 值
        /// </summary>
        public void SetDateTime(DateTime value);
        /// <summary>
        /// 设置 bool 值
        /// </summary>
        public void SetBoolean(bool value);
        #endregion


    }
}
