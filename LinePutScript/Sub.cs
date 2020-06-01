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
            string[] st = lpsSub.Split(new char[1] { '#' }, 2);
            Name = st[0];
            if (st.Length > 1)
                info = st[1];
        }
        /// <summary>
        /// 通过名字和信息创建新的Sub
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="info">信息 (正常版本)</param>
        public Sub(string name, string info)
        {
            Name = name;
            Info = info;
        }

        /// <summary>
        /// 通过Sub创建新的Sub
        /// </summary>
        /// <param name="sub">其他Sub</param>
        public Sub(Sub sub)
        {
            Name = sub.Name;
            info = sub.info;
        }
        /// <summary>
        /// 将其他Sub内容拷贝到本Sub
        /// </summary>
        /// <param name="sub">其他Sub</param>
        public void Set(Sub sub)
        {
            Name = sub.Name;
            info = sub.info;
        }

        /// <summary>
        /// 名称 没有唯一性
        /// </summary>
        public string Name = "lps";
        /// <summary>
        /// 信息 (去除关键字的文本)
        /// </summary>
        public string info = "";
        /// <summary>
        /// 信息 (正常)
        /// </summary>
        public string Info
        {
            get => TextDeReplace(info);
            set
            {
                info = TextReplace(value);
            }
        }
        /// <summary>
        /// 信息 (int)
        /// </summary>
        public int InfoToInt
        {
            get
            {
                int.TryParse(info, out int i);
                return i;
            }
            set
            {
                info = value.ToString();
            }
        }
        /// <summary>
        /// 信息 (int64)
        /// </summary>
        public long InfoToInt64
        {
            get
            {
                long.TryParse(info, out long i);
                return i;
            }
            set
            {
                info = value.ToString();
            }
        }
        /// <summary>
        /// 信息 (double)
        /// </summary>
        public double InfoToDouble
        {
            get
            {
                double.TryParse(info, out double i);
                return i;
            }
            set
            {
                info = value.ToString();
            }
        }
        /// <summary>
        /// 返回循环访问 Info集合 的枚举数。
        /// </summary>
        /// <returns>用于 Info集合 的枚举数</returns>
        public System.Collections.IEnumerator GetEnumerator()
        {
            return GetInfos().GetEnumerator();
        }
        /// <summary>
        /// 返回一个 Info集合 的第一个string。
        /// </summary>
        /// <returns>要返回的第一个string</returns>
        public string First()
        {
            string[] Subs = GetInfos();
            if (Subs.Length == 0)
                return null;
            return Subs[0];
        }
        /// <summary>
        /// 返回一个 Info集合 的最后一个string。
        /// </summary>
        /// <returns>要返回的最后一个string</returns>
        public string Last()
        {
            string[] Subs = GetInfos();
            if (Subs.Length == 0)
                return null;
            return Subs[Subs.Length - 1];
        }
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
        /// 退回Info的反转义文本 (正常显示)
        /// </summary>
        /// <returns>info的反转义文本 (正常显示)</returns>
        public string GetInfo()
        {
            return Info;
        }
        /// <summary>
        /// 退回Info集合的转义文本集合 (正常显示)
        /// </summary>
        /// <returns>info的转义文本集合 (正常显示)</returns>
        public string[] GetInfos()
        {
            string[] sts = info.Split(',');
            for (int i = 0; i < sts.Length; i++)
                sts[i] = TextDeReplace(sts[i]);
            return sts;
        }

        /// <summary>
        /// 将文本进行反转义处理(成为正常显示的文本)
        /// </summary>
        /// <param name="Reptex">要反转义的文本</param>
        /// <returns>反转义后的文本 正常显示的文本</returns>
        public static string TextDeReplace(string Reptex)
        {
            if (Reptex == null)
                return "";
            Reptex = Reptex.Replace("/stop", ":|");
            Reptex = Reptex.Replace("/tab", "\t");
            Reptex = Reptex.Replace("/n", "\n");
            Reptex = Reptex.Replace("/r", "\r");
            Reptex = Reptex.Replace("/id", "#");            
            Reptex = Reptex.Replace("/com", ",");
            Reptex = Reptex.Replace("/!", "/");
            return Reptex;
        }
        /// <summary>
        /// 将文本进行转义处理(成为去除关键字的文本)
        /// </summary>
        /// <param name="Reptex">要转义的文本</param>
        /// <returns>转义后的文本 (去除关键字的文本)</returns>
        public static string TextReplace(string Reptex)
        {
            if (Reptex == null)
                return "";
            Reptex = Reptex.Replace("/", "/!");
            Reptex = Reptex.Replace(":|", "/stop");
            Reptex = Reptex.Replace("\t", "/tab");
            Reptex = Reptex.Replace("\n", "/n");
            Reptex = Reptex.Replace("\r", "/r");
            Reptex = Reptex.Replace("#", "/id");
            Reptex = Reptex.Replace(",", "/com");           
            return Reptex;
        }
        /// <summary>
        /// 将当前Sub转换成文本格式 (info已经被转义/去除关键字)
        /// </summary>
        /// <returns>Sub的文本格式 (info已经被转义/去除关键字)</returns>
        public new string ToString()//不能继承
        {
            if (info == "")
                return TextReplace(Name) + ":|";
            return TextReplace(Name) + '#' + info + ":|";
        }

        //public static string[] Split(string input, string pattern)
        //{
        //    int splwhere = input.IndexOf(pattern);
        //    if (splwhere == -1)
        //        return new string[1] { input };
        //    return new string[2] { input.}
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
