using LinePutScript;
using LinePutScript.Converter;
using LinePutScript.Dictionary;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using static LinePutScript.Converter.LPSConvert;

namespace TestConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("欢迎使用 LinePutScript 测试控制台");

            LpsDocument lps = new LpsDocument();

            while (true)
            {
                string[] order = (Console.ReadLine() ?? "").Split(new char[] { ' ' }, 2);
                switch (order[0].ToLower())
                {
                    case "":
                    case "test":
                        Text();
                        break;
                    case "addline":
                        lps.AddLine(new Line(order[1]));
                        break;
                    case "tostring":
                        Console.WriteLine(lps.ToString());
                        break;
                    case "exit":
                    case "quit":
                        return;
                }
            }
        }

        static void Text()
        {
            //简单的测试LPS功能是否正常

            LpsDocument lps = new LpsDocument();
            lps[(gint)"money"] = 10000; //设置 money 行 值(int)为10000
            lps["computer"][(gstr)"name"] = "我的电脑";
            lps[(gint)"money"] += 500;

            Console.WriteLine("标准测试:\t" + lps.ToString().Equals(Properties.Resources.test1.Replace("\r", "")));

            lps[(gflt)"flt"] = 3.1415926;
            Console.WriteLine("升级测试1:\t" + lps[(gflt)"flt"].Equals(3.1415926));
            lps["flt"][(gflt)"flt"] = 3.1415926;
            Console.WriteLine("升级测试2:\t" + lps["flt"][(gflt)"flt"].Equals(3.1415926));
            var now = DateTime.Now;
            lps[(gdat)"now"] = now;
            Console.WriteLine("升级测试3:\t" + lps[(gdat)"now"].Equals(now));

            lps = new LpsDocument(Properties.Resources.test3);
            Console.WriteLine($"读取测试1:\t" + lps["flt"][(gflt)"flt"].Equals(0));
            Console.WriteLine("读取测试2:\t" + lps[(gflt)"flt"].Equals(3.1415926));
            Console.WriteLine("输出测试:\t" + lps.ToString().Equals(Properties.Resources.test3.Replace("\r", "")));

            lps = new LpsDocument();
            lps[(gstr)"str"] = "abc=abc";
            Console.WriteLine("SS测试1:\t" + lps["str"].Infos["abc"].Equals("abc"));
            Console.WriteLine("SS测试2:\t" + lps["str"].Infos["bcd"].Equals(""));

            lps.AddLine(new Line("str2"));
            lps["str2"].Infos["abc"] = "abc";
            Console.WriteLine("SS测试3:\t" + lps["str2"].Infos["abc"].Equals("abc"));
            lps["str2"].Infos[(gflt)"flt"] = 114514.191980;
            Console.WriteLine("SS测试4:\t" + lps["str2"].Infos[(gflt)"flt"].Equals(114514.191980));

            lps["str2"].Texts["abc"] = "abc";
            Console.WriteLine("SS测试5:\t" + lps["str2"].Texts["abc"].Equals("abc"));
            lps["str2"].Texts[(gflt)"flt"] = 114514.191980;
            Console.WriteLine("SS测试6:\t" + lps["str2"].Texts[(gflt)"flt"].Equals(114514.191980));

            Console.WriteLine("SS测试\t:\t" + lps.ToString().Equals(Properties.Resources.test2.Replace("\r", "")));

            lps = new LpsDocument(Properties.Resources.test2);
            Console.WriteLine("SS读取测试:\t" + lps["str2"].Texts[(gflt)"flt"].Equals(114514.191980));
            Console.WriteLine("SS输出测试:\t" + lps.ToString().Equals(Properties.Resources.test2.Replace("\r", "")));


            lps["str2"].Texts["crlf"] = "ab\nabc\nc";
            Console.WriteLine("SS测试7:\t" + lps["str2"].Texts["crlf"].Equals("ab\nabc\nc"));

            lps["str2"].Texts[(gint)"gint1"] = 114514;
            Console.WriteLine("SS测试8:\t" + lps["str2"].Texts[(gint)"gint1"].Equals(114514));
            lps["str2"].Texts[(gint)"gint2"] = 1145141919;
            Console.WriteLine("SS测试9:\t" + lps["str2"].Texts[(gint)"gint2"].Equals(1145141919));

            var tc = new testclass()
            {
                getsetDateTime = new DateTime(2200, 1, 1),
                intlistgetset = new int[] { 20, 40, 60, 80, 100 },
                intlist = new List<int>() { 10, 20, 30, 40, 50 },
                pubstr = "pubtest",
                pubint = 20,
                getsetint = 10,
                stringlistgetset = new string[] { "a:|a", "b#b", "c||c" },
                intdict = new Dictionary<int, int>() { { 1, 2 }, { 3, 4 }, { 5, 6 } },
                tc = new testclass()
                {
                    getsetDateTime = new DateTime(2200, 4, 29),
                    intlistgetset = new int[] { 11, 45, 14, 19, 19 },
                    pubstr = "substring",
                    intdict = new Dictionary<int, int>() { { 1, 2 }, { 3, 4 }, { 5, 6 } },
                }
            };
            Console.WriteLine("CV测试1:\t" + LPSConvert.SerializeObject(tc).ToString().Equals(Properties.Resources.test4.Replace("\r", "")));

#pragma warning disable CS8604 // 引用类型参数可能为 null。
            Console.WriteLine("CV测试2:\t" + LPSConvert.SerializeObject(
                LPSConvert.DeserializeObject<testclass>(new LpsDocument(Properties.Resources.test4))
                ).ToString().Equals(Properties.Resources.test4.Replace("\r", "")));
#pragma warning restore CS8604 // 引用类型参数可能为 null。
            //Console.WriteLine(LPSConvert.SerializeObject(
            //    LPSConvert.DeserializeObject<testclass>(new LpsDocument(Properties.Resources.test4))
            //    ).ToString());
            //Console.WriteLine();
            //tc.tc.tc = new testclass() {
            //    pubstr = "intc",
            //};
            //Console.WriteLine(LPSConvert.SerializeObject(tc));
#pragma warning disable CS8604 // 引用类型参数可能为 null。
#pragma warning disable CS8602 // 解引用可能出现空引用。
            var tlpscv = LPSConvert.DeserializeObject<testlpsconv>(LPSConvert.SerializeObject(new testlpsconv(lps, lps.First(), lps.Last().First())));

            if (tlpscv == null)
            {
                Console.WriteLine("CV测试3:\t" + false);
            }
            else
            {
                Console.WriteLine("CV测试3.1:\t" + tlpscv.TestLps.ToString().Equals(lps.ToString()));
                Console.WriteLine("CV测试3.2:\t" + tlpscv.TestLine.ToString().Equals(lps.First().ToString()));
                Console.WriteLine("CV测试3.3:\t" + (tlpscv.TestSub == (lps.Last().First())).ToString());
            }
            var l = new Line("food:|type#Drink:|name#ab钙奶:|price#6.5:|desc#健康美味，经济实惠:|Exp#5:|Strength#5:|StrengthDrink#4:|StrengthFood#5:|Health#1:|Feeling#1:|");
            Console.WriteLine("CV测试3.4:\t" + (LPSConvert.DeserializeObject<testigcase>(l).StrengthDrink == 4));

            lps["ABC"].info = "a\\nb,c";
            lps["ABC"].Info = lps["ABC"].Info.Replace(@"\n", "\n").Replace(@"\r", "\r"); ;
            Console.WriteLine("BS测试:\t" + lps["ABC"].GetString());
        }
