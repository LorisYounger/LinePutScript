using System;
using System.Windows.Documents;
using LinePutScript;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

namespace LinePutScript.LinePut
{
    /// <summary>
    /// LPT播放器:Lineput内容
    /// </summary>
    public class LPTPlayer : LptDocument
    {
        public RichTextBox Document;
        public LineDisplay IADisplay;

        public LPTPlayer(RichTextBox fdm, string LPT, int start = 1) : base(LPT)
        {
            Document = fdm;
            LineNode = start;//不需要应用，因为继承的LPTDoc
            IADisplay = new LineDisplay(OADisplay);
        }

        /// <summary>
        /// 文本置换
        /// </summary>
        /// <param name="Reptex">原文本</param>
        public string TextDeReplace(string Reptex)
        {
            Reptex = Reptex.Replace("/now", DateTime.Now.ToString());
            Reptex = Reptex.Replace("/date", DateTime.Now.ToShortDateString());
            Reptex = Reptex.Replace("/time", DateTime.Now.ToShortTimeString());
            Reptex = Sub.TextDeReplace(Reptex);
            return Reptex;
        }
        public enum ReturnMsg
        {
            Error,
            PageEnd,
            Cls,
            Others,
            Load,
            EndDoc
        }

        ///// <summary>
        ///// 控制显示委托 可用于插件,实现特殊功能等
        ///// </summary>
        ///// <param name="order">指令</param>
        ///// <param name="ldp">预备显示内容</param>
        ///// <param name="fd">流文档(输出端口</param>
        //public void PlayDisplay(Sub order, ref LineDisplay ldp, ref FlowDocument fd)
        //{
        //    TextDeReplace(ref ldp.OutPut);
        //    switch (order.Name.ToLower())
        //    {
        //        case "":
        //            break;
        //    }
        //}
        bool clsnext = false;
        public void Back()
        {
            Save();
            NowSaveID -= 2;
            if (NowSaveID < 0)
            {
                NowSaveID = 0;
                return;
            }
            TextRange TS = new TextRange(Document.Document.ContentStart, Document.Document.ContentEnd);
            TS.Load(MSs[NowSaveID], DataFormats.Xaml);
        }
        public void NextToCls()
        {
            while (Next() == ReturnMsg.PageEnd) ;
        }
        public ReturnMsg Next()
        {
            if (!ReadCanNext() && MSs.Count < NowSaveID)//如果没有下一句，则直接退出
            {
                return ReturnMsg.EndDoc;
            }
            if (MSs.Count > NowSaveID + 1)//如果有存档 先加载存档
            {
                TextRange TS = new TextRange(Document.Document.ContentStart, Document.Document.ContentEnd);
                TS.Load(MSs[NowSaveID + 1], DataFormats.Xaml);
                return ReturnMsg.Load;
            }
            Save();
            bool next = true;
            bool NextUseRun = false;
            Line line; LineDisplay disThis;
            FlowDocument fd = Document.Document;
            if (clsnext)
            {
                clsnext = false;
                fd.Blocks.Clear();
            }
            while (next)
            {
                if (!ReadCanNext())
                {
                    return ReturnMsg.EndDoc;
                }
                line = ReadNext();
                line.Subs.Insert(0, line);//登记下本地
                disThis = new LineDisplay(IADisplay);

                foreach (Sub sub in line)
                {
                    switch (sub.Name.ToLower())
                    {
                        case "":
                        case "ia"://防止和IAdisplay头重叠
                        case "linedisplay"://防止自动生成的代码中linedisplay混淆
                            break;
                        case "cls":
                            clsnext = true;
                            next = false;
                            break;
                        case "pageend":
                        case "end":
                            next = false;
                            break;
                        case "shell":
                            Process.Start(sub.Info);
                            break;
                        case "goto":
                            this.LineNode = sub.InfoToInt;
                            break;
                        case "img":
                        case "open":
                        case "mov":
                            break;//todo
                        case "msg":
                            MessageBox.Show(sub.Info);
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
                                IADisplay.FontFamily = disThis.FontFamily;
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
                                IADisplay.FontColor = disThis.FontColor;
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
                                IADisplay.FontSize = disThis.FontSize;
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
                                IADisplay.BackColor = disThis.BackColor;
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
                                IADisplay.BackColor = disThis.BackColor;
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
                            Output(new LineDisplay(IADisplay) { OutPut = '|' + sub.ToString() }, fd);
                            break;
                    }
                }

                disThis.OutPut += TextDeReplace(line.text);

                bool nextnextuserun = !disThis.OutPut.EndsWith("\n");
                if (!nextnextuserun)
                    disThis.OutPut = disThis.OutPut.Substring(0, disThis.OutPut.Length - 1);

                Output(disThis, fd, IADisplay, NextUseRun);//输出

                NextUseRun = nextnextuserun;
            }
            if (clsnext)
                return ReturnMsg.Cls;
            else
                return ReturnMsg.PageEnd;
        }
        public List<MemoryStream> MSs = new List<MemoryStream>();
        public int NowSaveID = 1;
        /// <summary>
        /// 保存每次切换的数据，用于切换页面
        /// </summary>
        public void Save()
        {
            TextRange TS = new TextRange(Document.Document.ContentStart, Document.Document.ContentEnd);
            MemoryStream ms = new MemoryStream();
            TS.Save(ms, DataFormats.Xaml);
            if (NowSaveID >= MSs.Count)
            {
                MSs.Add(ms);
            }
            else
            {
                MSs[NowSaveID].Dispose();
                MSs[NowSaveID] = ms;
            }
            NowSaveID += 1;
        }
    }
}
