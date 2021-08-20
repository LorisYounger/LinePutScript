using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Xml;
using System.Linq;
using LinePutScript;
using System.Text.RegularExpressions;

namespace LinePutScript.LinePut
{
    /// <summary>
    /// 记录一行所用的参数 (实意版本,不是纯文本)
    /// </summary>
    public class LineDisplay
    {
        /// <summary>
        /// 字体大小
        /// </summary>
        public float FontSize = 12;
        /// <summary>
        /// 字体颜色
        /// </summary>
        public Color FontColor = Colors.White;
        /// <summary>
        /// 背景颜色
        /// </summary>
        public Color BackColor = Color.FromRgb(68, 68, 68);
        /// <summary>
        /// 字体
        /// </summary>
        public FontFamily FontFamily = new FontFamily("Microsoft YaHei UI");
        /// <summary>
        /// 输出文本内容
        /// </summary>
        public string OutPut = "";//部分可能会用到
        /// <summary>
        /// 位置
        /// </summary>
        public TextAlignment Alignment = TextAlignment.Left;//有其他属性以paragraph结尾
        /// <summary>
        /// 加粗
        /// </summary>
        public bool Bold = false;
        /// <summary>
        /// 斜体
        /// </summary>
        public bool Italic = false;
        /// <summary>
        /// 下划线
        /// </summary>
        public bool Underline = false;
        /// <summary>
        /// 横线
        /// </summary>
        public bool Strikethrough = false;
        /// <summary>
        /// 新建实体行
        /// </summary>
        public LineDisplay()
        {

        }
        /// <summary>
        /// 复制实体行格式(不包括输出)
        /// </summary>
        /// <param name="ld"></param>
        public LineDisplay(LineDisplay ld)
        {
            Set(ld);
        }
        /// <summary>
        /// 从别处复制一份参数到本LD,(注意不包含Output)
        /// </summary>
        /// <param name="ld">样本</param>
        public void Set(LineDisplay ld)
        {
            FontSize = ld.FontSize;
            FontColor = ld.FontColor;
            BackColor = ld.BackColor;
            FontFamily = ld.FontFamily;
            Alignment = ld.Alignment;
            Bold = ld.Bold;
            Italic = ld.Italic;
            Underline = ld.Underline;
            Strikethrough = ld.Strikethrough;
        }
        /// <summary>
        /// 判断两个实意行是否有相同的格式 (不对比文本)
        /// </summary>
        /// <param name="ld">被对比的实意行</param>
        /// <returns></returns>
        public bool Equals(LineDisplay ld)
        {
            return !(FontSize != ld.FontSize || FontColor != ld.FontColor || BackColor != ld.BackColor || FontFamily != ld.FontFamily ||
             Alignment != ld.Alignment || Bold != ld.Bold || Italic != ld.Italic || Underline != ld.Underline || Strikethrough != ld.Strikethrough);
        }
        ///在什么情况下用Paragraph或者Run: 有非默认的TextAlignment
        public bool UseRun()
        {
            return Alignment == TextAlignment.Left;
        }
        /// <summary>
        /// 在什么情况下用Paragraph或者Run: 有非默认的TextAlignment
        /// </summary>
        /// <param name="OA"></param>
        /// <returns></returns>
        public bool UseRun(LineDisplay OA)
        {
            return OA.Alignment == TextAlignment.Left;
        }
        /// <summary>
        /// 输出Run
        /// </summary>
        /// <returns></returns>
        public Run OutPutRun()
        {
            Run run = new Run(OutPut);
            if (Bold)
                run.FontWeight = FontWeights.Bold;

            if (Italic)
                run.FontStyle = FontStyles.Italic;

            if (Underline)
                run.TextDecorations = TextDecorations.Underline;

            if (Strikethrough)
                run.TextDecorations = TextDecorations.Strikethrough;

            run.FontSize = FontSize;
            run.FontFamily = FontFamily;

            run.Background = new SolidColorBrush(BackColor);
            run.Foreground = new SolidColorBrush(FontColor);
            //run.TextAlignment = Alignment;
            return run;
        }
        /// <summary>
        /// 输出Run
        /// </summary>
        /// <param name="OA">覆盖通用格式</param>
        /// <returns></returns>
        public Run OutPutRun(LineDisplay OA)
        {
            Run run = new Run(OutPut);

            if (OA.Bold != Bold)
                if (Bold)
                    run.FontWeight = FontWeights.Bold;

            if (OA.Italic != Italic)
                if (Italic)
                    run.FontStyle = FontStyles.Italic;

            if (OA.Underline != Underline)
                if (Underline)
                    run.TextDecorations = TextDecorations.Underline;

            if (OA.Strikethrough != Strikethrough)
                if (Strikethrough)
                    run.TextDecorations = TextDecorations.Strikethrough;

            if (OA.FontSize.ToString("f1") != FontSize.ToString("f1"))
                run.FontSize = FontSize;
            if (OA.FontFamily != FontFamily)
                run.FontFamily = FontFamily;
            if (OA.BackColor != BackColor)
                run.Background = new SolidColorBrush(BackColor);
            if (OA.FontColor != FontColor)
                run.Foreground = new SolidColorBrush(FontColor);
            //run.TextAlignment = Alignment;
            return run;
        }
        /// <summary>
        /// 输出Paragraph
        /// </summary>
        /// <returns></returns>
        public Paragraph OutPutParagraph()
        {
            Paragraph run = new Paragraph(new Run(OutPut));
            //run.Inlines.Add();//一个重要的小提示
            if (Bold)
                run.FontWeight = FontWeights.Bold;

            if (Italic)
                run.FontStyle = FontStyles.Italic;

            if (Underline)
                run.TextDecorations = TextDecorations.Underline;

            if (Strikethrough)
                run.TextDecorations = TextDecorations.Strikethrough;

            run.FontSize = FontSize;
            run.FontFamily = FontFamily;
            run.Background = new SolidColorBrush(BackColor);
            run.Foreground = new SolidColorBrush(FontColor);
            run.TextAlignment = Alignment;

            return run;
        }

