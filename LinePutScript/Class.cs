using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LinePutScript
{
    /// <summary>
    /// 子类 LinePutScript最基础的类
    /// </summary>
    public class Sub
    {
        /// <summary>
        /// 创建一个子类
        /// </summary>
        public Sub() { }
        /// <summary>
        /// 通过lpsSub文本创建一个子类
        /// </summary>
        /// <param name="lpsSub">lpsSub文本</param>
        public Sub(string lpsSub)
        {
            string[] st = lpsSub.Split('#');
            Name = st[0];
            if (st.Length > 1)
                Info = st[1];
        }
        /// <summary>
        /// 将其他Sub内容拷贝到本Sub
        /// </summary>
        /// <param name="sub">其他Sub</param>
        public void Set(Sub sub)
        {
            Name = sub.Name;
            Info = sub.Info;            
        }

        /// <summary>
        /// 名称 没有唯一性
        /// </summary>
        public string Name;
        /// <summary>
        /// 信息
        /// </summary>
        public string Info = "";
        ////暂时应该不需要判断类型
        ///// <summary>
        ///// 获取当前标签的类型
        ///// </summary>
        ///// <returns>类型</returns>
        //public new virtual string GetType()
        //{
        //    return "sub";
        //}
        /// <summary>
        /// 退回Info的转义文本
        /// </summary>
        /// <returns>info的转义文本</returns>
        public string GetInfo()
        {
            return TextReplace(Info);
        }
        /// <summary>
        /// 退回Info集合的转义文本
        /// </summary>
        /// <returns>info的转义文本</returns>
        public string[] GetInfos()
        {
            string[] sts = Info.Split(',');
            for (int i = 0; i < sts.Length; i++)
                sts[i] = TextReplace(sts[i]);
            return sts;
        }

        /// <summary>
        /// 将文本进行转义处理
        /// </summary>
        /// <param name="Reptex">要转义的文本</param>
        /// <returns>转义后的文本</returns>
        public static string TextReplace(string Reptex)
        {
            Reptex = Reptex.Replace("/|", ":|");
            Reptex = Reptex.Replace("/tab", "\t");
            Reptex = Reptex.Replace("/n", "\n");
            Reptex = Reptex.Replace("/id", "#");
            Reptex = Reptex.Replace("/?", "//");
            Reptex = Reptex.Replace("/!", "/");
            return Reptex;
        }
        /// <summary>
        /// 将文本进行反转义处理
        /// </summary>
        /// <param name="Reptex">要反转义的文本</param>
        /// <returns>反转义后的文本</returns>
        public static string TextDeReplace(string Reptex)
        {
            Reptex = Reptex.Replace(":|", "/|");
            Reptex = Reptex.Replace("\t", "/tab");
            Reptex = Reptex.Replace("\n", "/n");
            Reptex = Reptex.Replace("#", "/id");
            Reptex = Reptex.Replace("//", "/?");
            Reptex = Reptex.Replace("/", "/!");
            return Reptex;
        }
        /// <summary>
        /// 将当前Sub转换成文本格式
        /// </summary>
        /// <returns>Sub的文本格式</returns>
        public new string ToString()//不能继承
        {
            return TextDeReplace(Name) + '#' + TextDeReplace(Info) + ":|";
        }


    }
    /// <summary>
    /// 行 包含多个子类 继承自子类
    /// </summary>
    public class Line : Sub
    {
        /// <summary>
        /// 创建一行
        /// </summary>
        public Line() { }
        /// <summary>
        /// 通过lpsLine文本创建一行
        /// </summary>
        /// <param name="lpsLine">lpsSub文本</param>
        public Line(string lpsLine)
        {
            string[] sts = Regex.Split(Info, ":|", RegexOptions.IgnoreCase);

            string[] st = sts[0].Split('#');//第一个
            Name = st[0];
            if (st.Length > 1)
                Info = st[1];

            Text = sts.Last();//最后一个

            for(int i = 1; i < sts.Length - 1; i++)
            {
                Subs.Add(new Sub(sts[i]));
            }
        }
        /// <summary>
        /// 将其他Line内容拷贝到本Line
        /// </summary>
        /// <param name="line">其他line</param>
        public void Set(Line line)
        {
            Name = line.Name;
            Info = line.Info;

            Text = line.Text;
            Subs = line.Subs.ToList();
        }


        /// <summary>
        /// 文本 在末尾没有结束行号的文本
        /// </summary>
        public string Text = "";
        /// <summary>
        /// 退回Text的转义文本
        /// </summary>
        /// <returns>Text的转义文本</returns>
        public string GetText()
        {
            return TextReplace(Text);
        }
        /// <summary>
        /// 子项目
        /// </summary>
        public List<Sub> Subs = new List<Sub>();



        #region List操作
        /// <summary>
        /// 将指定的Sub添加到Subs列表的末尾。
        /// </summary>
        /// <param name="newSub">要添加的Sub</param>
        public void AddSub(Sub newSub)
        {
            Subs.Add(newSub);
        }
        /// <summary>
        /// 将指定Sub的元素添加到Subs的末尾。
        /// </summary>
        /// <param name="newSubs">要添加的多个Sub</param>
        public void AddRange(params Sub[] newSubs)
        {
            Subs.AddRange(newSubs);
        }

        /// <summary>
        /// 将指定的Sub添加到指定索引处
        /// </summary>
        /// <param name="index">应插入 Sub 的从零开始的索引</param>
        /// <param name="newSub">要添加的Sub</param>
        public void InsertSub(int index, Sub newSub)
        {
            Subs.Insert(index, newSub);
        }
        /// <summary>
        /// 将指定Sub的元素添加指定索引处
        /// </summary>
        /// <param name="index">应插入 Sub 的从零开始的索引</param>
        /// <param name="newSubs">要添加的多个Sub</param>
        public void InsertRange(int index, params Sub[] newSubs)
        {
            Subs.InsertRange(index, newSubs);
        }
        /// <summary>
        /// 从Subs中移除特定对象的第一个匹配项
        /// </summary>
        /// <param name="Sub">要从Subs中删除的Sub的名称</param>
        /// <returns>如果成功移除了Sub，则为 true；否则为 false</returns>
        public bool Remove(Sub Sub)
        {
            return Subs.Remove(Sub);
        }
        /// <summary>
        /// 从Subs中移除特定名称的第一个匹配项
        /// </summary>
        /// <param name="SubName">要从Subs中删除的Sub的名称</param>
        /// <returns>如果成功移除了Sub，则为 true；否则为 false</returns>
        public bool Remove(string SubName)
        {
            for (int i = 0; i < Subs.Count; i++)
            {
                if (Subs[i].Name == SubName)
                {
                    Subs.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 确定某Sub是否在Line集合中。
        /// </summary>
        /// <param name="sub">要在Line集合中定位的Sub</param>
        /// <returns>如果在Line集合中找到sub，则为True；否则为false。</returns>
        public bool Contains(Sub sub)
        {
            if (this == sub)
                return true;
            return Subs.Contains(sub);
        }
        /// <summary>
        /// 确定某Sub是否在Line集合中
        /// </summary>
        /// <param name="subName">要在Line集合中定位的Sub的名字</param>
        /// <returns>如果在Line集合中找到符合的名字，则为True；否则为false。</returns>
        public bool Contains(string subName)
        {
            if (Name == subName)
                return true;
            return (Subs.FirstOrDefault(x => x.Name == subName) != null);
        }
        /// <summary>
        /// 匹配拥有相同名称的sub的所有元素。
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的sub，其中所有元素均与指定谓词定义的条件匹配，则为该数组；否则为一个空的Array</returns>
        public Sub[] FindAll(string subName)
        {
            List<Sub> subs = new List<Sub>();
            foreach (Sub su in Subs)
                if (su.Name == subName)
                    subs.Add(su);
            return subs.ToArray();
        }
        /// <summary>
        /// 搜索与指定名称，并返回整个Subs中的第一个匹配元素。
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的第一个sub，则为该sub；否则为null。</returns>
        public Sub Find(string subName)
        {
            return Subs.FirstOrDefault(x => x.Name == subName);
        }

        /// <summary>
        /// 搜索相同名称的Sub，并返回整个Subs中第一个匹配的sub从零开始的索引。
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的sub的第一个元素，则为该元素的从零开始的索引；否则为 -1</returns>
        public int IndexOf(string subName)
        {
            for (int i = 0; i < Subs.Count; i++)
            {
                if (Subs[i].Name == subName)
                    return i;
            }
            return -1;
        }
        /// <summary>
        /// 搜索相同名称的Sub，并返回整个Sub中全部匹配的sub从零开始的索引。
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的sub的元素，则为该元素的从零开始的索引组；否则为空的Array</returns>
        public int[] IndexsOf(string subName)
        {
            List<int> lines = new List<int>();
            for (int i = 0; i < Subs.Count; i++)
            {
                if (Subs[i].Name == subName)
                    lines.Add(i);
            }
            return lines.ToArray();
        }
        #endregion

        /// <summary>
        /// 将当前Line转换成文本格式
        /// </summary>
        /// <returns>Line的文本格式</returns>
        public new string ToString()//不能继承
        {
            StringBuilder str = new StringBuilder(TextDeReplace(Name) + '#' + TextDeReplace(Info) + ":|");
            foreach (Sub su in Subs)
                str.Append(su.ToString());
            str.Append(TextDeReplace(Text));
            return str.ToString();
        }


        ////暂时应该不需要判断类型
        ///// <summary>
        ///// 获取当前标签的类型
        ///// </summary>
        ///// <returns>类型</returns>
        //public override string GetType()
        //{
        //    return "line";
        //}
    }

    ////目前不清楚是否需要组类别 用户可以自行设定，没必要限制用户想象力
    ///// <summary>
    ///// 组 包含多个Line 继承自子类
    ///// </summary>
    //public class Group : Sub
    //{
    //    /// <summary>
    //    /// 文本 在末尾没有结束Line号的文本
    //    /// </summary>
    //    public string Text = "";
    //    /// <summary>
    //    /// 子Line
    //    /// </summary>
    //    public List<Line> Lines = new List<Line>();
    //    /// <summary>
    //    /// 获取当前标签的类型
    //    /// </summary>
    //    /// <returns>类型</returns>
    //    public override string GetType()
    //    {
    //        return "group";
    //    }
    //}  
}
