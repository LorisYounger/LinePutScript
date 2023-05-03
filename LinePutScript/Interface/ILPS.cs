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
    /// LPS接口
    /// </summary>
    public interface ILPS : IList<ILine>, ICollection<ILine>, IEnumerable<ILine>, IEnumerable, IGetOBJ<ILine>
    {
        /// <summary>
        /// 返回一个新List,包含所有Line
        /// </summary>
        /// <returns>所有储存的Line</returns>
        public List<ILine> ToList();
        #region List操作
        /// <summary>
        /// 将指定的Line添加到Assemblage列表的末尾
        /// </summary>
        /// <param name="newLine">要添加的Line</param>
        public void AddLine(ILine newLine);
        /// <summary>
        /// 若无相同名称(Name)的Line,则将指定的Line添加到Assemblage列表的末尾
        /// 若有,则替换成要添加的Line
        /// </summary>
        /// <param name="newLine">要添加的Line</param>
        public void AddorReplaceLine(ILine newLine);
        /// <summary>
        /// 将指定Line的元素添加到Assemblage的末尾
        /// </summary>
        /// <param name="newLines">要添加的多个Line</param>
        public void AddRange(params ILine[] newLines);
        /// <summary>
        /// 将指定Line的元素添加到Assemblage的末尾
        /// </summary>
        /// <param name="newLines">要添加的多个Line</param>
        public void AddRange(IEnumerable<ILine> newLines);
        /// <summary>
        /// 将指定的Line添加到指定索引处
        /// </summary>
        /// <param name="index">应插入 Line 的从零开始的索引</param>
        /// <param name="newLine">要添加的Line</param>
        public void InsertLine(int index, ILine newLine);
        /// <summary>
        /// 将指定Line的元素添加指定索引处
        /// </summary>
        /// <param name="index">应插入 Line 的从零开始的索引</param>
        /// <param name="newLines">要添加的多个Line</param>
        public void InsertRange(int index, params ILine[] newLines);
        /// <summary>
        /// 将指定Line的元素添加指定索引处
        /// </summary>
        /// <param name="index">应插入 Line 的从零开始的索引</param>
        /// <param name="newLines">要添加的多个Line</param>
        public void InsertRange(int index, IEnumerable<ILine> newLines);
        /// <summary>
        /// 从Assemblage中移除特定名称的第一个匹配项
        /// </summary>
        /// <param name="lineName">要从Assemblage中删除的Line的名称</param>
        /// <returns>如果成功移除了line,则为 true; 否则为 false</returns>
        public bool Remove(string lineName);

        /// <summary>
        /// 从Assemblage中移除移除与条件相匹配的所有Line
        /// </summary>
        /// <param name="line">要从Assemblage中删除的Line</param>
        public void RemoveAll(ILine line);
        /// <summary>
        /// 从Assemblage中移除移除与名称相匹配的所有Line
        /// </summary>
        /// <param name="lineName">要从Assemblage中删除的Line的名称</param>
        public void RemoveAll(string lineName);
        /// <summary>
        /// 确定某Sub是否在Assemblage中
        /// </summary>
        /// <param name="sub">要在Assemblage中定位的Sub</param>
        /// <returns>如果在Assemblage中找到line,则为True; 否则为false</returns>
        public bool Contains(ISub sub);
        /// <summary>
        /// 确定某Line(名字定位)是否在Assemblage中
        /// </summary>
        /// <param name="value">Line的名字</param>
        /// <returns>如果在Assemblage中找到相同的名字,则为True; 否则为false</returns>
        public bool ContainsLine(string value);
        /// <summary>
        /// 确定某sub(名字定位)是否在Assemblage中
        /// </summary>
        /// <param name="value">sub的名字</param>
        /// <returns>如果在Assemblage的sub中找到相同的名字,则为True; 否则为false</returns>
        public bool ContainsSub(string value);
        /// <summary>
        /// 匹配拥有相同名称的Line的所有元素
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的Line,其中所有元素均与指定谓词定义的条件匹配,则为该数组; 否则为一个空的Array</returns>
        public ILine[] FindAllLine(string lineName);
        /// <summary>
        /// 匹配拥有相同名称和信息的Line的所有元素
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <param name="lineinfo">用于定义匹配的信息 (去除关键字的文本)</param>
        /// <returns>如果找到相同名称和信息的Line,其中所有元素均与指定谓词定义的条件匹配,则为该数组; 否则为一个空的Array</returns>
        public ILine[] FindAllLine(string lineName, string lineinfo);
        /// <summary>
        /// 匹配拥有相同信息的Line的所有元素
        /// </summary>
        /// <param name="lineinfo">用于定义匹配的信息 (去除关键字的文本)</param>
        /// <returns>如果找到相同信息的Line,其中所有元素均与指定谓词定义的条件匹配,则为该数组; 否则为一个空的Array</returns>
        public ILine[] FindAllLineInfo(string lineinfo);
        /// <summary>
        /// 搜索与指定名称,并返回整个Assemblage中的第一个匹配元素
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的第一个Line,则为该Line; 否则为null</returns>
        public ILine FindLine(string lineName);
        /// <summary>
        /// 搜索与指定名称和信息,并返回整个Assemblage中的第一个匹配元素
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <param name="lineinfo">用于定义匹配的信息 (去除关键字的文本)</param>
        /// <returns>如果找到相同名称和信息的第一个Line,则为该Line; 否则为null</returns>
        public ILine FindLine(string lineName, string lineinfo);
        /// <summary>
        /// 搜索与指定信息,并返回整个Assemblage中的第一个匹配元素
        /// </summary>
        /// <param name="lineinfo">用于定义匹配的信息 (去除关键字的文本)</param>
        /// <returns>如果找到相同信息的第一个Line,则为该Line; 否则为null</returns>
        public ILine FindLineInfo(string lineinfo);
        /// <summary>
        /// 搜索与指定名称,并返回整个Assemblage中的第一个匹配元素; 若未找到,则新建并添加相同名称的Line,并且返回这个Line
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的第一个Line,则为该Line; 否则为新建的相同名称Line</returns>
        public ILine FindorAddLine(string lineName);
        /// <summary>
        /// 匹配拥有相同名称的Sub的所有元素
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的Sub,其中所有元素均与指定谓词定义的条件匹配,则为该数组; 否则为一个空的Array</returns>
        public ISub[] FindAllSub(string subName);
        /// <summary>
        /// 匹配拥有相同名称和信息的Sub的所有元素
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <param name="subinfo">用于定义匹配的信息 (去除关键字的文本)</param>
        /// <returns>如果找到相同名称和信息的Sub,其中所有元素均与指定谓词定义的条件匹配,则为该数组; 否则为一个空的Array</returns>
        public ISub[] FindAllSub(string subName, string subinfo);
        /// <summary>
        /// 匹配拥有相同信息的Sub的所有元素
        /// </summary>
        /// <param name="subinfo">用于定义匹配的信息 (去除关键字的文本)</param>
        /// <returns>如果找到相同信息的Sub,其中所有元素均与指定谓词定义的条件匹配,则为该数组; 否则为一个空的Array</returns>
        public ISub[] FindAllSubInfo(string subinfo);
        /// <summary>
        /// 搜索与指定名称,并返回整个Assemblage中的第一个匹配元素
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的第一个Sub,则为该Line; 否则为null</returns>
        public ISub FindSub(string subName);
        /// <summary>
        /// 搜索与指定名称和信息,并返回整个Assemblage中的第一个匹配元素
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <param name="subinfo">用于定义匹配的信息 (去除关键字的文本)</param>
        /// <returns>如果找到相同名称和信息的第一个Sub,则为该Line; 否则为null</returns>
        public ISub FindSub(string subName, string subinfo);
        /// <summary>
        /// 搜索与指定信息,并返回整个Assemblage中的第一个匹配元素
        /// </summary>
        /// <param name="subinfo">用于定义匹配的信息 (去除关键字的文本)</param>
        /// <returns>如果找到相同信息的第一个Sub,则为该Line; 否则为null</returns>
        public ISub FindSubInfo(string subinfo);

        /// <summary>
        /// 搜索全部相似名称的Line的所有元素
        /// </summary>
        /// <param name="value">%字段%</param>
        /// <returns>如果找到相似名称的Line,则为数组; 否则为一个空的Array</returns>
        public ILine[] SearchAllLine(string value);
        /// <summary>
        /// 搜索字段是否出现在Line名称,并返回整个Assemblage中的第一个匹配元素
        /// </summary>
        /// <param name="value">%字段%</param>
        /// <returns>如果找到相似名称的第一个Line,则为该Line; 否则为null</returns>
        public ILine SearchLine(string value);
        /// <summary>
        /// 搜索全部相似名称的Sub的所有元素
        /// </summary>
        /// <param name="value">%字段%</param>
        /// <returns>如果找到相似名称的Line,则为该数组; 否则为一个空的Array</returns>
        public ISub[] SearchAllSub(string value);
        /// <summary>
        /// 搜索字段是否出现在Sub名称,并返回整个Assemblage中的第一个匹配元素
        /// </summary>
        /// <param name="value">%字段%</param>
        /// <returns>如果找到相同名称的第一个Sub,则为该Sub; 否则为null</returns>
        public ISub SearchSub(string value);

        /// <summary>
        /// 搜索相同名称的Line,并返回整个Assemblage中第一个匹配的Line从零开始的索引
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的Line的第一个元素,则为该元素的从零开始的索引; 否则为 -1</returns>
        public int IndexOf(string lineName);
        /// <summary>
        /// 搜索相同名称的Line,并返回整个Assemblage中全部匹配的Line从零开始的索引
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的Line的元素,则为该元素的从零开始的索引组; 否则为空的Array</returns>
        public int[] IndexsOf(string lineName);
        #endregion
        /// <summary>
        /// 从指定的字符串加载LPS文档
        /// </summary>
        /// <param name="lps">包含要加载的LPS文档的字符串</param>
        public void Load(string lps);
        /// <summary>
        /// 返回一个Assemblage的第一个元素。
        /// </summary>
        /// <returns>要返回的第一个元素</returns>
        public ILine First();
        /// <summary>
        /// 返回一个Assemblage的最后一个元素。
        /// </summary>
        /// <returns>要返回的最后一个元素</returns>
        public ILine Last();
        /// <summary>
        /// 获得该LPS文档的长哈希代码
        /// </summary>
        /// <returns>64位哈希代码</returns>
        public long GetLongHashCode();
        /// <summary>
        /// 获得该LPS文档的哈希代码
        /// </summary>
        /// <returns>32位哈希代码</returns>
        public int GetHashCode();

    }
}