        /// <summary>
        /// 将行显示行(实意)转换成行(文本) //注 |:| 会直接输出成行
        /// </summary>
        /// <returns>行(文本)</returns>
        public Line ToLine()
        {
            if (OutPut.StartsWith("|") && OutPut.EndsWith(":|"))
                return new Line(OutPut.Substring(1));
            return new Line("linedisplay", "", OutPut, ToSubs());
        }
        /// <summary>
        /// 将行显示行(实意)转换成行(文本) //注 |:| 会直接输出成行
        /// </summary>
        /// <param name="OA">全局变量</param>
        /// <returns>行(文本)</returns>
        public Line ToLine(LineDisplay OA)
        {
            if (OutPut.StartsWith("|") && OutPut.EndsWith(":|"))
                return new Line(OutPut.Substring(1));
            return new Line("linedisplay", "", OutPut, ToSubs(OA));
        }
        /// <summary>
        /// 将显示行(实意)转换成Sub集合
        /// </summary>
        /// <returns>行(文本)</returns>
        public Sub[] ToSubs()
        {
            List<Sub> subs = new List<Sub>();
            if (Bold)
                subs.Add(new Sub("Bold", ""));

            if (Italic)
                subs.Add(new Sub("Italic", ""));

            if (Underline)
                subs.Add(new Sub("Underline", ""));

            if (Strikethrough)
                subs.Add(new Sub("Deleteline", ""));

            subs.Add(new Sub("FontSize", FontSize.ToString("f2")));
            subs.Add(new Sub("FontFamily", FontFamily.ToString()));
            subs.Add(new Sub("BackColor", LptDocument.ColorToHEX(BackColor)));
            subs.Add(new Sub("FontColor", LptDocument.ColorToHEX(FontColor)));
            switch (Alignment)
            {
                case TextAlignment.Left:
                    subs.Add(new Sub("Left", "")); break;
                case TextAlignment.Right:
                    subs.Add(new Sub("Right", "")); break;
                case TextAlignment.Center:
                    subs.Add(new Sub("Center", "")); break;
                case TextAlignment.Justify:
                    subs.Add(new Sub("Justify", "")); break;
            }
            return subs.ToArray();
        }
        /// <summary>
        /// 将显示行(实意)转换成Sub集合
        /// </summary>
        /// <param name="OA">全局变量</param>
        /// <returns>行(文本)</returns>
        public Sub[] ToSubs(LineDisplay OA)
        {
            List<Sub> subs = new List<Sub>();
            if (OA.Bold != Bold)
                if (Bold)
                    subs.Add(new Sub("Bold", ""));
            if (OA.Italic != Italic)
                if (Italic)
                    subs.Add(new Sub("Italic", ""));
            if (OA.Underline != Underline)
                if (Underline)
                    subs.Add(new Sub("Underline", ""));
            if (OA.Strikethrough != Strikethrough)
                if (Strikethrough)
                    subs.Add(new Sub("Deleteline", ""));

            if (OA.FontSize.ToString("f1") != FontSize.ToString("f1"))
                subs.Add(new Sub("FontSize", FontSize.ToString("f2")));
            if (OA.FontFamily != FontFamily)
                subs.Add(new Sub("FontFamily", FontFamily.ToString()));
            if (OA.BackColor != BackColor)
                subs.Add(new Sub("BackColor", LptDocument.ColorToHEX(BackColor)));
            if (OA.FontColor != FontColor)
                subs.Add(new Sub("FontColor", LptDocument.ColorToHEX(FontColor)));
            if (OA.Alignment != Alignment)
                switch (Alignment)
                {
                    case TextAlignment.Left:
                        subs.Add(new Sub("Left", "")); break;
                    case TextAlignment.Right:
                        subs.Add(new Sub("Right", "")); break;
                    case TextAlignment.Center:
                        subs.Add(new Sub("Center", "")); break;
                    case TextAlignment.Justify:
                        subs.Add(new Sub("Justify", "")); break;
                }
            return subs.ToArray();
        }
        /// <summary>
        /// 对多个实意行进行简化处理 (合并同类项)
        /// </summary>
        /// <param name="displays">要被整理的多个实意</param>
        /// <returns></returns>
        public static LineDisplay[] SimplifyLineDisplays(LineDisplay[] displays)
        {
            if (displays.Length <= 1)
                return displays;
            List<LineDisplay> lines = new List<LineDisplay>();
            lines.Add(displays[0]);

            for (int i = 1; i < displays.Length; i++)
            {
                if (displays[i].OutPut.Contains(":|"))
                {
                    if (lines.Last().Equals(displays[i]))
                    {//如果相同就把文本加进去
                        displays[i].OutPut = lines.Last().OutPut + displays[i].OutPut;
                        lines.RemoveAt(lines.Count - 1);
                    }
                    string[] spl = Regex.Split(displays[i].OutPut, @"\:\|", RegexOptions.IgnoreCase);
                    int sp2;
                    for (int i1 = 0; i1 < spl.Length - 1; i1++)
                    {
                        string sp = (string)spl[i1];
                        sp2 = sp.LastIndexOf('|');
                        if (sp2 == -1)
                            lines.Add(new LineDisplay(displays[i]) { OutPut = sp });
                        else if (sp2 == 0)
                        {
                            lines.Add(new LineDisplay() { OutPut = sp + ":|" });
                        }
                        else
                        {
                            lines.Add(new LineDisplay(displays[i]) { OutPut = sp.Substring(0, sp2) });
                            lines.Add(new LineDisplay() { OutPut = sp.Substring(sp2) + ":|" });
                            Console.Write("abc");
                        }
                    }
                }
                else
                {
                    if (lines.Last().Equals(displays[i]))
                    {//如果相同就把文本加进去
                        lines.Last().OutPut += displays[i].OutPut;
                    }
                    else
                    {//不相同就加新的进去
                        lines.Add(displays[i]);
                    }
                }
            }
            return lines.ToArray();
        }
        /// <summary>
        /// 将多个实意转换成Lps文档
        /// </summary>
        /// <param name="displays">实意行显示</param>
        /// <returns></returns>
        public static LpsDocument LineDisplaysToLpsDocument(LineDisplay[] displays)
        {
            LpsDocument lps = new LpsDocument();
            foreach (LineDisplay ld in displays)
            {
                lps.AddLine(ld.ToLine());
            }
            return lps;
        }
        /// <summary>
        /// 将多个实意转换成Lps文档
        /// </summary>
        /// <param name="displays">实意行显示</param>
        /// <param name="OA">全局变量</param>
        /// <returns></returns>
        public static LpsDocument LineDisplaysToLpsDocument(LineDisplay[] displays, LineDisplay OA)
        {
            LpsDocument lps = new LpsDocument();
            foreach (LineDisplay ld in displays)
            {
                lps.AddLine(ld.ToLine(OA));
            }
            return lps;
        }



