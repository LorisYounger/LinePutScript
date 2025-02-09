using LinePutScript;
using LinePutScript.Converter;
using LinePutScript.Dictionary;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using static LinePutScript.Converter.LPSConvert;
using static TestConsole.Program.Forecast.WeatherCast;
using static TestConsole.Program.MPMessage;

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
                    case "flongspeed":
                    case "fs":
                        flongspeed();
                        break;
                    case "flongtest":
                    case "ft":
                        flongtest();
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
                    case "def":
                        def();
                        break;
                }
            }
        }

        static void def(FInt64 fInt64 = default)
        {
            Console.WriteLine("def:");
            Console.WriteLine(fInt64.ToString());
            Console.WriteLine(fInt64.m_value);

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

            Console.WriteLine("SS测试6.1:\t" + lps.ToString().Equals(Properties.Resources.test2.Replace("\r", "")));

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
                db = 3.1415926,
                db2 = 3.1415926,
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
            //Console.WriteLine(LPSConvert.SerializeObject(tc).ToString());
            Console.WriteLine("CV测试1:\t" + LPSConvert.SerializeObject(tc).ToString().Equals(Properties.Resources.test4.Replace("\r", "")));
            //var obj1 = LPSConvert.DeserializeObject<testclass>(new LpsDocument(Properties.Resources.test4));
            //Console.WriteLine(LPSConvert.SerializeObject(
            //    obj1
            //    ).ToString());
#pragma warning disable CS8604 // 引用类型参数可能为 null。
            Console.WriteLine("CV测试2:\t" + LPSConvert.SerializeObject(
                LPSConvert.DeserializeObject<testclass>(new LpsDocument(Properties.Resources.test4))
                ).ToString().Equals(Properties.Resources.test4.Replace("\r", "")));


#pragma warning restore CS8604 // 引用类型参数可能为 null。           
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

            l = new Line("BetterBuyData:|LastDiscont#0:|Discont:|ScheduleBuyItems:|BuyHistory#/null:|");
            var bbd = LPSConvert.DeserializeObject<BetterBuyData>(l);
            Console.WriteLine("CV测试3.5:\t" + (bbd.ScheduleBuyItems != null));
            Console.WriteLine("CV测试3.6:\t" + (bbd.BuyHistory == null));

            var lnull = LPSConvert.SerializeObject(new List<string>(), convertNoneLineAttribute: true);
            //Console.WriteLine(lnull.ToString());
            var returnlnull = LPSConvert.DeserializeObject<List<string>>(lnull, convertNoneLineAttribute: true);
            Console.WriteLine("CV测试3.7:\t" + (returnlnull.Count == 0));

            lnull = LPSConvert.SerializeObject(new List<string>() { "a", "b", "c" }, convertNoneLineAttribute: true);
            //Console.WriteLine(lnull.ToString());
            returnlnull = LPSConvert.DeserializeObject<List<string>>(lnull, convertNoneLineAttribute: true);
            Console.WriteLine("CV测试3.8:\t" + (returnlnull.Count == 3 && returnlnull[0] == "a"));

            lps["ABC"].info = "a\\nb,c";
            lps["ABC"].Info = lps["ABC"].Info.Replace(@"\n", "\n").Replace(@"\r", "\r"); ;
            Console.WriteLine("BS测试1:\t" + (lps["ABC"].GetString() == "a\nb,c"));
            Console.WriteLine("BS测试2:\t" + (new Line_D("test").GetInt("tt", 60) == 60));

            int hs = lps.GetHashCode();
            int hs2 = lps.GetHashCode();
            Console.WriteLine("HS测试1:\t" + (hs == hs2)); // + lps.ToString());
            Line_D tmr = new Line_D(lps["str"]);
            lps.Remove("str");
            int hs3 = lps.GetHashCode();
            Console.WriteLine("HS测试2:\t" + (hs != hs3));
            lps.Insert(0, tmr);
            int hs4 = lps.GetHashCode();
            Console.WriteLine("HS测试3:\t" + (hs == hs4)); //+ lps.ToString());
            lps.Remove(tmr);
            int hs5 = lps.GetHashCode();
            Console.WriteLine("HS测试4:\t" + (hs != hs5));
            Console.WriteLine("HS测试5:\t" + (hs5 == hs3));

            //测试 Class_C
            var linc = new Line_C<testclass>(tc, "tc");
            Console.WriteLine("CC测试1:\t" + (linc[(gstr)"pubstr"] == "pubtest"));
            Console.WriteLine("CC测试2:\t" + (linc[(gdbe)"db"] == 3.1415926));
            Console.WriteLine("CC测试3:\t" + (linc[(gint)"pubint"] == 20));
            linc[(gint)"pubint"] = 30;
            Console.WriteLine("CC测试4:\t" + (linc[(gint)"pubint"] == 30));
            Console.WriteLine("CC测试5:\t" + (linc.Value.pubint == 30));
            linc.Value.pubint = 40;
            Console.WriteLine("CC测试6:\t" + (linc[(gint)"pubint"] == 40));
            var linec2 = new Line_C<testclass>(new Line(linc.ToString()));
            Console.WriteLine("CC测试7:\t" + (linec2[(gint)"pubint"] == 40));

            lps = new LpsDocument("test#测试注释:|///测试\n///test2#注释失效:|");
            Console.WriteLine("注释测试1:\t" + (lps["test"].Comments == "测试"));
            Console.WriteLine("注释测试2:\t" + (lps[1].Comments == "test2#注释失效:|"));
            Console.WriteLine("注释测试3:\t" + (lps[1].Name != "test2"));

            var gi = new GraphInfo()
            {
                Name = "test",
                Type = GraphInfo.GraphType.Move,
                Animat = GraphInfo.AnimatType.B_Loop
            };
            var gi2 = LPSConvert.SerializeObject(gi, convertNoneLineAttribute: true);
            //Console.WriteLine("VA测试1:\t" + LPSConvert.SerializeObject(gi, convertNoneLineAttribute: true));
            GraphInfo? g2 = DeserializeObject<GraphInfo>(gi2, true);
            Console.WriteLine("VA测试2:\t" + (g2.Name == gi.Name).ToString());
            Console.WriteLine("VA测试3:\t" + (g2.Type == gi.Type).ToString());
            Console.WriteLine("VA测试4:\t" + (g2.Animat == gi.Animat).ToString());

            var mpm = MPMessage.ConverTo(Properties.Resources.test5);
            Console.WriteLine("VA测试5:\t" + (mpm.Type == (int)MPMessage.MSGType.DispayGraph).ToString());
            Console.WriteLine("VA测试6:\t" + (mpm.To == 76561198267979020).ToString());

            var g3 = DeserializeObject<GraphInfo>(new Line(mpm.Content), convertNoneLineAttribute: true);
            Console.WriteLine("VA测试7:\t" + (g3.Name == "workone").ToString());
            Console.WriteLine("VA测试8:\t" + (g3.Type == GraphInfo.GraphType.Work).ToString());
            Console.WriteLine("VA测试9:\t" + (g3.Animat == GraphInfo.AnimatType.A_Start).ToString());

            var wc = JsonConvert.DeserializeObject<WeatherResponse>(@"{""status"":200,""count"":2,""info"":""OK"",""infocode"":0,""forecasts"":[{""city"":""香港特别行政区"",""adcode"":810000,""province"":""香港"",""reporttime"":""2025-01-26T15:32:53"",""casts"":[{""date"":""2025-01-26T00:00:00"",""dayweather"":3,""nightweather"":3,""daywind"":2,""nightwind"":2,""daypower"":""5"",""nightpower"":""5"",""daytemp_float"":18.0,""nighttemp_float"":13.0},{""date"":""2025-01-27T00:00:00"",""dayweather"":0,""nightweather"":0,""daywind"":8,""nightwind"":8,""daypower"":""5"",""nightpower"":""5"",""daytemp_float"":16.0,""nighttemp_float"":11.0},{""date"":""2025-01-28T00:00:00"",""dayweather"":0,""nightweather"":0,""daywind"":1,""nightwind"":1,""daypower"":""4"",""nightpower"":""4"",""daytemp_float"":18.0,""nighttemp_float"":12.0},{""date"":""2025-01-29T00:00:00"",""dayweather"":0,""nightweather"":0,""daywind"":1,""nightwind"":1,""daypower"":""4"",""nightpower"":""4"",""daytemp_float"":18.0,""nighttemp_float"":13.0}]}],""lives"":[{""province"":""香港"",""city"":""香港特别行政区"",""adcode"":810000,""weather"":4,""winddirection"":2,""windpower"":""6"",""reporttime"":""2025-01-26T15:32:52"",""temperature_float"":14.0,""humidity_float"":50.0}]}");
            var wclps = LPSConvert.SerializeObject(wc, convertNoneLineAttribute: true);
            Console.WriteLine("WC测试1:\t" + (wclps[(gint)"Status"] == 200));
            Console.WriteLine("WC测试2:\t" + (wclps[(gint)"Count"] == 2));
            Console.WriteLine("WC测试3:\t" + (wclps[(gstr)"Info"] == "OK"));
            Console.WriteLine("WC测试4:\t" + (wclps[(gint)"InfoCode"] == 0));
            var wcback = LPSConvert.DeserializeObject<WeatherResponse>(wclps, convertNoneLineAttribute: true);
            Console.WriteLine("WC测试5:\t" + (wcback.Status == 200));
            Console.WriteLine("WC测试6:\t" + (wcback.Count == 2));
            Console.WriteLine("WC测试7:\t" + (wcback.Info == "OK"));
            Console.WriteLine("WC测试8:\t" + (wcback.Infocode == 0));
            Console.WriteLine("WC测试9:\t" + (wcback.Forecasts[0].City == "香港特别行政区"));
            Console.WriteLine("WC测试10:\t" + (wcback.Forecasts[0].Adcode == 810000));
            Console.WriteLine("WC测试11:\t" + (wcback.Forecasts[0].Province == "香港"));
            Console.WriteLine("WC测试12:\t" + (wcback.Forecasts[0].Casts.Count == wc.Forecasts[0].Casts.Count));
            Console.WriteLine("WC测试13:\t" + (wcback.Forecasts[0].Casts[0].Date == wc.Forecasts[0].Casts[0].Date));
            Console.WriteLine("WC测试14:\t" + (wcback.Forecasts[0].Casts[0].DayPower == wc.Forecasts[0].Casts[0].DayPower));

        }
