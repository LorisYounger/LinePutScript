using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LinePutScript
{
    /// <summary>
    /// 子类 LinePutScript最基础的类
    /// </summary>
    public class Sub : Sub<SetObject>
    {
        /// <summary>
        /// 创建一个子类
        /// </summary>
        public Sub() :base(){ }
        /// <summary>
        /// 通过lpsSub文本创建一个子类
        /// </summary>
        /// <param name="lpsSub">lpsSub文本</param>
        public Sub(string lpsSub) : base(lpsSub) { }
        /// <summary>
        /// 通过名字和信息创建新的Sub
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="info">信息 (正常版本)</param>
        public Sub(string name, string info) : base(name, info) { }
        /// <summary>
        /// 通过名字和信息创建新的Sub
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="info">多个信息 (正常版本)</param>
        public Sub(string name, params string[] info) : base(name, info) { }
        #region Function
        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="text">需要分割的文本</param>
        /// <param name="separator">分割符号</param>
        /// <param name="count">分割次数 -1 为无限次数</param>
        /// <returns></returns>
        public static List<string> Split(string text, string separator, int count = -1)
        {
            List<string> list = new List<string>();
            string lasttext = text;
            for (int i = 0; i < count || count == -1; i++)
            {
                int iof = lasttext.IndexOf(separator);
                if (iof == -1)
                {
                    break;
                }
                else
                {
                    list.Add(lasttext.Substring(0, iof));
                    lasttext = lasttext.Substring(iof + separator.Length);
                }
            }
            list.Add(lasttext);
            return list;
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
            Reptex = Reptex.Replace("/equ", "=");
            Reptex = Reptex.Replace("/tab", "\t");
            Reptex = Reptex.Replace("/n", "\n");
            Reptex = Reptex.Replace("/r", "\r");
            Reptex = Reptex.Replace("/id", "#");
            Reptex = Reptex.Replace("/com", ",");
            Reptex = Reptex.Replace("/!", "/");
            Reptex = Reptex.Replace("/|", "|");
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
            Reptex = Reptex.Replace("|", "/|");
            Reptex = Reptex.Replace("/", "/!");
            Reptex = Reptex.Replace(":|", "/stop");
            Reptex = Reptex.Replace("\t", "/tab");
            Reptex = Reptex.Replace("\n", "/n");
            Reptex = Reptex.Replace("\r", "/r");
            Reptex = Reptex.Replace("#", "/id");
            Reptex = Reptex.Replace(",", "/com");
            //Reptex = Reptex.Replace("=", "/equ");
            return Reptex;
        }
        /// <summary>
        /// 获取String的HashCode(MD5)
        /// </summary>
        /// <param name="text">String</param>
        /// <returns>HashCode</returns>
        public static long GetHashCode(string text)
        {
            using (MD5 md5 = MD5CryptoServiceProvider.Create())
            {
                return BitConverter.ToInt64(md5.ComputeHash(Encoding.UTF8.GetBytes(text)), 0);
            }
        }
        #endregion
    }
}