        //xml操作

        /// <summary>
        /// 将FlowDocument(Xaml)转换成实意显示行
        /// </summary>
        /// <param name="xaml">FlowDocument(Xaml)</param>
        /// <param name="OA">全局变量</param>
        /// <returns></returns>
        public static LineDisplay[] XamlToLPT(FlowDocument xaml, LineDisplay OA)
        {
            List<LineDisplay> lps = new List<LineDisplay>();
            MemoryStream ms = new MemoryStream();

            // write XAML out to a MemoryStream
            TextRange tr = new TextRange(
                xaml.ContentStart,
                xaml.ContentEnd);
            tr.Save(ms, DataFormats.Xaml);
            ms.Seek(0, SeekOrigin.Begin);

            XmlDocument xml = new XmlDocument();
            xml.Load(XmlReader.Create(ms, new XmlReaderSettings() { IgnoreComments = true }));

            //读取XAML转换成LineDisplay

            //得到顶层节点列表 
            XmlNodeList topM = xml.DocumentElement.ChildNodes;
            foreach (XmlElement element in topM)
            {
                if (element.Name == "Paragraph")
                {
                    GetLineDisplaysFromLoopXmlElement(element, lps, OA);
                    if (lps.Count == 0)
                        lps.Add(new LineDisplay(OA) { OutPut = "\n" });
                    else
                        lps.Last().OutPut += '\n';
                }
                else
                {//ToDo:支持图片
                    MessageBox.Show(element.Name);
                }
                //清空多余的回车
            }
            if (lps.Count != 0)//空的退回空
                lps.Last().OutPut = lps.Last().OutPut.TrimEnd('\n');
            return lps.ToArray();
        }
        /// <summary>
        /// 获取这一xml元素的全部数据
        /// </summary>
        /// <param name="element">ml元素</param>
        /// <param name="OA">全局/临时全局变量</param>
        /// <returns></returns>
        public static LineDisplay GetLineDisplayFormXmlElement(XmlElement element, LineDisplay OA)
        {
            LineDisplay tmp = new LineDisplay(OA);
            foreach (XmlAttribute atr in element.Attributes)
            {
                switch (atr.NodeType)
                {
                    case XmlNodeType.Text:
                        tmp.OutPut += atr.Value;
                        break;
                    case XmlNodeType.Attribute:
                        switch (atr.Name)
                        {
                            case "":
                            case "Margin":
                            case "xml:lang":
                                //弃用的参数
                                break;
                            default:
                                MessageBox.Show(atr.Name);
                                break;
                            //ToDo:支持图片
                            case "FontFamily":
                                tmp.FontFamily = new FontFamily(atr.Value);
                                break;
                            case "Foreground":
                                tmp.FontColor = LptDocument.HEXToColor(atr.Value);
                                break;
                            case "FontSize":
                                tmp.FontSize = Convert.ToSingle(atr.Value);
                                break;
                            case "Background":
                                tmp.BackColor = LptDocument.HEXToColor(atr.Value);
                                break;
                            case "FontWeight":
                                tmp.Bold = (atr.Value == "Bold"); break;
                            case "FontStyle":
                                tmp.Italic = (atr.Value == "Italic"); break;
                            case "TextDecorations":
                                if (atr.Value == "Strikethrough")
                                    tmp.Strikethrough = !tmp.Strikethrough;
                                else if (atr.Value == "Underline")
                                    tmp.Underline = !tmp.Underline;
                                else
                                {
                                    tmp.Strikethrough = false; tmp.Underline = false;
                                }
                                break;
                            case "TextAlignment":
                                switch (atr.Value)
                                {
                                    case "Left":
                                        tmp.Alignment = TextAlignment.Left; break;
                                    case "Right":
                                        tmp.Alignment = TextAlignment.Right; break;
                                    case "Center":
                                        tmp.Alignment = TextAlignment.Center; break;
                                    case "Justify":
                                        tmp.Alignment = TextAlignment.Justify; break;
                                    default:
                                        tmp.Alignment = TextAlignment.Justify; break;
                                }
                                break;
                        }
                        break;
                    case XmlNodeType.Element:
                        MessageBox.Show(atr.Name);
                        break;
                    default:
                        MessageBox.Show(atr.Name);
                        break;
                        //case XmlNodeType.Attribute:
                        //    tmp =GetLineDisplayFormXmlElement((XmlElement)atr, tmp);
                        //    break;
                }
            }
            return tmp;
        }

