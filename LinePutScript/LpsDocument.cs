using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinePutScript
{
    /// <summary>
    /// 文件 包括文件读写等一系列操作
    /// </summary>
    public class LpsDocument
    {/// <summary>
     /// 集合 全部文件的数据
     /// </summary>
        public List<Line> Assemblage = new List<Line>();

        /// <summary>
        /// 创建一个 LpsDocument
        /// </summary>
        public LpsDocument() { }
        /// <summary>
        /// 从指定的字符串创建 LpsDocument
        /// </summary>
        /// <param name="lps">包含要加载的LPS文档的字符串</param>
        public LpsDocument(string lps)
        {
            Load(lps);
        }

        #region List操作
        /// <summary>
        /// 将指定的Line添加到Assemblage列表的末尾
        /// </summary>
        /// <param name="newLine">要添加的Line</param>
        public void AddLine(Line newLine)
        {
            Assemblage.Add(newLine);
        }
        /// <summary>
        /// 将指定Line的元素添加到Assemblage的末尾
        /// </summary>
        /// <param name="newLines">要添加的多个Line</param>
        public void AddRange(params Line[] newLines)
        {
            Assemblage.AddRange(newLines);
        }

        /// <summary>
        /// 将指定的Line添加到指定索引处
        /// </summary>
        /// <param name="index">应插入 Line 的从零开始的索引</param>
        /// <param name="newLine">要添加的Line</param>
        public void InsertLine(int index, Line newLine)
        {
            Assemblage.Insert(index, newLine);
        }
        /// <summary>
        /// 将指定Line的元素添加指定索引处
        /// </summary>
        /// <param name="index">应插入 Line 的从零开始的索引</param>
        /// <param name="newLines">要添加的多个Line</param>
        public void InsertRange(int index, params Line[] newLines)
        {
            Assemblage.InsertRange(index, newLines);
        }
        /// <summary>
        /// 从Assemblage中移除特定对象的第一个匹配项
        /// </summary>
        /// <param name="line">要从Assemblage中删除的Line的名称</param>
        /// <returns>如果成功移除了line，则为 true；否则为 false</returns>
        public bool Remove(Line line)
        {
            return Assemblage.Remove(line);
        }
        /// <summary>
        /// 从Assemblage中移除特定名称的第一个匹配项
        /// </summary>
        /// <param name="lineName">要从Assemblage中删除的Line的名称</param>
        /// <returns>如果成功移除了line，则为 true；否则为 false</returns>
        public bool Remove(string lineName)
        {
            for (int i = 0; i < Assemblage.Count; i++)
            {
                if (Assemblage[i].Name == lineName)
                {
                    Assemblage.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 从Assemblage中移除移除与条件相匹配的所有Line
        /// </summary>
        /// <param name="line">要从Assemblage中删除的Line</param>
        public void RemoveAll(Line line)
        {
            while (Assemblage.Remove(line)) ;
        }
        /// <summary>
        /// 从Assemblage中移除移除与名称相匹配的所有Line
        /// </summary>
        /// <param name="lineName">要从Assemblage中删除的Line的名称</param>
        public void RemoveAll(string lineName)
        {
            for (int i = 0; i < Assemblage.Count; i++)
            {
                if (Assemblage[i].Name == lineName)
                {
                    Assemblage.RemoveAt(i--);
                }
            }
        }
        /// <summary>
        /// 移除Assemblage的指定索引处的Line
        /// </summary>
        /// <param name="index">要移除的Line的从零开始的索引</param>
        public void RemoveAt(int index)
        {
            Assemblage.RemoveAt(index);
        }

        /// <summary>
        /// 确定某Line是否在Assemblage中
        /// </summary>
        /// <param name="line">要在Assemblage中定位的Line</param>
        /// <returns>如果在Assemblage中找到line，则为True；否则为false </returns>
        public bool Contains(Line line)
        {
            return Assemblage.Contains(line);
        }
        /// <summary>
        /// 确定某Sub是否在Assemblage中
        /// </summary>
        /// <param name="sub">要在Assemblage中定位的Sub</param>
        /// <returns>如果在Assemblage中找到line，则为True；否则为false</returns>
        public bool Contains(Sub sub)
        {
            foreach (Line li in Assemblage)
                if (li.Contains(sub))
                    return true;
            return false;
        }
        /// <summary>
        /// 返回一个值，该值指示指定的字段是否出现在Assemblage的line的名字。
        /// </summary>
        /// <param name="value">字段</param>
        /// <returns>如果在Assemblage的line中找到与value相似的名字，则为True；否则为false</returns>
        public bool ContainsLine(string value)
        {
            return Assemblage.FirstOrDefault(x => x.Name.Contains(value)) != null;
        }
        /// <summary>
        /// 返回一个值，该值指示指定的字段是否出现在Assemblage的Line或里面的Sub的名字
        /// </summary>
        /// <param name="value">字段</param>
        /// <returns>如果在Assemblage的line和line里面的sub中找到相似的名字，则为True；否则为false</returns>
        public bool ContainsSub(string value)
        {
            if (Assemblage.FirstOrDefault(x => x.Name.Contains(value)) != null)
                return true;
            return Assemblage.FirstOrDefault(x => x.Contains(value)) != null;
        }
        /// <summary>
        /// 确定某Line(名字定位)是否在Assemblage中
        /// </summary>
        /// <param name="lineName">Line的名字</param>
        /// <returns>如果在Assemblage中找到相同的名字，则为True；否则为false</returns>
        public bool HaveLine(string lineName)
        {
            return (Assemblage.FirstOrDefault(x => x.Name.Contains(lineName)) != null);
        }
        /// <summary>
        /// 确定某sub(名字定位)是否在Assemblage中
        /// </summary>
        /// <param name="subName">sub的名字</param>
        /// <returns>如果在Assemblage的sub中找到相同的名字，则为True；否则为false</returns>
        public bool HaveSub(string subName)
        {
            if (Assemblage.FirstOrDefault(x => x.Name == subName) != null)
                return true;
            return Assemblage.FirstOrDefault(x => x.Have(subName)) != null;
        }


        /// <summary>
        /// 匹配拥有相同名称的Line的所有元素
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的Line，其中所有元素均与指定谓词定义的条件匹配，则为该数组；否则为一个空的Array</returns>
        public Line[] FindAllLine(string lineName)
        {
            List<Line> lines = new List<Line>();
            foreach (Line li in Assemblage)
                if (li.Name == lineName)
                    lines.Add(li);
            return lines.ToArray();
        }
        /// <summary>
        /// 搜索与指定名称，并返回整个Assemblage中的第一个匹配元素
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的第一个Line，则为该Line；否则为null</returns>
        public Line FindLine(string lineName)
        {
            return Assemblage.FirstOrDefault(x => x.Name == lineName);
        }
        /// <summary>
        /// 匹配拥有相同名称的Sub的所有元素
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的Sub，其中所有元素均与指定谓词定义的条件匹配，则为该数组；否则为一个空的Array</returns>
        public Sub[] FindAllSub(string subName)
        {
            List<Sub> lines = new List<Sub>();
            foreach (Line li in Assemblage)
            {
                lines.AddRange(li.FindAll(subName));
            }
            return lines.ToArray();
        }
        /// <summary>
        /// 搜索与指定名称，并返回整个Assemblage中的第一个匹配元素
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的第一个Sub，则为该Line；否则为null</returns>
        public Sub FindSub(string subName)
        {//ToDO:给全部find单个的sub进行优化(不使用have)
            foreach (Line li in Assemblage)
            {
                if (li.Name == subName)
                    return li;
                if (li.Have(subName))
                    return li.Find(subName);
            }
            return null;
        }

        /// <summary>
        /// 搜索全部相似名称的Line的所有元素
        /// </summary>
        /// <param name="value">字段</param>
        /// <returns>如果找到相似名称的Line,则为数组；否则为一个空的Array</returns>
        public Line[] SearchAllLine(string value)
        {
            List<Line> lines = new List<Line>();
            foreach (Line li in Assemblage)
                if (li.Name.Contains(value))
                    lines.Add(li);
            return lines.ToArray();
        }
        /// <summary>
        /// 搜索字段是否出现在Line名称，并返回整个Assemblage中的第一个匹配元素
        /// </summary>
        /// <param name="value">字段</param>
        /// <returns>如果找到相似名称的第一个Line，则为该Line；否则为null</returns>
        public Line SearchLine(string value)
        {
            return Assemblage.FirstOrDefault(x => x.Name.Contains(value));
        }
        /// <summary>
        /// 搜索全部相似名称的Sub的所有元素
        /// </summary>
        /// <param name="value">字段</param>
        /// <returns>如果找到相似名称的Line,则为该数组；否则为一个空的Array</returns>
        public Sub[] SearchAllSub(string value)
        {
            List<Sub> lines = new List<Sub>();
            foreach (Line li in Assemblage)
            {                
                lines.AddRange(li.SeachALL(value));
            }
            return lines.ToArray();
        }
        /// <summary>
        /// 搜索字段是否出现在Sub名称，并返回整个Assemblage中的第一个匹配元素
        /// </summary>
        /// <param name="value">字段</param>
        /// <returns>如果找到相同名称的第一个Sub，则为该Sub；否则为null</returns>
        public Sub SearchSub(string value)
        {//ToDO:给全部find单个的sub进行优化(不使用Contains)
            foreach (Line li in Assemblage)
            {
                if (li.Name.Contains(value))
                    return li;
                if (li.Contains(value))
                    return li.Seach(value);
            }
            return null;
        }

        /// <summary>
        /// 搜索相同名称的Line，并返回整个Assemblage中第一个匹配的Line从零开始的索引
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的Line的第一个元素，则为该元素的从零开始的索引；否则为 -1</returns>
        public int IndexOf(string lineName)
        {
            for (int i = 0; i < Assemblage.Count; i++)
            {
                if (Assemblage[i].Name == lineName)
                    return i;
            }
            return -1;
        }
        /// <summary>
        /// 搜索相同名称的Line，并返回整个Assemblage中全部匹配的Line从零开始的索引
        /// </summary>
        /// <param name="lineName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的Line的元素，则为该元素的从零开始的索引组；否则为空的Array</returns>
        public int[] IndexsOf(string lineName)
        {
            List<int> lines = new List<int>();
            for (int i = 0; i < Assemblage.Count; i++)
            {
                if (Assemblage[i].Name == lineName)
                    lines.Add(i);
            }
            return lines.ToArray();
        }

        #endregion

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
        /// <returns>如何有则返回当前Line，如果没有则返回null</returns>
        public Line ReadNext()
        {
            if (LineNode == Assemblage.Count)
                return null;
            return Assemblage[LineNode++];
        }
        /// <summary>
        /// 获取读取进度当前Line
        /// </summary>
        /// <returns>如何有则返回当前Line，如果没有则返回null</returns>
        public Line Read()
        {
            if (LineNode == Assemblage.Count)
                return null;
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
            InsertLine(LineNode + 1, new Line(newlineName, info, text, subs));
        }
        /// <summary>
        /// 将指定的Sub添加到当前读取进度Line中
        /// </summary>
        /// <param name="newSub">要添加的子类</param>
        public void AppendSub(params Sub[] newSub)
        {
            Read().AddRange(newSub);
        }
        /// <summary>
        /// 将指定的Sub添加到当前读取进度Line中
        /// </summary>
        /// <param name="newSubName">要添加的行名称</param>
        /// <param name="info">要添加的行信息</param>
        public void AppendSub(string newSubName, string info = "")
        {
            Read().AddSub(new Sub(newSubName, info));
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
        /// <returns>如果还有下一行，返回True，否则False</returns>
        public bool ReadCanNext()
        {
            return LineNode != Assemblage.Count;
        }

        /// <summary>
        /// 获取最后一行的信息
        /// </summary>
        /// <returns>最后一行的信息</returns>
        public Line LastLine()
        {
            return Assemblage.Last();
        }

        #endregion





        /// <summary>
        /// 从指定的字符串加载LPS文档
        /// </summary>
        /// <param name="lps">包含要加载的LPS文档的字符串</param>
        /// <returns></returns>
        public bool Load(string lps)
        {
            Assemblage.Clear();//清空当前文档
            try
            {
                string[] file = lps.Replace("\r", "").Trim('\n').Split('\n');
                foreach (string str in file)
                {
                    if (str != "")
                        Assemblage.Add(new Line(str));
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 返回一个Assemblage的第一个元素。
        /// </summary>
        /// <returns>要返回的第一个元素</returns>
        public Line First()
        {
            if (Assemblage.Count == 0)
                return null;
            return Assemblage[0];
        }
        /// <summary>
        /// 返回一个Assemblage的最后一个元素。
        /// </summary>
        /// <returns>要返回的最后一个元素</returns>
        public Line Last()
        {
            if (Assemblage.Count == 0)
                return null;
            return Assemblage[Assemblage.Count - 1];
        }
        /// <summary>
        /// 返回循环访问 Assemblage 的枚举数。
        /// </summary>
        /// <returns>用于 Assemblage 的枚举数</returns>
        public IEnumerator<Line> GetEnumerator()
        {
            return Assemblage.GetEnumerator();
        }

        /// <summary>
        /// 将当前Documents转换成文本格式
        /// </summary>
        /// <returns>LinePutScript的文本格式</returns>
        public new string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Line li in Assemblage)
                sb.Append(li.ToString() + "\n");
            return sb.ToString().Trim('\n');
        }
    }

}
