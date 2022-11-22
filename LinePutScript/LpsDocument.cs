using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinePutScript
{
    /// <summary>
    /// 文件 包括文件读写等一系列操作
    /// </summary>
    public class LpsDocument : LPS<Line, Sub, SetObject>
    {
        /// <summary>
        /// 创建一个 LpsDocument
        /// </summary>
        public LpsDocument() { }
        /// <summary>
        /// 从指定的字符串创建 LpsDocument
        /// </summary>
        /// <param name="lps">包含要加载的LPS文档的字符串</param>
        public LpsDocument(string lps) : base(lps) { }


        //就是行操作
        #region LPT操作
        int lineNode;
        /// <summary>
        /// 当前Line读取进度
        /// </summary>
        public int LineNode
        {
            get { return lineNode; }
            set
            {
                if (value > Assemblage.Count)
                    lineNode = Assemblage.Count;
                else if (value < 0)
                    lineNode = 0;
                else
                    lineNode = value;
            }
        }
        /// <summary>
        /// 读取读取进度当前Line 并且自动切换到下一Line
        /// </summary>
        /// <returns>如何有则返回当前Line,如果没有则返回null</returns>
        public Line ReadNext()
        {
            if (LineNode == Assemblage.Count)
                return default;
            return Assemblage[LineNode++];
        }
        /// <summary>
        /// 获取读取进度当前Line
        /// </summary>
        /// <returns>如何有则返回当前Line,如果没有则返回null</returns>
        public Line Read()
        {
            if (LineNode == Assemblage.Count)
                return default;
            return Assemblage[LineNode];
        }

        /// <summary>
        /// 将指定的Line添加到当前读取进度之后
        /// </summary>
        /// <param name="newline">要添加的Line</param>
        public void Append(Line newline)
        {
            InsertLine(LineNode + 1, newline);
        }
        /// <summary>
        /// 新建的Line添加到当前读取进度之后
        /// </summary>
        /// <param name="newlineName">要添加的行名称</param>
        /// <param name="info">行信息</param>
        /// <param name="text">行文本</param>
        /// <param name="subs">行子类</param>
        public void Append(string newlineName, string info = "", string text = "", params Sub[] subs)
        {
            Line t = new Line();
            t.Load(newlineName, info, text, subs);
            InsertLine(LineNode + 1, t);
        }
        /// <summary>
        /// 将指定的Sub添加到当前读取进度Line中
        /// </summary>
        /// <param name="newSub">要添加的子类</param>
        public void AppendSub(params Sub[] newSub)
        {
            Read()?.AddRange(newSub);
        }
        /// <summary>
        /// 将指定的Sub添加到当前读取进度Line中
        /// </summary>
        /// <param name="newSubName">要添加的行名称</param>
        /// <param name="info">要添加的行信息</param>
        public void AppendSub(string newSubName, string info = "")
        {
            Sub u = new Sub();
            u.Load(newSubName, info);
            Read()?.AddSub(u);
        }


        /// <summary>
        /// 将读取进度设置为0
        /// </summary>
        public void ReadReset()
        {
            LineNode = 0;
        }
        /// <summary>
        /// 将读取进度设置为上限 即最后一行
        /// </summary>
        public void ReadEnd()
        {
            LineNode = Assemblage.Count;
        }
        /// <summary>
        /// 判断是否能够继续读取数据
        /// </summary>
        /// <returns>如果还有下一行,返回True,否则False</returns>
        public bool ReadCanNext()
        {
            return LineNode != Assemblage.Count;
        }
        #endregion
    }
}