#pragma warning restore CS8602 // 解引用可能出现空引用。
#pragma warning restore CS8604 // 引用类型参数可能为 null。
        public class testconvert : ConvertFunction
        {
            public override string Convert(dynamic value)
            {
                return "test";
            }

            public override dynamic ConvertBack(string info)
            {
                return "test";
            }
        }
        public class testclass
        {
            [Line(name: "auto", converter: typeof(testconvert))]
            private string nopubstr = "1";
            [Line]
            public string pubstr = "";
            [Line]
            public testclass? tc;
            [Line]
            public int pubint;
            [Line]
            public int getsetint { get; set; }
            [Line]
            public DateTime getsetDateTime { get; set; }
            [Line]
            public List<int>? intlist;
            [Line]
            public int[]? intlistgetset { get; set; }
            [Line]
            public string[]? stringlistgetset { get; set; }
            [Line]
            public Dictionary<int, int>? intdict;
        }

        public class testlpsconv
        {
            [Line(converter: typeof(LPSConvert.ConvertFunction.CF_LPS<LpsDocument>), type: ConvertType.Converter)]
            public LpsDocument TestLps = new LpsDocument();
            [Line(converter: typeof(LPSConvert.ConvertFunction.CF_Sub<Line_D>), type: ConvertType.Converter)]
            public Line_D TestLine = new Line_D();
            [Line(converter: typeof(LPSConvert.ConvertFunction.CF_Sub<Sub>), type: ConvertType.Converter)]
            public ISub TestSub = new Sub();
            public testlpsconv(LpsDocument testlps, ILine line, ISub sub)
            {
                this.TestLps = testlps;
                this.TestLine = new Line_D(line);
                this.TestSub = sub;
            }
            public testlpsconv()
            {
            }
        }

        public class testigcase
        {
            /// <summary>
            /// 食物类型
            /// </summary>
            public enum FoodType
            {
                /// <summary>
                /// 食物 (默认)
                /// </summary>
                Food,
                /// <summary>
                /// 收藏 (自定义)
                /// </summary>
                Star,
                /// <summary>
                /// 正餐
                /// </summary>
                Meal,
                /// <summary>
                /// 零食
                /// </summary>
                Snack,
                /// <summary>
                /// 饮料
                /// </summary>
                Drink,
                /// <summary>
                /// 功能性
                /// </summary>
                Functional,
                /// <summary>
                /// 药品
                /// </summary>
                Drug,
            }
            /// <summary>
            /// 食物类型
            /// </summary>
            [Line(type: ConvertType.ToEnum, ignoreCase: true)]
            public FoodType Type { get; set; } = FoodType.Food;
            /// <summary>
            /// 食物名字
            /// </summary>
            [Line(name: "name")]
            public string? Name { get; set; }
            [Line(ignoreCase: true)]
            public int Exp { get; set; }
            [Line(ignoreCase: true)]
            public double Strength { get; set; }
            [Line(ignoreCase: true)]
            public double StrengthFood { get; set; }
            [Line(ignoreCase: true)]
            public double StrengthDrink { get; set; }
            [Line(ignoreCase: true)]
            public double Feeling { get; set; }
            [Line(ignoreCase: true)]
            public double Health { get; set; }
            [Line(ignoreCase: true)]
            public double Likability { get; set; }
            /// <summary>
            /// 食物价格
            /// </summary>
            [Line(ignoreCase: true)]
            public double Price { get; set; }
            /// <summary>
            /// 描述
            /// </summary>
            [Line(ignoreCase: true)]
            public string? Desc { get; set; }
            /// <summary>
            /// 是否已收藏
            /// </summary>
            public bool Star { get; set; }
            /// <summary>
            /// 物品图片
            /// </summary>
            [Line(ignoreCase: true)]
            public string Image;
        }
    }
}