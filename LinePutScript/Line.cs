using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LinePutScript.Sub;

namespace LinePutScript
{
    /// <summary>
    /// 行 包含多个子类 继承自子类
    /// </summary>
    public class Line : Line<Sub, SetObject>
    {
        /// <summary>
        /// 创建一行
        /// </summary>
        public Line() { }
        /// <summary>
        /// 通过lpsLine文本创建一行
        /// </summary>
        /// <param name="lpsLine">lpsSub文本</param>
        public Line(string lpsLine) : base(lpsLine) { }
        /// <summary>
        /// 通过名字和信息创建新的Line
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="info">信息 (正常)</param>
        /// <param name="text">文本 在末尾没有结束行号的文本 (正常)</param>
        /// <param name="subs">子类集合</param>
        public Line(string name, string info, string text = "", params Sub[] subs) : base(name, info, text, subs) { }
        /// <summary>
        /// 通过其他Line创建新的Line
        /// </summary>
        /// <param name="line">其他line</param>
        public Line(Line line) : base(line) { }

        /// <summary>
        /// 克隆一个Line
        /// </summary>
        /// <returns>相同的Line</returns>
        public new object Clone()
        {
            return new Line(this);
        }
        /// <summary>
        /// 返回一个新List,包含所有Subs
        /// </summary>
        /// <returns>所有储存的Subs</returns>
        public new List<Sub> ToList()
        {
            return Subs.ToList();
        }
    }
}
