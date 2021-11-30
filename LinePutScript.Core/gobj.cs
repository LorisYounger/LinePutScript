using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinePutScript
{

    // Get Object 可以通过强制转换进行更轻松的getset操作

    /// <summary>
    /// string 可以通过强制转换进行更轻松的getset操作
    /// 例: line[(gstr)"subname"]
    /// </summary>
    public class gstr
    {
        /// <summary>
        /// 储存在gobject中的值
        /// </summary>
        public string VALUE;
        /// <summary>
        /// 强制转换gobjcet成string
        /// </summary>
        /// <param name="gobject">gobject</param>
        public static explicit operator string(gstr gobject) => gobject.VALUE;
        /// <summary>
        /// 强制转换string成gobjcet
        /// </summary>
        /// <param name="str">string</param>
        public static explicit operator gstr(string str) => new gstr() { VALUE = str };
    }

    /// <summary>
    /// bool 可以通过强制转换进行更轻松的getset操作
    /// 例: line[(gbol)"subname"]
    /// </summary>
    public class gbol
    {
        /// <summary>
        /// 储存在gobject中的值
        /// </summary>
        public string VALUE;
        /// <summary>
        /// 强制转换gobjcet成string
        /// </summary>
        /// <param name="gobject">gobject</param>
        public static explicit operator string(gbol gobject) => gobject.VALUE;
        /// <summary>
        /// 强制转换string成gobjcet
        /// </summary>
        /// <param name="str">string</param>
        public static explicit operator gbol(string str) => new gbol() { VALUE = str };
    }

    /// <summary>
    /// int 可以通过强制转换进行更轻松的getset操作
    /// 例: line[(gint)"subname"]
    /// </summary>
    public class gint
    {
        /// <summary>
        /// 储存在gobject中的值
        /// </summary>
        public string VALUE;
        /// <summary>
        /// 强制转换gobjcet成string
        /// </summary>
        /// <param name="gobject">gobject</param>
        public static explicit operator string(gint gobject) => gobject.VALUE;
        /// <summary>
        /// 强制转换string成gobjcet
        /// </summary>
        /// <param name="str">string</param>
        public static explicit operator gint(string str) => new gint() { VALUE = str };
    }

    /// <summary>
    /// long 可以通过强制转换进行更轻松的getset操作
    /// 例: line[(gi64)"subname"]
    /// </summary>
    public class gi64
    {
        /// <summary>
        /// 储存在gobject中的值
        /// </summary>
        public string VALUE;
        /// <summary>
        /// 强制转换gobjcet成string
        /// </summary>
        /// <param name="gobject">gobject</param>
        public static explicit operator string(gi64 gobject) => gobject.VALUE;
        /// <summary>
        /// 强制转换string成gobjcet
        /// </summary>
        /// <param name="str">string</param>
        public static explicit operator gi64(string str) => new gi64() { VALUE = str };
    }

    /// <summary>
    /// double 可以通过强制转换进行更轻松的getset操作
    /// 例: line[(gdbe)"subname"]
    /// </summary>
    public class gdbe
    {
        /// <summary>
        /// 储存在gobject中的值
        /// </summary>
        public string VALUE;
        /// <summary>
        /// 强制转换gobjcet成string
        /// </summary>
        /// <param name="gobject">gobject</param>
        public static explicit operator string(gdbe gobject) => gobject.VALUE;
        /// <summary>
        /// 强制转换string成gobjcet
        /// </summary>
        /// <param name="str">string</param>
        public static explicit operator gdbe(string str) => new gdbe() { VALUE = str };
    }
}
