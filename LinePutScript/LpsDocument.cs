﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// 确定某元素是否在Assemblage中
        /// </summary>
        /// <param name="sub">要在Assemblage中定位的sub</param>
        /// <returns>如果在Assemblage中找到line，则为True；否则为false</returns>
        public bool Contains(Sub sub)
        {
            foreach (Line li in Assemblage)
                if (li.Contains(sub))
                    return true;
            return false;
        }
        /// <summary>
        /// 确定某Line(名字定位)是否在Assemblage中
        /// </summary>
        /// <param name="lineName">Line的名字</param>
        /// <returns>如果在Assemblage中找到符合的名字，则为True；否则为false</returns>
        public bool ContainsLine(string lineName)
        {
            return (Assemblage.FirstOrDefault(x => x.Name == lineName) != null);
        }
        /// <summary>
        /// 确定某sub(名字定位)是否在Assemblage中
        /// </summary>
        /// <param name="subName">sub的名字</param>
        /// <returns>如果在Assemblage的sub中找到符合的名字，则为True；否则为false</returns>
        public bool ContainsSub(string subName)
        {
            if (Assemblage.FirstOrDefault(x => x.Name == subName) != null)
                return true;
            return Assemblage.FirstOrDefault(x => x.Contains(subName)) != null;
            //foreach (Line li in Assemblage)
            //    if (li.Contains(subName))
            //        return true;
            //return false;
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
                if (li.Name == subName)
                    lines.Add(li);
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
        {
            foreach (Line li in Assemblage)
            {
                if (li.Name == subName)
                    return li;
                if (li.Contains(subName))
                    return li.Find(subName);
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
        /// 读取当前Line 并且自动切换到下一Line
        /// </summary>
        /// <returns>当前Line</returns>
        public Line ReadNext()
        {
            return Assemblage[LineNode++];
        }
        /// <summary>
        /// 获取当前Line
        /// </summary>
        /// <returns>当前Line</returns>
        public Line Read()
        {
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
        /// 将读取进度设置为0
        /// </summary>
        public void ReadReset()
        {
            LineNode = 0;
        }
        /// <summary>
        /// 将读取进度设置为上限
        /// </summary>
        public void ReadEnd()
        {
            LineNode = Assemblage.Count;
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
                string[] file = lps.Replace("\r", "").Split('\n');
                foreach (string str in file)
                {
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
        /// 将当前Line转换成文本格式
        /// </summary>
        /// <returns>Line的文本格式</returns>
        public new string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Line li in Assemblage)
                sb.Append(li.ToString());
            return sb.ToString();
        }
    }

}