        /// <summary>
        /// 循环获取文本输出从xml元素
        /// </summary>
        /// <param name="element">xml元素</param>
        /// <param name="displays">显示行列表</param>
        /// <param name="OA">全局/临时全局变量</param>
        public static void GetLineDisplaysFromLoopXmlElement(XmlElement element, List<LineDisplay> displays, LineDisplay OA)
        {
            //先读这个element
            LineDisplay tmp = GetLineDisplayFormXmlElement(element, OA);
            if (tmp.OutPut != "")//如果有内容就先输出
            {
                displays.Add(tmp);
            }
            foreach (XmlNode xn in element.ChildNodes)
            {
                //由于不可能有注释 直接转换
                //放入循环中进行读取
                switch (xn.NodeType)
                {
                    case XmlNodeType.Text:
                        if (tmp.OutPut == "")
                        {
                            tmp.OutPut = xn.Value;
                            displays.Add(tmp);
                        }
                        else
                        {
                            tmp.OutPut = xn.Value;
                        }
                        break;
                    case XmlNodeType.Element:
                        GetLineDisplaysFromLoopXmlElement((XmlElement)xn, displays, tmp);
                        break;
                    case XmlNodeType.SignificantWhitespace:
                        if (displays.Count == 0)
                            displays.Add(new LineDisplay(OA) { OutPut = xn.InnerText });
                        else
                            displays.Last().OutPut += xn.InnerText;
                        break;
                    default://ToDo:支持图片
                        MessageBox.Show(xn.Name);
                        break;
                }
            }
        }
    }
}
