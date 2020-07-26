using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LinePutScript
{
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
            string[] sts = Regex.Split(lpsLine, @"\:\|", RegexOptions.IgnoreCase);

            string[] st = sts[0].Split(new char[1] { '#' }, 2);//第一个
            Name = st[0];
            if (st.Length > 1)
                info = st[1];//lpstext都是转义后(无关键字)

            text = sts.Last();//最后一个

            for (int i = 1; i < sts.Length - 1; i++)
            {
                Subs.Add(new Sub(sts[i]));
            }
        }
        /// <summary>
        /// 通过名字和信息创建新的Line
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="info">信息 (正常)</param>
        /// <param name="text">文本 在末尾没有结束行号的文本 (正常)</param>
        /// <param name="subs">子类集合</param>
        public Line(string name, string info, string text = "", params Sub[] subs)
        {
            Name = name;
            Info = info;
            Text = text;
            Subs.AddRange(subs);
        }
        /// <summary>
        /// 通过其他Line创建新的Line
        /// </summary>
        /// <param name="line">其他line</param>
        public Line(Line line)
        {
            Name = line.Name;
            info = line.info;

            text = line.text;
            Subs = line.Subs.ToList();
        }
        /// <summary>
        /// 将其他Line内容拷贝到本Line
        /// </summary>
        /// <param name="line">其他line</param>
        public void Set(Line line)
        {
            Name = line.Name;
            info = line.info;

            text = line.text;
            Subs = line.Subs.ToList();
        }


        /// <summary>
        /// 文本 在末尾没有结束行号的文本 (去除关键字的文本)
        /// </summary>
        public string text = "";
        /// <summary>
        /// 文本 在末尾没有结束行号的文本 (正常)
        /// </summary>
        public string Text
        {
            get => TextDeReplace(text);
            set
            {
                text = TextReplace(value);
            }
        }
        /// <summary>
        /// 文本 (int)
        /// </summary>
        public int TextToInt
        {
            get
            {
                int.TryParse(text, out int i);
                return i;
            }
            set
            {
                text = value.ToString();
            }
        }
        /// <summary>
        /// 文本 (int64)
        /// </summary>
        public long TextToInt64
        {
            get
            {
                long.TryParse(text, out long i);
                return i;
            }
            set
            {
                text = value.ToString();
            }
        }
        /// <summary>
        /// 文本 (double)
        /// </summary>
        public double TextToDouble
        {
            get
            {
                double.TryParse(text, out double i);
                return i;
            }
            set
            {
                info = value.ToString();
            }
        }
        /// <summary>
        /// 退回Text的反转义文本 (正常显示)
        /// </summary>
        /// <returns>Text的反转义文本 (正常显示)</returns>
        public string GetText()
        {
            return Text;
        }
        /// <summary>
        /// 子项目
        /// </summary>
        public List<Sub> Subs = new List<Sub>();



        #region List操作
        /// <summary>
        /// 将指定的Sub添加到Subs列表的末尾
        /// </summary>
        /// <param name="newSub">要添加的Sub</param>
        public void AddSub(Sub newSub)
        {
            Subs.Add(newSub);
        }
        /// <summary>
        /// 若无相同名称(Name)的Sub,则将指定的Sub添加到Subs列表的末尾
        /// 若有,则替换成要添加的Sub
        /// </summary>
        /// <param name="newSub">要添加的Sub</param>
        public void AddorReplaceSub(Sub newSub)
        {
            Sub oldsub = Find(newSub.Name);
            if (oldsub != null)
            {
                oldsub.Set(newSub);
            }
            else
                Subs.Add(newSub);
        }
        /// <summary>
        /// 将指定Sub的元素添加到Subs的末尾
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
        /// <returns>如果成功移除了Sub,则为 true；否则为 false</returns>
        public bool Remove(Sub Sub)
        {
            return Subs.Remove(Sub);
        }
        /// <summary>
        /// 从Subs中移除特定名称的第一个匹配项
        /// </summary>
        /// <param name="SubName">要从Subs中删除的Sub的名称</param>
        /// <returns>如果成功移除了Sub,则为 true；否则为 false</returns>
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
        /// 从Subs中移除特定名称的所有元素
        /// </summary>
        /// <param name="SubName">要从Subs中删除的Sub的名称</param>
        public void RemoveAll(string SubName)
        {
            for (int i = 0; i < Subs.Count; i++)
            {
                if (Subs[i].Name == SubName)
                {
                    Subs.RemoveAt(i--);
                }
            }
        }
        /// <summary>
        /// 确定某Sub是否在Line集合中
        /// </summary>
        /// <param name="sub">要在Line集合中定位的Sub</param>
        /// <returns>如果在Line集合中找到sub,则为True；否则为false</returns>
        public bool Contains(Sub sub)
        {
            if (this == sub)
                return true;
            return Subs.Contains(sub);
        }
        /// <summary>
        /// 返回一个值,该值指示指定的字段是否出现在Subs的Sub的名字
        /// </summary>
        /// <param name="value">字段</param>
        /// <returns>如果在Line集合中找到符合的名字,则为True；否则为false</returns>
        public bool Contains(string value)
        {
            if (Name.Contains(value))
                return true;
            return (Subs.FirstOrDefault(x => x.Name.Contains(value)) != null);
        }
        /// <summary>
        /// 确定某Sub是否在Line集合中
        /// </summary>
        /// <param name="subName">要在Line集合中定位的Sub的名字</param>
        /// <returns>如果在Line集合中找到符合的名字,则为True；否则为false</returns>
        public bool Have(string subName)
        {
            if (Name == subName)
                return true;
            return (Subs.FirstOrDefault(x => x.Name == subName) != null);
        }


        /// <summary>
        /// 匹配拥有相同名称的Line或sub的所有元素
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的sub,其中所有元素均与指定谓词定义的条件匹配,则为该数组；否则为一个空的Array</returns>
        public Sub[] FindAll(string subName)
        {
            List<Sub> subs = new List<Sub>();
            if (Name == subName)
                subs.Add(this);
            foreach (Sub su in Subs)
                if (su.Name == subName)
                    subs.Add(su);
            return subs.ToArray();
        }
        /// <summary>
        /// 搜索与指定名称,并返回Line或整个Subs中的第一个匹配元素
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的第一个sub,则为该sub；否则为null</returns>
        public Sub Find(string subName)
        {
            if (this.Name == subName)
                return this;
            return Subs.FirstOrDefault(x => x.Name == subName);
        }

        /// <summary>
        /// 搜索与指定名称,并返回Line或整个Subs中的第一个匹配元素;若未找到,则新建并添加相同名称的Sub,并且返回这个Sub
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的第一个sub,则为该sub；否则为新建的相同名称sub</returns>
        public Sub FindorAdd(string subName)
        {
            Sub sub = Find(subName);
            if (sub == null)
            {
                sub = new Line(subName, "", "");
                AddSub(sub);
                return sub;
            }
            else
            {
                return sub;
            }
        }


        /// <summary>
        /// 搜索全部相似名称的Sub的所有元素
        /// </summary>
        /// <param name="value">字段</param>
        /// <returns>如果找到相似名称的Sub,则为数组；否则为一个空的Array</returns>
        public Sub[] SeachALL(string value)
        {
            List<Sub> subs = new List<Sub>();
            if (Name.Contains(value))
                subs.Add(this);
            foreach (Sub su in Subs)
                if (su.Name.Contains(value))
                    subs.Add(su);
            return subs.ToArray();
        }
        /// <summary>
        /// 搜索字段是否出现在Line名称,并返回整个Subs中的第一个匹配元素
        /// </summary>
        /// <param name="value">字段</param>
        /// <returns>如果找到相似名称的第一个Sub,则为该Sub；否则为null</returns>
        public Sub Seach(string value)
        {
            if (this.Name.Contains(value))
                return this;
            return Subs.FirstOrDefault(x => x.Name.Contains(value));
        }



        /// <summary>
        /// 搜索相同名称的Sub,并返回整个Subs中第一个匹配的sub从零开始的索引
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的sub的第一个元素,则为该元素的从零开始的索引；否则为 -1</returns>
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
        /// 搜索相同名称的Sub,并返回整个Sub中全部匹配的sub从零开始的索引
        /// </summary>
        /// <param name="subName">用于定义匹配的名称</param>
        /// <returns>如果找到相同名称的sub的元素,则为该元素的从零开始的索引组；否则为空的Array</returns>
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
        /// 将当前Line转换成文本格式 (info已经被转义/去除关键字)
        /// </summary>
        /// <returns>Line的文本格式 (info已经被转义/去除关键字)</returns>
        public new string ToString()//不能继承
        {
            StringBuilder str = new StringBuilder(TextReplace(Name));
            if (info != "")
                str.Append('#' + info);
            str.Append(":|");
            foreach (Sub su in Subs)
                str.Append(su.ToString());
            str.Append(text);
            return str.ToString();
        }

        /// <summary>
        /// 返回循环访问 Subs 的枚举数。
        /// </summary>
        /// <returns>用于 Subs 的枚举数</returns>
        public new IEnumerator<Sub> GetEnumerator()
        {
            return Subs.GetEnumerator();
        }
        /// <summary>
        /// 返回一个 Subs 的第一个元素。
        /// </summary>
        /// <returns>要返回的第一个元素</returns>
        public new Sub First()
        {
            if (Subs.Count == 0)
                return null;
            return Subs[0];
        }
        /// <summary>
        /// 返回一个 Subs 的最后一个元素。
        /// </summary>
        /// <returns>要返回的最后一个元素</returns>
        public new Sub Last()
        {
            if (Subs.Count == 0)
                return null;
            return Subs[Subs.Count - 1];
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
}
