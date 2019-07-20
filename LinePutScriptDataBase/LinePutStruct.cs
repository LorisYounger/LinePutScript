using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LinePutScript.DataBase
{
    //全部废弃:指定的类型必须是不包含引用的结构
    ///// <summary>
    ///// 由LpsDocument转换而来
    ///// </summary>
    //public struct LinePutStruct
    //{


    //}
    //public class LineStructReader
    //{
    //    /// <summary>
    //    /// 通过lpsLine文本创建一行
    //    /// </summary>
    //    /// <param name="lpsLine">lpsSub文本</param>
    //    public LineStructReader(string lpsLine)
    //    {
    //        this.info = "";
    //        this.subs = "";

    //        string[] sts = Regex.Split(lpsLine, @"\:\|", RegexOptions.IgnoreCase);

    //        string[] st = sts[0].Split(new char[1] { '#' }, 2);//第一个
    //        Name = st[0];
    //        if (st.Length > 1)
    //            info = st[1];//lpstext都是转义后(无关键字)

    //        text = sts.Last();//最后一个

    //        for (int i = 1; i < sts.Length - 1; i++)
    //        {
    //            subs += sts[i];
    //        }

    //    }
    //    /// <summary>
    //    /// 通过名字和信息创建新的Line
    //    /// </summary>
    //    /// <param name="name">名称</param>
    //    /// <param name="info">信息 (正常)</param>
    //    /// <param name="text">文本 在末尾没有结束行号的文本 (正常)</param>
    //    /// <param name="subs">子类集合</param>
    //    public LineStructReader(string name, string info, string text = "", params Sub[] subs)
    //    {
    //        Name = name;
    //        this.info = "";
    //        this.text = "";
    //        this.subs = "";
    //        Info = info;
    //        Text = text;
    //        Subs = subs.ToList();
    //    }
    //    /// <summary>
    //    /// 通过其他Line创建新的Line
    //    /// </summary>
    //    /// <param name="line">其他line</param>
    //    public LineStructReader(Line line)
    //    {
    //        this.subs = "";
    //        Name = line.Name;
    //        info = line.info;

    //        text = line.text;
    //        Subs = line.Subs.ToList();
    //    }
    //    /// <summary>
    //    /// 将其他Line内容拷贝到本Line
    //    /// </summary>
    //    /// <param name="line">其他line</param>
    //    public void Set(Line line)
    //    {
    //        Name = line.Name;
    //        info = line.info;

    //        text = line.text;
    //        Subs = line.Subs.ToList();
    //    }

    //    /// <summary>
    //    /// 名称 没有唯一性
    //    /// </summary>
    //    public string Name;
    //    /// <summary>
    //    /// 信息 (去除关键字的文本)
    //    /// </summary>
    //    public string info;
    //    /// <summary>
    //    /// 信息 (正常)
    //    /// </summary>
    //    public string Info
    //    {
    //        get => TextDeReplace(info);
    //        set
    //        {
    //            info = TextReplace(value);
    //        }
    //    }
    //    /// <summary>
    //    /// 信息 (int)
    //    /// </summary>
    //    public int InfoToInt
    //    {
    //        get
    //        {
    //            int.TryParse(info, out int i);
    //            return i;
    //        }
    //        set
    //        {
    //            info = value.ToString();
    //        }
    //    }
    //    /// <summary>
    //    /// 信息 (int64)
    //    /// </summary>
    //    public long InfoToInt64
    //    {
    //        get
    //        {
    //            long.TryParse(info, out long i);
    //            return i;
    //        }
    //        set
    //        {
    //            info = value.ToString();
    //        }
    //    }
    //    /// <summary>
    //    /// 信息 (double)
    //    /// </summary>
    //    public double InfoToDouble
    //    {
    //        get
    //        {
    //            double.TryParse(info, out double i);
    //            return i;
    //        }
    //        set
    //        {
    //            info = value.ToString();
    //        }
    //    }

    //    public string GetInfo()
    //    {
    //        return Info;
    //    }
    //    /// <summary>
    //    /// 退回Info集合的转义文本集合 (正常显示)
    //    /// </summary>
    //    /// <returns>info的转义文本集合 (正常显示)</returns>
    //    public string[] GetInfos()
    //    {
    //        string[] sts = info.Split(',');
    //        for (int i = 0; i < sts.Length; i++)
    //            sts[i] = TextDeReplace(sts[i]);
    //        return sts;
    //    }

    //    /// <summary>
    //    /// 将文本进行反转义处理(正常显示的文本)
    //    /// </summary>
    //    /// <param name="Reptex">要反转义的文本</param>
    //    /// <returns>反转义后的文本 正常显示的文本</returns>
    //    public static string TextDeReplace(string Reptex)
    //    {
    //        Reptex = Reptex.Replace("/stop", ":|");
    //        Reptex = Reptex.Replace("/tab", "\t");
    //        Reptex = Reptex.Replace("/n", "\n");
    //        Reptex = Reptex.Replace("/r", "\r");
    //        Reptex = Reptex.Replace("/id", "#");
    //        Reptex = Reptex.Replace("/!", "/");
    //        Reptex = Reptex.Replace("/com", ",");
    //        return Reptex;
    //    }
    //    /// <summary>
    //    /// 将文本进行转义处理(去除关键字的文本)
    //    /// </summary>
    //    /// <param name="Reptex">要转义的文本</param>
    //    /// <returns>转义后的文本 (去除关键字的文本)</returns>
    //    public static string TextReplace(string Reptex)
    //    {
    //        Reptex = Reptex.Replace(":|", "/stop");
    //        Reptex = Reptex.Replace("\t", "/tab");
    //        Reptex = Reptex.Replace("\n", "/n");
    //        Reptex = Reptex.Replace("\r", "/r");
    //        Reptex = Reptex.Replace("#", "/id");
    //        Reptex = Reptex.Replace(",", "/com");
    //        return Reptex;
    //    }



    //    /// <summary>
    //    /// 文本 在末尾没有结束行号的文本 (去除关键字的文本)
    //    /// </summary>
    //    public string text;
    //    /// <summary>
    //    /// 文本 在末尾没有结束行号的文本 (正常)
    //    /// </summary>
    //    public string Text
    //    {
    //        get => TextDeReplace(text);
    //        set
    //        {
    //            text = TextReplace(value);
    //        }
    //    }
    //    /// <summary>
    //    /// 文本 (int)
    //    /// </summary>
    //    public int TextToInt
    //    {
    //        get => Convert.ToInt32(text);
    //        set
    //        {
    //            info = value.ToString();
    //        }
    //    }
    //    /// <summary>
    //    /// 文本 (int64)
    //    /// </summary>
    //    public long TextToInt64
    //    {
    //        get => Convert.ToInt64(text);
    //        set
    //        {
    //            info = value.ToString();
    //        }
    //    }
    //    /// <summary>
    //    /// 文本 (double)
    //    /// </summary>
    //    public double TextToDouble
    //    {
    //        get => Convert.ToDouble(text);
    //        set
    //        {
    //            info = value.ToString();
    //        }
    //    }
    //    /// <summary>
    //    /// 退回Text的反转义文本 (正常显示)
    //    /// </summary>
    //    /// <returns>Text的反转义文本 (正常显示)</returns>
    //    public string GetText()
    //    {
    //        return Text;
    //    }
    //    /// <summary>
    //    /// 将本Struct转换成Sub !注意:无法直接修改,仅供获取!
    //    /// </summary>
    //    public Sub This
    //    {
    //        get => new Sub(Name, Info);
    //    }

    //    /// <summary>
    //    /// 子项目:源数据
    //    /// </summary>
    //    public string subs;
    //    /// <summary>
    //    /// 子项目
    //    /// </summary>
    //    public List<Sub> Subs
    //    {
    //        get
    //        {
    //            List<Sub> sb = new List<Sub>();
    //            string[] vs = Regex.Split(subs, @"\:\|", RegexOptions.IgnoreCase);
    //            foreach (string str in vs)
    //            {
    //                sb.Add(new Sub(str));
    //            }
    //            return sb;
    //        }
    //        set
    //        {
    //            StringBuilder str = new StringBuilder();
    //            foreach (Sub su in value)
    //                str.Append(su.ToString());
    //            subs = str.ToString();
    //        }

    //    }



    //    #region List操作
    //    /// <summary>
    //    /// 将指定的Sub添加到Subs列表的末尾
    //    /// </summary>
    //    /// <param name="newSub">要添加的Sub</param>
    //    public void AddSub(Sub newSub)
    //    {
    //        subs += newSub.ToString();
    //    }
    //    /// <summary>
    //    /// 将指定Sub的元素添加到Subs的末尾
    //    /// </summary>
    //    /// <param name="newSubs">要添加的多个Sub</param>
    //    public void AddRange(params Sub[] newSubs)
    //    {
    //        foreach (Sub sb in newSubs)
    //        {
    //            subs += sb.ToString();
    //        }
    //    }


    //    /// <summary>
    //    /// 从Subs中移除特定名称的第一个匹配项
    //    /// </summary>
    //    /// <param name="SubName">要从Subs中删除的Sub的名称</param>
    //    /// <returns>如果成功移除了Sub，则为 true；否则为 false</returns>
    //    public bool Remove(string SubName)
    //    {
    //        List<Sub> ls = Subs;
    //        for (int i = 0; i < ls.Count; i++)
    //        {
    //            if (ls[i].Name == SubName)
    //            {
    //                ls.RemoveAt(i);
    //                Subs = ls;
    //                return true;
    //            }
    //        }
    //        return false;
    //    }
    //    /// <summary>
    //    /// 返回一个值，该值指示指定的字段是否出现在Subs的Sub的名字
    //    /// </summary>
    //    /// <param name="value">字段</param>
    //    /// <returns>如果在Line集合中找到符合的名字，则为True；否则为false</returns>
    //    public bool Contains(string value)
    //    {           
    //        if (Name.Contains(value))
    //            return true;
    //        List<Sub> ls = Subs;
    //        return (ls.FirstOrDefault(x => x.Name.Contains(value)) != null);
    //    }
    //    /// <summary>
    //    /// 确定某Sub是否在Line集合中
    //    /// </summary>
    //    /// <param name="subName">要在Line集合中定位的Sub的名字</param>
    //    /// <returns>如果在Line集合中找到符合的名字，则为True；否则为false</returns>
    //    public bool Have(string subName)
    //    {
    //        if (Name == subName)
    //            return true;
    //        List<Sub> ls = Subs;
    //        return (ls.FirstOrDefault(x => x.Name == subName) != null);
    //    }


    //    /// <summary>
    //    /// 匹配拥有相同名称的Line或sub的所有元素 !注意:无法直接修改,仅供获取!
    //    /// </summary>
    //    /// <param name="subName">用于定义匹配的名称</param>
    //    /// <returns>如果找到相同名称的sub，其中所有元素均与指定谓词定义的条件匹配，则为该数组；否则为一个空的Array</returns>
    //    public Sub[] FindAll(string subName)
    //    {
    //        List<Sub> sb = new List<Sub>();

    //        if (Name == subName)
    //            sb.Add(new Sub(Name,Info));
    //        foreach (Sub su in Subs)
    //            if (su.Name == subName)
    //                sb.Add(su);
    //        return sb.ToArray();
    //    }
    //    /// <summary>
    //    /// 搜索与指定名称，并返回Line或整个Subs中的第一个匹配元素 !注意:无法直接修改,仅供获取!
    //    /// </summary>
    //    /// <param name="subName">用于定义匹配的名称</param>
    //    /// <returns>如果找到相同名称的第一个sub，则为该sub；否则为null</returns>
    //    public Sub Find(string subName)
    //    {
    //        if (this.Name == subName)
    //            return This;
    //        return Subs.FirstOrDefault(x => x.Name == subName);
    //    }

    //    /// <summary>
    //    /// 搜索全部相似名称的Sub的所有元素 !注意:无法直接修改,仅供获取!
    //    /// </summary>
    //    /// <param name="value">字段</param>
    //    /// <returns>如果找到相似名称的Sub,则为数组；否则为一个空的Array</returns>
    //    public Sub[] SeachALL(string value)
    //    {
    //        List<Sub> sb = new List<Sub>();
    //        if (Name.Contains(value))
    //            sb.Add(This);
    //        foreach (Sub su in Subs)
    //            if (su.Name.Contains(value))
    //                sb.Add(su);
    //        return sb.ToArray();
    //    }
    //    /// <summary>
    //    /// 搜索字段是否出现在Line名称，并返回整个Subs中的第一个匹配元素 !注意:无法直接修改,仅供获取!
    //    /// </summary>
    //    /// <param name="value">字段</param>
    //    /// <returns>如果找到相似名称的第一个Sub，则为该Sub；否则为null</returns>
    //    public Sub Seach(string value)
    //    {
    //        if (this.Name.Contains(value))
    //            return This;
    //        return Subs.FirstOrDefault(x => x.Name.Contains(value));
    //    }



    //    /// <summary>
    //    /// 搜索相同名称的Sub，并返回整个Subs中第一个匹配的sub从零开始的索引
    //    /// </summary>
    //    /// <param name="subName">用于定义匹配的名称</param>
    //    /// <returns>如果找到相同名称的sub的第一个元素，则为该元素的从零开始的索引；否则为 -1</returns>
    //    public int IndexOf(string subName)
    //    {
    //        for (int i = 0; i < Subs.Count; i++)
    //        {
    //            if (Subs[i].Name == subName)
    //                return i;
    //        }
    //        return -1;
    //    }
    //    /// <summary>
    //    /// 搜索相同名称的Sub，并返回整个Sub中全部匹配的sub从零开始的索引
    //    /// </summary>
    //    /// <param name="subName">用于定义匹配的名称</param>
    //    /// <returns>如果找到相同名称的sub的元素，则为该元素的从零开始的索引组；否则为空的Array</returns>
    //    public int[] IndexsOf(string subName)
    //    {
    //        List<int> lines = new List<int>();
    //        for (int i = 0; i < Subs.Count; i++)
    //        {
    //            if (Subs[i].Name == subName)
    //                lines.Add(i);
    //        }
    //        return lines.ToArray();
    //    }
    //    #endregion

    //    /// <summary>
    //    /// 将当前Line转换成文本格式 (info已经被转义/去除关键字)
    //    /// </summary>
    //    /// <returns>Line的文本格式 (info已经被转义/去除关键字)</returns>
    //    public new string ToString()//不能继承
    //    {
    //        StringBuilder str = new StringBuilder(TextReplace(Name));
    //        if (info != "")
    //            str.Append('#' + info);
    //        str.Append(":|");
    //            str.Append(subs);
    //        str.Append(text);
    //        return str.ToString();
    //    }
    //    /// <summary>
    //    /// 将当前Line转换成正常的Line格式 (info已经被转义/去除关键字)
    //    /// </summary>
    //    /// <returns>Line的文本格式 (info已经被转义/去除关键字)</returns>
    //    public Line ToLine()//不能继承
    //    {
    //        return new Line(Name, info, Text, Subs.ToArray());           
    //    }

    //    /// <summary>
    //    /// 返回一个 Subs 的第一个元素。
    //    /// </summary>
    //    /// <returns>要返回的第一个元素</returns>
    //    public Sub First()
    //    {
    //        if (Subs.Count == 0)
    //            return null;
    //        return Subs[0];
    //    }
    //    /// <summary>
    //    /// 返回一个 Subs 的最后一个元素。
    //    /// </summary>
    //    /// <returns>要返回的最后一个元素</returns>
    //    public Sub Last()
    //    {
    //        if (Subs.Count == 0)
    //            return null;
    //        return Subs[Subs.Count - 1];
    //    }    
    //}


}
