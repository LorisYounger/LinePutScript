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
    /// 行接口 包含多个子类的接口 继承自子类接口
    /// </summary>
    public interface ILine : ISub, IList<ISub>, ICollection<ISub>, IEnumerable<ISub>, IEnumerable, IGetOBJ<ISub>, IComparable<ILine>, IEquatable<ILine>
    {
        /// <summary>
        /// 通过名字和信息创建新的Line
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="info">信息 (正常)</param>
        /// <param name="text">文本 在末尾没有结束行号的文本 (正常)</param>
        /// <param name="subs">子类集合</param>
        public void Load(string name, string info, string text = "", params ISub[] subs);
        /// <summary>
        /// 通过名字和信息创建新的Line
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="info">信息 (正常)</param>
        /// <param name="text">文本 在末尾没有结束行号的文本 (正常)</param>
        /// <param name="subs">子类集合</param>
        public void Load(string name, string info, IEnumerable<ISub> subs, string text = "");
        /// <summary>
        /// 将其他Line内容拷贝到本Line
        /// </summary>
        /// <param name="line">其他line</param>
        public void Load(ILine line);
        /// <summary>
        /// 文本 在末尾没有结束行号的文本 (去除关键字的文本)
        /// </summary>
        public string text { get; set; }
        /// <summary>
        /// 文本 在末尾没有结束行号的文本 (正常)
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 注释 ///为注释
        /// </summary>
        public string Comments { get; set; }
        /// <summary>
        /// 文本 (int)
        /// </summary>
        public int TextToInt { get; set; }
        /// <summary>
        /// 文本 (int64)
        /// </summary>
        public long TextToInt64 { get; set; }
        /// <summary>
        /// 文本 (double)
        /// </summary>
        public double TextToDouble { get; set; }
        /// <summary>
        /// 退回Text的反转义文本 (正常显示)
        /// </summary>
        /// <returns>Text的反转义文本 (正常显示)</returns>
        public string GetText();
        #region List操作
        /// <summary>
        /// 将指定的Sub添加到Subs列表的末尾
        /// </summary>
        /// <param name="newSub">要添加的Sub</param>
        public void AddSub(ISub newSub);
        /// <summary>
        /// 若无相同名称(Name)的Sub,则将指定的Sub添加到Subs列表的末尾
        /// 若有,则替换成要添加的Sub
        /// </summary>
        /// <param name="newSub">要添加的Sub</param>
        public void AddorReplaceSub(ISub newSub);
        /// <summary>
        /// 将指定Sub的元素添加到Subs的末尾
        /// </summary>
        /// <param name="newSubs">要添加的多个Sub</param>
        public void AddRange(IEnumerable<ISub> newSubs);

        /// <summary>
        /// 将指定的Sub添加到指定索引处
        /// </summary>
        /// <param name="index">应插入 Sub 的从零开始的索引</param>
        /// <param name="newSub">要添加的Sub</param>
        public void InsertSub(int index, ISub newSub);
        /// <summary>
        /// 将指定Sub的元素添加指定索引处
        /// </summary>
        /// <param name="index">应插入 Sub 的从零开始的索引</param>
        /// <param name="newSubs">要添加的多个Sub</param>
        public void InsertRange(int index, IEnumerable<ISub> newSubs);
        /// <summary>
        /// 从Subs中移除特定名称的第一个匹配项
        /// </summary>
        /// <param name="SubName">要从Subs中删除的Sub的名称</param>
        /// <returns>如果成功移除了Sub,则为 true; 否则为 false</returns>
        public bool Remove(string SubName);
        /// <summary>
        /// 从Subs中移除特定名称的所有元素
        /// </summary>
        /// <param name="SubName">要从Subs中删除的Sub的名称</param>
        public void RemoveAll(string SubName);
        /// <summary>
        /// 返回一个值,确定某Sub是否在Line集合中
        /// </summary>
        /// <param name="value">要在Line集合中定位的Sub的名字</param>
        /// <returns>如果在Line集合中找到符合的名字,则为True; 否则为false</returns>
        public bool Contains(string value);

        /// <summary>
        /// 匹配拥有相同名称的Line或sub的所有元素
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的sub,其中所有元素均与指定谓词定义的条件匹配,则为该数组; 否则为一个空的Array</returns>
        public ISub[] FindAll(string subName);
        /// <summary>
        /// 匹配拥有相同名称和信息的Line或sub的所有元素
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <param name="subinfo">用于定义匹配的信息 (去除关键字的文本)</param>
        /// <returns>如果找到相同名称和信息的sub,其中所有元素均与指定谓词定义的条件匹配,则为该数组; 否则为一个空的Array</returns>
        public ISub[] FindAll(string subName, string subinfo);
        /// <summary>
        /// 匹配拥有相同信息的Line或sub的所有元素
        /// </summary>
        /// <param name="subinfo">用于定义匹配的信息 (去除关键字的文本)</param>
        /// <returns>如果找到相同信息的sub,其中所有元素均与指定谓词定义的条件匹配,则为该数组; 否则为一个空的Array</returns>
        public ISub[] FindAllInfo(string subinfo);
        /// <summary>
        /// 搜索与指定名称,并返回Line或整个Subs中的第一个匹配元素
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的第一个sub,则为该sub; 否则为null</returns>
        public ISub? Find(string subName);
        /// <summary>
        /// 搜索与指定名称,并返回整个Assemblage中的第一个匹配元素
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <param name="subinfo">用于定义匹配的信息 (去除关键字的文本)</param>
        /// <returns>如果找到相同名称和信息的第一个Line,则为该Line; 否则为null</returns>
        public ISub? Find(string subName, string subinfo);
        /// <summary>
        /// 搜索与指定信息,并返回整个Assemblage中的第一个匹配元素
        /// </summary>
        /// <param name="subinfo">用于定义匹配的信息 (去除关键字的文本)</param>
        /// <returns>如果找到相同信息的第一个Line,则为该Line; 否则为null</returns>
        public ISub? FindInfo(string subinfo);
        /// <summary>
        /// 搜索与指定名称,并返回Line或整个Subs中的第一个匹配元素;若未找到,则新建并添加相同名称的Sub,并且返回这个Sub
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的第一个sub,则为该sub; 否则为新建的相同名称sub</returns>
        public ISub? FindorAdd(string subName);
        /// <summary>
        /// 搜索全部相似名称的Sub的所有元素
        /// </summary>
        /// <param name="value">%字段%</param>
        /// <returns>如果找到相似名称的Sub,则为数组; 否则为一个空的Array</returns>
        public ISub[] SeachALL(string value);
        /// <summary>
        /// 搜索字段是否出现在Line名称,并返回整个Subs中的第一个匹配元素
        /// </summary>
        /// <param name="value">%字段%</param>
        /// <returns>如果找到相似名称的第一个Sub,则为该Sub; 否则为null</returns>
        public ISub? Seach(string value);
        /// <summary>
        /// 搜索相同名称的Sub,并返回整个Subs中第一个匹配的sub从零开始的索引
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的sub的第一个元素,则为该元素的从零开始的索引; 否则为 -1</returns>
        public int IndexOf(string subName);
        /// <summary>
        /// 搜索相同名称的Sub,并返回整个Sub中全部匹配的sub从零开始的索引
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的sub的元素,则为该元素的从零开始的索引组; 否则为空的Array</returns>
        public int[] IndexsOf(string subName);
        #endregion

        /// <summary>
        /// 将当前Line转换成文本格式 (info已经被转义/去除关键字) 将输出储存到StringBuilder
        /// </summary>
        /// <param name="str">储存到的 StringBuilder</param>
        /// <returns>Line的文本格式 (info已经被转义/去除关键字)</returns>
        public void ToString(StringBuilder str);

        /// <summary>
        /// 返回一个 Subs 的第一个元素。
        /// </summary>
        /// <returns>要返回的第一个元素</returns>
        public new ISub? First();
        /// <summary>
        /// 返回一个 Subs 的最后一个元素。
        /// </summary>
        /// <returns>要返回的最后一个元素</returns>
        public new ISub? Last();
        /// <summary>
        /// 返回一个新List,包含所有Subs
        /// </summary>
        /// <returns>所有储存的Subs</returns>
        public List<ISub> ToList();
        /// <summary>
        /// 获得Text的String结构
        /// </summary>
        public StringStructure Texts { get; }
    }
}