#pragma warning restore CS8602 // 解引用可能出现空引用。
#pragma warning restore CS8604 // 引用类型参数可能为 null。
#pragma warning disable CS8981 // 该类型名称仅包含小写 ascii 字符。此类名称可能会成为该语言的保留值。
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
            public double db;
            [Line(Type = ConvertType.ToFloat)]
            public double db2;
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
            public string? Image;
        }

        static void flongspeed()
        {
            // 创建一些测试值
            FInt64 fI64A = FInt64.FromNumberDouble(1.23);
            FInt64 fI64B = FInt64.FromNumberDouble(4.56);
            double doubleA = 1.23;
            double doubleB = 4.56;
            long longA = fI64A.m_value;
            long longB = fI64B.m_value;
            db2 dbA = new db2(doubleA);
            db2 dbB = new db2(doubleB);
            int intA = 123;
            int intB = 456;
            float floatA = 1.23f;
            float floatB = 4.56f;
            decimal decimalA = 1.23m;
            decimal decimalB = 4.56m;
            BigInteger bigIntegerA = new BigInteger(123);
            BigInteger bigIntegerB = new BigInteger(456);
            FInt64 fdoubleA = new FInt64(doubleA);
            FInt64 fdoubleB = new FInt64(doubleB);
            SetObject setA = new SetObject(doubleA);
            SetObject setB = new SetObject(doubleB);

            // 创建一个 Stopwatch 对象来测量时间
            Stopwatch stopwatch = new Stopwatch();

            // 测试 FInt64 的速度
            stopwatch.Start();
            for (int i = 0; i < 100000000; i++)
            {
                FInt64 result = fI64A + fI64B;
                result = fI64A - fI64B;
                result = fI64A * fI64B;
                result = fI64A / fI64B;
            }
            stopwatch.Stop();
            Console.WriteLine($"fI64 time: {stopwatch.ElapsedMilliseconds} ms");

            // 重置 Stopwatch
            stopwatch.Reset();

            // 测试 double 的速度
            stopwatch.Start();
            for (int i = 0; i < 100000000; i++)
            {
                double result = doubleA + doubleB;
                result = doubleA - doubleB;
                result = doubleA * doubleB;
                result = doubleA / doubleB;
            }
            stopwatch.Stop();
            Console.WriteLine($"double time: {stopwatch.ElapsedMilliseconds} ms");

            // 重置 Stopwatch
            stopwatch.Reset();
            stopwatch.Start();
            for (int i = 0; i < 100000000; i++)
            {
                long result = longA + longB;
                result = longA - longB;
                result = longA * longB;
                result = longA / longB;
            }
            stopwatch.Stop();
            Console.WriteLine($"long time: {stopwatch.ElapsedMilliseconds} ms");

            // 重置 Stopwatch
            stopwatch.Reset();
            stopwatch.Start();
            for (int i = 0; i < 100000000; i++)
            {
                db2 result = dbA + dbB;
                result = dbA - dbB;
                result = dbA * dbB;
                result = dbA / dbB;
            }
            stopwatch.Stop();
            Console.WriteLine($"db2 time: {stopwatch.ElapsedMilliseconds} ms");
            // 重置 Stopwatch
            stopwatch.Reset();
            stopwatch.Start();
            for (int i = 0; i < 100000000; i++)
            {
                int result = intA + intB;
                result = intA - intB;
                result = intA * intB;
                result = intA / intB;
            }
            stopwatch.Stop();
            Console.WriteLine($"int time: {stopwatch.ElapsedMilliseconds} ms");
            // 重置 Stopwatch
            stopwatch.Reset();
            stopwatch.Start();
            for (int i = 0; i < 100000000; i++)
            {
                float result = floatA + floatB;
                result = floatA - floatB;
                result = floatA * floatB;
                result = floatA / floatB;
            }
            stopwatch.Stop();
            Console.WriteLine($"float time: {stopwatch.ElapsedMilliseconds} ms");
            stopwatch.Reset();
            stopwatch.Start();
            for (int i = 0; i < 100000000; i++)
            {
                decimal result = decimalA + decimalB;
                result = decimalA - decimalB;
                result = decimalA * decimalB;
                result = decimalA / decimalB;
            }
            stopwatch.Stop();
            Console.WriteLine($"decimal time: {stopwatch.ElapsedMilliseconds} ms");
            stopwatch.Reset();
            stopwatch.Start();
            for (int i = 0; i < 100000000; i++)
            {
                BigInteger result = bigIntegerA + bigIntegerB;
                result = bigIntegerA - bigIntegerB;
                result = bigIntegerA * bigIntegerB;
                result = bigIntegerA / bigIntegerB;
            }
            stopwatch.Stop();
            Console.WriteLine($"BigInteger time: {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Reset();
            stopwatch.Start();
            for (int i = 0; i < 100000000; i++)
            {
                FInt64 result = fdoubleA + fdoubleB;
                result = fdoubleA - fdoubleB;
                result = fdoubleA * fdoubleB;
                result = fdoubleA / fdoubleB;
            }
            stopwatch.Stop();
            Console.WriteLine($"FInt64 time: {stopwatch.ElapsedMilliseconds} ms");
            stopwatch.Reset();
            stopwatch.Start();
            for (int i = 0; i < 100000000; i++)
            {
                SetObject result = setA + setB;
            }
            stopwatch.Stop();
            Console.WriteLine($"SetObject time: {stopwatch.ElapsedMilliseconds} ms");

        }
        public readonly struct db2
        {
            public readonly double value = 0;
            public db2(double value)
            {
                this.value = value;
            }
            public static db2 operator +(db2 a, db2 b)
            {
                return new db2(a.value + b.value);
            }
            public static db2 operator -(db2 a, db2 b)
            {
                return new db2(a.value - b.value);
            }
            public static db2 operator *(db2 a, db2 b)
            {
                return new db2(a.value * b.value);
            }
            public static db2 operator /(db2 a, db2 b)
            {
                return new db2(a.value / b.value);
            }
        }
        static void flongtest()
        {
            Console.WriteLine("欢迎使用 FInt64 测试控制台");
            Console.WriteLine("测试FI64");
            for (int i = 0; i < 10; i++)
            {
                // 创建一些测试值
                Random random = new Random();
                double doubleA = random.NextDouble() * random.Next(-1000, 1000);
                double doubleB = random.NextDouble() * random.Next(-1000, 1000);
                FInt64 fdoubleA = new FInt64(doubleA);
                FInt64 fdoubleB = new FInt64(doubleB);

                // 测试加法
                FInt64 fdoubleAddResult = fdoubleA + fdoubleB;
                double doubleAddResult = doubleA + doubleB;
                if (Math.Abs((double)(fdoubleAddResult - doubleAddResult)) > 0.0000001)
                {
                    Console.WriteLine($"ADD FInt64 = {fdoubleAddResult}, double = {doubleAddResult} (Error)");
                }
                // 测试减法
                FInt64 fdoubleSubResult = fdoubleA - fdoubleB;
                double doubleSubResult = doubleA - doubleB;
                if (Math.Abs((double)(fdoubleSubResult - doubleSubResult)) > 0.0000001)
                {
                    Console.WriteLine($"SUB FInt64 = {fdoubleSubResult}, double = {doubleSubResult} (Error)");
                }
                // 测试乘法
                FInt64 fdoubleMulResult = fdoubleA * fdoubleB;
                double doubleMulResult = doubleA * doubleB;
                if (Math.Abs((double)(fdoubleMulResult - doubleMulResult)) > 0.01)
                {
                    Console.WriteLine($"MUL FInt64 = {fdoubleMulResult}, double = {doubleMulResult} (Error)");
                }
                // 测试除法
                FInt64 fdoubleDivResult = fdoubleA / fdoubleB;
                double doubleDivResult = doubleA / doubleB;
                if (Math.Abs((double)(fdoubleDivResult - doubleDivResult)) > 0.0001)
                {
                    Console.WriteLine($"DIV FInt64 = {fdoubleDivResult}, double = {doubleDivResult} (Error)");
                }
                // 测试比较
                int fdoubleCompareResult = fdoubleA.CompareTo(fdoubleB);
                int doubleCompareResult = doubleA.CompareTo(doubleB);
                if (Math.Abs((double)(fdoubleCompareResult - doubleCompareResult)) > 0.0001)
                {
                    Console.WriteLine($"COM FInt64 = {fdoubleCompareResult}, double = {doubleCompareResult} (Error)");
                }
                // 测试等于
                bool fdoubleEqualsResult = fdoubleA.Equals(fdoubleB);
                bool doubleEqualsResult = doubleA.Equals(doubleB);
                if (fdoubleEqualsResult != doubleEqualsResult)
                {
                    Console.WriteLine($"EQU FInt64 = {fdoubleEqualsResult}, double = {doubleEqualsResult} (Error)");
                }
            }
            Console.WriteLine("测试FI64+DBL");
            for (int i = 0; i < 10; i++)
            {
                // 创建一些测试值
                Random random = new Random();
                double doubleA = random.NextDouble() * random.Next(0, 100);
                double doubleB = random.NextDouble() * random.Next(0, 100);
                FInt64 fdoubleA = new FInt64(doubleA);
                double fdoubleB = doubleB;

                // 测试加法
                FInt64 fdoubleAddResult = fdoubleA + fdoubleB;
                double doubleAddResult = doubleA + doubleB;
                if (Math.Abs((double)(fdoubleAddResult - doubleAddResult)) > 0.0000001)
                {
                    Console.WriteLine($"ADD FInt64 = {fdoubleAddResult}, double = {doubleAddResult} (Error)");
                }
                // 测试减法
                FInt64 fdoubleSubResult = fdoubleA - fdoubleB;
                double doubleSubResult = doubleA - doubleB;
                if (Math.Abs((double)(fdoubleSubResult - doubleSubResult)) > 0.0000001)
                {
                    Console.WriteLine($"SUB FInt64 = {fdoubleSubResult}, double = {doubleSubResult} (Error)");
                }
                // 测试乘法
                FInt64 fdoubleMulResult = fdoubleA * fdoubleB;
                double doubleMulResult = doubleA * doubleB;
                if (Math.Abs((double)(fdoubleMulResult - doubleMulResult)) > 0.01)
                {
                    Console.WriteLine($"MUL FInt64 = {fdoubleMulResult}, double = {doubleMulResult} (Error)");
                }
                // 测试除法
                FInt64 fdoubleDivResult = fdoubleA / fdoubleB;
                double doubleDivResult = doubleA / doubleB;
                if (Math.Abs((double)(fdoubleDivResult - doubleDivResult)) > 0.0001)
                {
                    Console.WriteLine($"DIV FInt64 = {fdoubleDivResult}, double = {doubleDivResult} (Error)");
                }
                // 测试比较
                int fdoubleCompareResult = fdoubleA.CompareTo(fdoubleB);
                int doubleCompareResult = doubleA.CompareTo(doubleB);
                if (Math.Abs((double)(fdoubleCompareResult - doubleCompareResult)) > 0.0001)
                {
                    Console.WriteLine($"COM FInt64 = {fdoubleCompareResult}, double = {doubleCompareResult} (Error)");
                }
                // 测试等于
                bool fdoubleEqualsResult = fdoubleA.Equals(fdoubleB);
                bool doubleEqualsResult = doubleA.Equals(doubleB);
                if (fdoubleEqualsResult != doubleEqualsResult)
                {
                    Console.WriteLine($"EQU FInt64 = {fdoubleEqualsResult}, double = {doubleEqualsResult} (Error)");
                }
            }
            Console.WriteLine("测试DBL+FI64");
            for (int i = 0; i < 10; i++)
            {
                // 创建一些测试值
                Random random = new Random();
                double doubleA = random.NextDouble() * random.Next(0, 100);
                double doubleB = random.NextDouble() * random.Next(0, 100);
                double fdoubleA = doubleA;
                FInt64 fdoubleB = new FInt64(doubleB);

                // 测试加法
                FInt64 fdoubleAddResult = fdoubleA + fdoubleB;
                double doubleAddResult = doubleA + doubleB;
                if (Math.Abs((double)(fdoubleAddResult - doubleAddResult)) > 0.0000001)
                {
                    Console.WriteLine($"ADD FInt64 = {fdoubleAddResult}, double = {doubleAddResult} (Error)");
                }
                // 测试减法
                FInt64 fdoubleSubResult = fdoubleA - fdoubleB;
                double doubleSubResult = doubleA - doubleB;
                if (Math.Abs((double)(fdoubleSubResult - doubleSubResult)) > 0.0000001)
                {
                    Console.WriteLine($"SUB FInt64 = {fdoubleSubResult}, double = {doubleSubResult} (Error)");
                }
                // 测试乘法
                FInt64 fdoubleMulResult = fdoubleA * fdoubleB;
                double doubleMulResult = doubleA * doubleB;
                if (Math.Abs((double)(fdoubleMulResult - doubleMulResult)) > 0.01)
                {
                    Console.WriteLine($"MUL FInt64 = {fdoubleMulResult}, double = {doubleMulResult} (Error)");
                }
                // 测试除法
                FInt64 fdoubleDivResult = fdoubleA / fdoubleB;
                double doubleDivResult = doubleA / doubleB;
                if (Math.Abs((double)(fdoubleDivResult - doubleDivResult)) > 0.0001)
                {
                    Console.WriteLine($"DIV FInt64 = {fdoubleDivResult}, double = {doubleDivResult} (Error)");
                }
            }
            Console.WriteLine("测试结束");
        }

        /// <summary>
        /// 动画信息
        /// </summary>
        /// 新版本动画类型是根据整体类型+名字定义而成
        /// 动画类型->动画名字
        /// 动画名字->状态+动作->动画
        /// 类型: 主要动作分类
        /// 动画名字: 用户自定义, 同名字动画支持相同随机,不再使用StoreRand
        /// 动作: 动画的动作 Start Loop End
        /// 状态: 动画的状态 Save.GameSave.ModeType
        public class GraphInfo
        {
            /// <summary>
            /// 用于Convert的空动画信息
            /// </summary>
            public GraphInfo()
            {

            }
            /// <summary>
            /// 类型: 主要动作分类
            /// </summary>
            /// * 为必须有的动画
            public enum GraphType
            {
                /// <summary>
                /// 通用动画,用于被被其他动画调用或者mod等用途
                /// </summary>
                /// 不被默认启用/使用的 不包含在GrapType
                Common,
                /// <summary>
                /// 被提起动态 *
                /// </summary>
                Raised_Dynamic,
                /// <summary>
                /// 被提起静态 (开始&循环&结束) *
                /// </summary>
                Raised_Static,
                /// <summary>
                /// 现在所有会动的东西都是MOVE
                /// </summary>
                Move,
                /// <summary>
                /// 呼吸 *
                /// </summary>
                Default,
                /// <summary>
                /// 摸头 (开始&循环&结束)
                /// </summary>
                Touch_Head,
                /// <summary>
                /// 摸身体 (开始&循环&结束)
                /// </summary>
                Touch_Body,
                /// <summary>
                /// 空闲 (包括下蹲/无聊等通用空闲随机动画) (开始&循环&结束)
                /// </summary>
                Idel,
                /// <summary>
                /// 睡觉 (开始&循环&结束) *
                /// </summary>
                Sleep,
                /// <summary>
                /// 说话 (开始&循环&结束) *
                /// </summary>
                Say,
                /// <summary>
                /// 待机 模式1 (开始&循环&结束)
                /// </summary>
                StateONE,
                /// <summary>
                /// 待机 模式2 (开始&循环&结束)
                /// </summary>
                StateTWO,
                /// <summary>
                /// 开机 *
                /// </summary>
                StartUP,
                /// <summary>
                /// 关机
                /// </summary>
                Shutdown,
                /// <summary>
                /// 工作 (开始&循环&结束) *
                /// </summary>
                Work,
                /// <summary>
                /// 向上切换状态
                /// </summary>
                Switch_Up,
                /// <summary>
                /// 向下切换状态
                /// </summary>
                Switch_Down,
                /// <summary>
                /// 口渴
                /// </summary>
                Switch_Thirsty,
                /// <summary>
                /// 饥饿
                /// </summary>
                Switch_Hunger,
            }
            /// <summary>
            /// 动作: 动画的动作 Start Loop End
            /// </summary>
            public enum AnimatType
            {
                /// <summary>
                /// 动画只有一个动作
                /// </summary>
                Single,
                /// <summary>
                /// 开始动作
                /// </summary>
                A_Start,
                /// <summary>
                /// 循环动作
                /// </summary>
                B_Loop,
                /// <summary>
                /// 结束动作
                /// </summary>
                C_End,
            }
            /// <summary>
            /// 动画名字: 用户自定义 同名字动画支持相同随机,不再使用StoreRand
            /// </summary>
            public string Name { get; set; } = "";
            /// <summary>
            /// 动作: 动画的动作 Start Loop End
            /// </summary>
            public AnimatType Animat { get; set; } = AnimatType.Single;
            /// <summary>
            /// 类型: 主要动作分类
            /// </summary>
            public GraphType Type { get; set; } = GraphType.Common;
        }
        public class BetterBuyData
        {
            /// <summary>
            /// 上次折扣时间
            /// </summary>
            [Line] public DateTime LastDiscont = DateTime.MinValue;

            /// <summary>
            /// 折扣数据
            /// </summary>
            [Line] public Dictionary<string, int> Discont = new Dictionary<string, int>();

            /// <summary>
            /// 定时购买的商品
            /// </summary>
            [Line] public ObservableCollection<ScheduleBuyItem> ScheduleBuyItems = new ObservableCollection<ScheduleBuyItem>();

            /// <summary>
            /// 购买历史
            /// </summary>
            [Line] public ObservableCollection<HistoryBuyItem> BuyHistory = new ObservableCollection<HistoryBuyItem>();

            /// <summary>
            /// 定时购买
            /// </summary>
            public class ScheduleBuyItem
            {
                /// <summary>
                /// 更好买物品
                /// </summary>
                [Line] public string SalabilityItemName { get; set; }

                /// <summary>
                /// 数量
                /// </summary>
                [Line] public int Quantity { get; set; }

                /// <summary>
                /// 频次
                /// </summary>
                public enum BuyFrequency
                {
                    None = 0,
                    EveryDay = 1,
                    EveryWeek = 7,
                    EveryMonth = 30,
                }
                /// <summary>
                /// 购买频率
                /// </summary>
                [Line] public BuyFrequency Frequency { get; set; }

                public int Frequency_id
                {
                    get
                    {
                        switch (Frequency)
                        {
                            default:
                            case BuyFrequency.None:
                                return 0;
                            case BuyFrequency.EveryDay:
                                return 1;
                            case BuyFrequency.EveryWeek:
                                return 2;
                            case BuyFrequency.EveryMonth:
                                return 3;
                        }
                    }
                    set
                    {
                        switch (value)
                        {
                            default:
                            case 0:
                                Frequency = BuyFrequency.None;
                                break;
                            case 1:
                                Frequency = BuyFrequency.EveryDay;
                                break;
                            case 2:
                                Frequency = BuyFrequency.EveryWeek;
                                break;
                            case 3:
                                Frequency = BuyFrequency.EveryMonth;
                                break;
                        }
                    }
                }
            }
            /// <summary>
            /// 购买历史
            /// </summary>
            public class HistoryBuyItem
            {
                /// <summary>
                /// 更好买物品
                /// </summary>
                [Line] public string SalabilityItemName { get; set; }
                /// <summary>
                /// 数量
                /// </summary>
                [Line] public int Quantity { get; set; }

                /// <summary>
                /// 金额
                /// </summary>
                [Line] public double Amount { get; set; }

                /// <summary>
                /// 购买日期
                /// </summary>
                [Line] public DateTime BuyTime { get; set; }
            }
        }
        /// <summary>
        /// 多人模式传输的消息
        /// </summary>
        public struct MPMessage
        {
            /// <summary>
            /// 消息类型
            /// </summary>
            public enum MSGType
            {
                /// <summary>
                /// 一般是出错或者空消息
                /// </summary>
                Empty,
                /// <summary>
                /// 聊天消息 (chat)
                /// </summary>
                Chat,
                /// <summary>
                /// 显示动画 (graphinfo)
                /// </summary>
                DispayGraph,
                /// <summary>
                /// 交互 (Interact)
                /// </summary>
                Interact,
                /// <summary>
                /// 喂食 (Feed)
                /// </summary>
                Feed,
            }
            /// <summary>
            /// 消息类型 MOD作者可以随便抽个不是MSGTYPE的数避免冲突,支持负数
            /// </summary>
            [Line] public int Type { get; set; }

            /// <summary>
            /// 消息内容
            /// </summary>
            [Line] public string Content { get; set; }
            /// <summary>
            /// 被操作者 (显示动画用)
            /// </summary>
            [Line] public ulong To { get; set; }

            public static string ConverTo(MPMessage data) => LPSConvert.SerializeObject(data).ToString();
            public static MPMessage ConverTo(string data)
            {
                var lps = new LPS(data);
                return LPSConvert.DeserializeObject<MPMessage>(lps);
            }
        }
        /// <summary>
        /// 表示地点响应的模型类
        /// </summary>
        public class LocationResponse
        {
            /// <summary>
            /// 响应状态码，通常为整数
            /// </summary>
            public int Status { get; set; }

            /// <summary>
            /// 响应信息，描述请求的结果
            /// </summary>
            public string Info { get; set; } = string.Empty;

            /// <summary>
            /// 信息代码，通常为整数，用于进一步的状态指示
            /// </summary>
            public int Infocode { get; set; }

            /// <summary>
            /// 省份名称
            /// </summary>
            public string Province { get; set; } = string.Empty;

            /// <summary>
            /// 城市名称
            /// </summary>
            public string City { get; set; } = string.Empty;

            /// <summary>
            /// 区域代码，通常为整数
            /// </summary>
            public int Adcode { get; set; }

            /// <summary>
            /// 矩形区域的坐标，格式为 "西南角经度,西南角纬度;东北角经度,东北角纬度"
            /// </summary>
            public string Rectangle { get; set; } = string.Empty;
        }
        /// <summary>
        /// 表示天气预报的响应模型类
        /// </summary>
        public class WeatherResponse
        {
            /// <summary>
            /// 响应状态码
            /// </summary>
            [JsonProperty("status")]
            public int Status { get; set; }

            /// <summary>
            /// 预报数量
            /// </summary>
            [JsonProperty("count")]
            public int Count { get; set; }

            /// <summary>
            /// 响应信息
            /// </summary>
            [JsonProperty("info")]
            public string Info { get; set; } = string.Empty;

            /// <summary>
            /// 信息代码
            /// </summary>
            [JsonProperty("infocode")] public int Infocode { get; set; }

            /// <summary>
            /// 预报列表
            /// </summary>
            [JsonProperty("forecasts")]
            public List<Forecast> Forecasts { get; set; } = new List<Forecast>();

            /// <summary>
            /// 当前天气信息列表
            /// </summary>
            [JsonProperty("lives")]
            public List<CurrentWeather> Lives { get; set; } = new List<CurrentWeather>();
        }

        /// <summary>
        /// 表示单个天气预报的模型类
        /// </summary>
        public class Forecast
        {
            /// <summary>
            /// 城市名称
            /// </summary>
            [JsonProperty("city")]
            public string City { get; set; } = string.Empty;

            /// <summary>
            /// 区域代码
            /// </summary>
            [JsonProperty("adcode")]
            public int Adcode { get; set; }

            /// <summary>
            /// 省份名称
            /// </summary>
            [JsonProperty("province")]
            public string Province { get; set; } = string.Empty;

            /// <summary>
            /// 报告时间
            /// </summary>
            [JsonProperty("reporttime")]
            public DateTime ReportTime { get; set; }

            /// <summary>
            /// 天气预报列表
            /// </summary>
            [JsonProperty("casts")]
            public List<WeatherCast> Casts { get; set; } = new List<WeatherCast>();

            /// <summary>
            /// 表示单个天气情况的模型类
            /// </summary>
            public class WeatherCast
            {
                /// <summary>
                /// 日期
                /// </summary>
                [JsonProperty("date")]
                public DateTime Date { get; set; }

                /// <summary>
                /// 白天天气情况
                /// </summary>
                [JsonProperty("dayweather")]
                public WeatherCondition DayWeather { get; set; }

                /// <summary>
                /// 晚上天气情况
                /// </summary>
                [JsonProperty("nightweather")]
                public WeatherCondition NightWeather { get; set; }

                /// <summary>
                /// 白天风向
                /// </summary>
                [JsonProperty("daywind")]
                public WindDirection DayWind { get; set; }

                /// <summary>
                /// 晚上风向
                /// </summary>
                [JsonProperty("nightwind")]
                public WindDirection NightWind { get; set; }

                /// <summary>
                /// 白天风力
                /// </summary>
                [JsonProperty("daypower")]
                public string DayPower { get; set; } = string.Empty;

                /// <summary>
                /// 晚上风力
                /// </summary>
                [JsonProperty("nightpower")]
                public string NightPower { get; set; } = string.Empty;

                /// <summary>
                /// 白天温度（浮动）
                /// </summary>
                [JsonProperty("daytemp_float")]
                public double DayTempFloat { get; set; }

                /// <summary>
                /// 晚上温度（浮动）
                /// </summary>
                [JsonProperty("nighttemp_float")]
                public double NightTempFloat { get; set; }

                /// <summary>
                /// 天气现象枚举
                /// </summary>
                public enum WeatherCondition
                {
                    晴, 少云, 晴间多云, 多云, 阴, 有风, 平静, 微风, 和风, 清风,
                    强风, 疾风, 大风, 烈风, 风暴, 狂爆风, 飓风, 热带风暴, 霾,
                    中度霾, 重度霾, 严重霾, 阵雨, 雷阵雨, 雷阵雨并伴有冰雹,
                    小雨, 中雨, 大雨, 暴雨, 大暴雨, 特大暴雨, 强阵雨, 强雷阵雨,
                    极端降雨, 毛毛雨, 雨, 小雨中雨, 中雨大雨, 大雨暴雨,
                    暴雨大暴雨, 大暴雨特大暴雨, 雨雪天气, 雨夹雪, 阵雨夹雪,
                    冻雨, 雪, 阵雪, 小雪, 中雪, 大雪, 暴雪, 小雪中雪,
                    中雪大雪, 大雪暴雪, 浮尘, 扬沙, 沙尘暴, 强沙尘暴,
                    龙卷风, 雾, 浓雾, 强浓雾, 轻雾, 大雾, 特强浓雾, 热, 冷, 未知
                }

                /// <summary>
                /// 风向枚举
                /// </summary>
                public enum WindDirection
                {
                    无风向, 东北, 东, 东南, 南, 西南, 西, 西北, 北, 旋转不定
                }
            }
        }


        /// <summary>
        /// 表示单个城市当前天气的模型类
        /// </summary>
        public class CurrentWeather
        {
            /// <summary>
            /// 省份名称
            /// </summary>
            [JsonProperty("province")]
            public string Province { get; set; } = string.Empty;

            /// <summary>
            /// 城市名称
            /// </summary>
            [JsonProperty("city")]
            public string City { get; set; } = string.Empty;

            /// <summary>
            /// 区域代码
            /// </summary>
            [JsonProperty("adcode")]
            public int Adcode { get; set; }

            /// <summary>
            /// 当前天气情况
            /// </summary>
            [JsonProperty("weather")]
            public WeatherCondition Weather { get; set; }


            /// <summary>
            /// 风向
            /// </summary>
            [JsonProperty("winddirection")]
            public WindDirection WindDirection { get; set; }

            /// <summary>
            /// 风力
            /// </summary>
            [JsonProperty("windpower")]
            public string WindPower { get; set; } = string.Empty;

            /// <summary>
            /// 报告时间
            /// </summary>
            [JsonProperty("reporttime")]
            public DateTime ReportTime { get; set; }

            /// <summary>
            /// 温度（浮动）
            /// </summary>
            [JsonProperty("temperature_float")]
            public double TemperatureFloat { get; set; }

            /// <summary>
            /// 湿度（浮动）
            /// </summary>
            [JsonProperty("humidity_float")]
            public double HumidityFloat { get; set; }
        }
    }
}