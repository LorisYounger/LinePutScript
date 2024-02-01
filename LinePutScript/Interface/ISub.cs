using LinePutScript.Structure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable enable
namespace LinePutScript
{
    /// <summary>
    /// 子类接口 LinePutScript最基础的类的最基础的接口
    /// </summary>
    public interface ISub : ISetObject
    {
        /// <summary>
        /// 加载 通过lps文本创建一个子类
        /// </summary>
        /// <param name="lps">lps文本</param>
        public void Load(string lps);
        /// <summary>
        /// 加载 通过名字和信息创建新的Sub
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="info">信息 (正常版本)</param>
        public void Load(string name, string info);
        /// <summary>
        /// 将其他Sub内容拷贝到本Sub
        /// </summary>
        /// <param name="sub">其他Sub</param>
        void Set(ISub sub);

        /// <summary>
        /// 名称 没有唯一性
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 信息 (去除关键字的文本) (string)
        /// </summary>
        string info { get; set; }
        /// <summary>
        /// 信息 (去除关键字的文本) (可复制)
        /// </summary>
        ICloneable infoCloneable { get;  }
        /// <summary>
        /// 信息 (去除关键字的文本) (可比较)
        /// </summary>
        IComparable infoComparable { get; }
        /// <summary>
        /// 信息 (正常)
        /// </summary>
        string Info { get; set; }
        /// <summary>
        /// 信息 (int)
        /// </summary>
        int InfoToInt { get; set; }
        /// <summary>
        /// 信息 (int64)
        /// </summary>
        long InfoToInt64 { get; set; }
        /// <summary>
        /// 信息 (double)
        /// </summary>
        double InfoToDouble { get; set; }
        /// <summary>
        /// 信息 (bool)
        /// </summary>
        bool InfoToBoolean { get; set; }
        /// <summary>
        /// 返回一个 Info集合 的第一个string。
        /// </summary>
        /// <returns>要返回的第一个string</returns>
        string? First();
        /// <summary>
        /// 返回一个 Info集合 的最后一个string。
        /// </summary>
        /// <returns>要返回的最后一个string</returns>
        string? Last();
        /// <summary>
        /// 退回Info的反转义文本 (正常显示)
        /// </summary>
        /// <returns>info的反转义文本 (正常显示)</returns>
        string GetInfo();
        /// <summary>
        /// 退回Info集合的转义文本集合 (正常显示)
        /// </summary>
        /// <returns>info的转义文本集合 (正常显示)</returns>
        string[] GetInfos();
        /// <summary>
        /// 将当前Sub转换成文本格式 (info已经被转义/去除关键字)
        /// </summary>
        /// <returns>Sub的文本格式 (info已经被转义/去除关键字)</returns>
        string ToString();

        /// <summary>
        /// 获得该Sub的哈希代码
        /// </summary>
        /// <returns>32位哈希代码</returns>
        int GetHashCode();
        /// <summary>
        /// 获得该Sub的长哈希代码
        /// </summary>
        /// <returns>64位哈希代码</returns>
        long GetLongHashCode();
        /// <summary>
        /// 获得Info的String结构
        /// </summary>
        public StringStructure Infos { get; }
    }
}
