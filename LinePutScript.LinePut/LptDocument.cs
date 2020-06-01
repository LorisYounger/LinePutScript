using System;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using LinePutScript;

namespace LinePutScript.LinePut
{
    //MainWindows的一些类和
    /// <summary>
    /// LPT文件:继承从LPS
    /// </summary>
    public class LptDocument : LpsDocument
    {
        #region 相关方法
        /// <summary>
        /// 颜色转换
        /// </summary>
        /// <param name="color">Media颜色转Drawing颜色</param>
        /// <returns>Media颜色</returns>
        public static System.Drawing.Color ColorConvent(Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
        /// <summary>
        /// 颜色转换
        /// </summary>
        /// <param name="color">Drawing颜色转Media颜色</param>
        /// <returns>Drawing颜色</returns>
        public static Color ColorConvent(System.Drawing.Color color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        /// <summary>
        /// HEX颜色值转颜色
        /// </summary>
        /// <param name="HEX">HEX颜色值</param>
        /// <returns></returns>
        public static Color HEXToColor(string HEX = "")
        {
            HEX = HEX.Trim('#').ToLower();
            try
            {
                switch (HEX.Length)
                {
                    case 3:
                        return Color.FromRgb(Convert.ToByte(HEX[0].ToString() + HEX[0], 16), Convert.ToByte(HEX[1].ToString() + HEX[1], 16), Convert.ToByte(HEX[2].ToString() + HEX[2], 16));
                    case 6:
                        return Color.FromRgb(Convert.ToByte(HEX[0].ToString() + HEX[1], 16), Convert.ToByte(HEX[2].ToString() + HEX[3], 16), Convert.ToByte(HEX[4].ToString() + HEX[5], 16));
                    case 8:
                        return Color.FromArgb(Convert.ToByte(HEX[0].ToString() + HEX[1], 16), Convert.ToByte(HEX[2].ToString() + HEX[3], 16), Convert.ToByte(HEX[4].ToString() + HEX[5], 16), Convert.ToByte(HEX[6].ToString() + HEX[7], 16));
                }
            }
            catch
            {

            }
            int hash = Math.Abs(HEX.GetHashCode());
            int hash1 = hash / 256;
            int hash2 = hash1 / 256;
            return Color.FromRgb((byte)(hash % 256), (byte)(hash1 % 256), (byte)(hash2 % 256));
        }
        /// <summary>
        /// 颜色转HEX颜色值
        /// </summary>
        /// <param name="Color">颜色</param>
        /// <returns></returns>
        public static string ColorToHEX(Color Color)
        {
            string returnValue = Convert.ToInt32(Color.R * 65536 + Color.G * 256 + Color.B).ToString("x");
            return returnValue.PadLeft(6, '0');
        }
        #endregion
        /// <summary>
        /// 新建一个LPT文档从LPT文本
        /// </summary>
        /// <param name="LPT">lpt文本</param>
        public LptDocument(string LPT) : base(LPT)
        {
            foreach (Sub sub in First())
            {
                switch (sub.Name.ToLower())
                {
                    case "allfontcolor":
                    case "fontcolor":
                        if (sub.info == null)
                        {
                            //log.Append(sub.Name + ":未找到颜色记录\n");
                        }
                        else
                        {
                            OADisplay.FontColor = HEXToColor(sub.info);
                        }
                        break;

                    case "backgroundcolor":
                    case "background":
                    case "allbackcolor":
                    case "backcolor":
                        if (sub.info == null)
                        {
                            //log.Append(sub.Name + ":未找到颜色记录\n");
                        }
                        else
                        {
                            OADisplay.BackColor = HEXToColor(sub.info);
                        }
                        break;
                    case "fontsize":
                    case "allfontsize":
                        if (sub.info == null)
                        {
                            //log.Append(sub.Name + ":未找到颜色记录\n");
                        }
                        else
                        {
                            OADisplay.FontSize = Convert.ToSingle(sub.info);
                        }
                        break;

                    case "font":
                    case "fontfamily":
                    case "allfontfamily":
                        if (sub.info == null)
                        {
                            //log.Append(sub.Name + ":未找到颜色记录\n");
                        }
                        else
                        {
                            OADisplay.FontFamily = new FontFamily(sub.info);
                        }
                        break;

                    case "u":
                    case "underline":
                        OADisplay.Underline = true; break;
                    case "b":
                    case "bold":
                        OADisplay.Bold = true; break;
                    case "i":
                    case "italic":
                        OADisplay.Italic = true; break;
                    case "d":
                    case "deleteline":
                        OADisplay.Strikethrough = true; break;
                    case "l":
                    case "left":
                        OADisplay.Alignment = TextAlignment.Left; break;
                    case "r":
                    case "right":
                        OADisplay.Alignment = TextAlignment.Right; break;
                    case "c":
                    case "center":
                        OADisplay.Alignment = TextAlignment.Center; break;
                    case "j":
                    case "justify":
                        OADisplay.Alignment = TextAlignment.Justify; break;


                    case "ver":
                    case "verizon":
                        if (sub.info != null)
                        {
                            Verison = sub.info;
                        }
                        break;

                    default:
                        FirstLineOtherInfo += $"{sub.Name}#{sub.info}:|";
                        break;
                }
            }
        }


        /// <summary>
        /// 获取文件版本
        /// </summary>
        public string Verison = "-1";
        /// <summary>
        /// 获取第一行除了版本以外的其他全部信息
        /// </summary>
        public string FirstLineOtherInfo = "";
        /// <summary>
        /// 编译错误记录
        /// </summary>
        public StringBuilder log = new StringBuilder();

        /// <summary>
        /// 实际输出用的文本转换,无可逆
        /// </summary>
        /// <param name="Reptex">要被转换的文本</param>
        public void TextReplace(ref string Reptex)
        {
            Reptex = Reptex.Replace("/date", DateTime.Now.ToShortDateString());
            Reptex = Reptex.Replace("/time", DateTime.Now.ToShortTimeString());
            Reptex = Reptex.Replace("/datetime", DateTime.Now.ToString());
            Reptex = Reptex.Replace("/lnow", LineNode.ToString());
            Reptex = Reptex.Replace("/llen", Assemblage.Count.ToString());
        }

        public LineDisplay OADisplay = new LineDisplay();
        /// <summary>
        /// 如果为true,跳过一次pageend
        /// </summary>
        public bool IsSkipPageEnd = false;
        /// <summary>
        /// 显示其中一行的内容//只是单纯的显示(编辑用)
        /// </summary>
        /// <param name="line">哪一行的内容是</param>
        /// <param name="fd">要被显示的文档</param>
        public static void DisplayLine(Line line, FlowDocument fd, LineDisplay IAld, ref bool NextUseRun)
        {
            //Note:
            //pageend等指令插入使用|pageend:|

            line = new Line(line);//复制一个Line//(不会更改提供的内容)
            line.InsertSub(0, line);//Line将会被加进去,以至于可以直接在遍历中找到Line.Name+info
                                    //输出的这一行将会用什么
            LineDisplay disThis = new LineDisplay(IAld);
            //IAld = new LineDisplay(OAld);//不new 在外部完成 毕竟不止有一个line
            //区别:OA=全局,不会更变 IA=局部,会被更改




            foreach (Sub sub in line)
            {
                switch (sub.Name.ToLower())
                {
                    case "":
                    case "ia"://防止和IAdisplay头重叠
                    case "linedisplay"://防止自动生成的代码中linedisplay混淆
                        break;
                    //case "cls":Cls不可能出现 出现就是bug
                    //    fd.Blocks.Clear();
                    //    break;
                    case "pageend":
                    case "end":
                        Output(new LineDisplay(IAld) { OutPut = $"|{sub.Name.ToLower()}:|" }, fd);
                        break;
                    case "shell":
                    case "goto":
                    case "img":
                    case "open":
                    case "mov":
                    case "msg":
                        Output(new LineDisplay(IAld) { OutPut = $"|{sub.Name.ToLower()}#{sub.info}:|" }, fd);
                        break;

                    case "font":
                    case "fontfamily":
                        if (sub.info == null)
                        {
                            //log.Append(sub.Name + ":未找到颜色记录\n");
                        }
                        else
                        {
                            disThis.FontFamily = new FontFamily(sub.Info);
                        }
                        break;
                    case "allfont":
                    case "allfontfamily":
                        if (sub.info == null)
                        {
                            //log.Append(sub.Name + ":未找到颜色记录\n");
                        }
                        else
                        {
                            disThis.FontFamily = new FontFamily(sub.Info);
                            IAld.FontFamily = disThis.FontFamily;
                            fd.FontFamily = disThis.FontFamily;
                        }
                        break;
                    case "fontcolor":
                        if (sub.info == null)
                        {
                            //log.Append(sub.Name + ":未找到颜色记录\n");
                        }
                        else
                        {
                            disThis.FontColor = HEXToColor(sub.info);
                        }
                        break;
                    case "allfontcolor":
                        if (sub.info == null)
                        {
                            //log.Append(sub.Name + ":未找到颜色记录\n");
                        }
                        else
                        {
                            disThis.FontColor = HEXToColor(sub.info);
                            IAld.FontColor = disThis.FontColor;
                            fd.Foreground = new SolidColorBrush(disThis.FontColor);
                        }
                        break;
                    case "fontsize":
                        if (sub.info == null)
                        {
                            //log.Append(sub.Name + ":未找到字体大小记录\n");
                        }
                        else
                        {
                            disThis.FontSize = Convert.ToSingle(sub.info);
                        }
                        break;
                    case "allfontsize":
                        if (sub.info == null)
                        {
                            //log.Append(sub.Name + ":未找到字体大小记录\n");
                        }
                        else
                        {
                            disThis.FontSize = Convert.ToSingle(sub.info);
                            IAld.FontSize = disThis.FontSize;
                        }
                        break;
                    case "backcolor":
                        if (sub.info == null)
                        {
                            //log.Append(sub.Name + ":未找到颜色记录\n");
                        }
                        else
                        {
                            disThis.BackColor = HEXToColor(sub.info);
                        }
                        break;
                    case "allbackcolor":
                        if (sub.info == null)
                        {
                            //log.Append(sub.Name + ":未找到颜色记录\n");
                        }
                        else
                        {
                            disThis.BackColor = HEXToColor(sub.info);
                            IAld.BackColor = disThis.BackColor;
                        }
                        break;
                    case "backgroundcolor":
                        if (sub.info == null)
                        {
                            //log.Append(sub.Name + ":未找到颜色记录\n");
                        }
                        else
                        {
                            disThis.BackColor = HEXToColor(sub.info);
                            IAld.BackColor = disThis.BackColor;
                            fd.Background = new SolidColorBrush(HEXToColor(sub.info));
                        }
                        break;


                    case "u":
                    case "underline":
                        disThis.Underline = !disThis.Underline; break;
                    case "b":
                    case "bold":
                        disThis.Bold = !disThis.Bold; break;
                    case "i":
                    case "italic":
                        disThis.Italic = !disThis.Italic; break;
                    case "d":
                    case "deleteline":
                        disThis.Strikethrough = !disThis.Strikethrough; break;
                    case "l":
                    case "left":
                        disThis.Alignment = TextAlignment.Left; break;
                    case "r":
                    case "right":
                        disThis.Alignment = TextAlignment.Right; break;
                    case "c":
                    case "center":
                        disThis.Alignment = TextAlignment.Center; break;
                    case "j":
                    case "justify":
                        disThis.Alignment = TextAlignment.Justify; break;

                    //不兼容em
                    case "h1":
                        disThis.FontSize = 48;
                        break;
                    case "h2":
                        disThis.FontSize = 36;
                        break;
                    case "h3":
                        disThis.FontSize = 24;
                        break;
                    case "h4":
                        disThis.FontSize = 16;
                        break;
                    case "h5":
                        disThis.FontSize = 12;
                        break;
                    case "h6":
                        disThis.FontSize = 9;
                        break;

                    default:
                        //支持行内其他代码,为以后的支持插件做准备 //ToDo:跳转(goto)做成官方内置插件 使用委托
                        Output(new LineDisplay(IAld) { OutPut = '|' + sub.ToString() }, fd);
                        break;
                }
            }

            disThis.OutPut += TextDeReplaceMD(line.text);//注:这个是用魔改/stop还是/stop 之所以这么用是因为这个是编辑用不是展示用

            bool nextnextuserun = !disThis.OutPut.EndsWith("\n");
            if (!nextnextuserun)
                disThis.OutPut = disThis.OutPut.Substring(0, disThis.OutPut.Length - 1);

            Output(disThis, fd, IAld, NextUseRun);//输出

            NextUseRun = nextnextuserun;

        }
        /// <summary>
        /// 输出内容到FlowDocument
        /// </summary>
        /// <param name="disThis">要输出的内容</param>
        /// <param name="fd">显示的FlowDocument</param>
        /// <param name="OAld">全局,可不填</param>
        public static void Output(LineDisplay disThis, FlowDocument fd, LineDisplay OAld = null, bool UseRun = true)
        {
            if (disThis.OutPut == "")
                return;
            //***一个比较有用的案例: (读取的时候也可以使用这个)
            //TextBox1.Document.Blocks.Add(new Paragraph(new Run("内容") { }))
            if (OAld == null)
            {
                if (fd.Blocks.Count != 0 && fd.Blocks.LastBlock.GetType().ToString() == "System.Windows.Documents.Paragraph")
                {
                    ((Paragraph)fd.Blocks.LastBlock).Inlines.Add(disThis.OutPutRun());
                }
                else
                {
                    fd.Blocks.Add(disThis.OutPutParagraph());
                }
            }
            else if (UseRun && disThis.UseRun(OAld))
            {
                if (fd.Blocks.Count != 0 && fd.Blocks.LastBlock.GetType().ToString() == "System.Windows.Documents.Paragraph")
                {
                    ((Paragraph)fd.Blocks.LastBlock).Inlines.Add(disThis.OutPutRun());//OutPutRun\Prargraph 真的没有用?emm
                }
                else
                {
                    fd.Blocks.Add(disThis.OutPutParagraph());
                }
            }
            else
            {
                fd.Blocks.Add(disThis.OutPutParagraph());
            }
        }
        /// <summary>
        /// 将文本进行反转义处理(正常显示的文本) //魔改版:不更改/stop
        /// </summary>
        /// <param name="Reptex">要反转义的文本</param>
        /// <returns>反转义后的文本 正常显示的文本</returns>
        public static string TextDeReplaceMD(string Reptex)
        {
            Reptex = Reptex.Replace("/tab", "\t");
            Reptex = Reptex.Replace("/n", "\n");
            Reptex = Reptex.Replace("/r", "\r");
            Reptex = Reptex.Replace("/id", "#");
            Reptex = Reptex.Replace("/!", "/");
            Reptex = Reptex.Replace("/com", ",");
            return Reptex;
        }
        ////Todo:完全弃用这个方法?转移到Player? 或从Player转移到这个
        ///// <summary>
        ///// 显示当前阅读行的//包括替换(演讲用)
        ///// </summary>
        ///// <param name="line">哪一行的内容是</param>
        ///// <param name="fd">要被显示的文档</param>
        ///// <param name="IAld">半全局变量 会随时更改</param>
        ///// <param name="dc">播放控制</param>
        //public void DisplayLine(Line line, FlowDocument fd, LineDisplay IAld, DisplayControl dc)//Todo:使用委托在Player实现特殊替换的等功能 这样player就不用担心
        //{
        //    //ToDo
        //}

        /// <summary>
        /// 控制显示委托 可用于插件,实现特殊功能等
        /// </summary>
        /// <param name="order">指令</param>
        /// <param name="ldp">预备显示内容</param>
        /// <param name="fd">流文档(输出端口</param>
        public delegate void DisplayControl(Sub order, ref LineDisplay ldp, ref FlowDocument fd);

    }
}
