using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinePutScript
{
    /// <summary>
    /// gobject 可以通过强制转换进行更轻松的getset操作
    /// 例: line[(gstr)"subname"]
    /// </summary>
    public class gobj
    {
        /// <summary>
        /// 储存在gobject中的值
        /// </summary>
        public string VALUE;
        /// <summary>
        /// 强制转换gobjcet成string
        /// </summary>
        /// <param name="gobject">gobject</param>
        public static explicit operator string(gobj gobject) => gobject.VALUE;
        /// <summary>
        /// 强制转换string成gobjcet
        /// </summary>
        /// <param name="str">gobject</param>
        public static explicit operator gobj(string str) => new gobj() { VALUE = str };
    }
    /// <summary>
    /// string 可以通过强制转换进行更轻松的getset操作
    /// 例: line[(gstr)"subname"]
    /// </summary>
    public class gstr : gobj { }

    /// <summary>
    /// bool 可以通过强制转换进行更轻松的getset操作
    /// 例: line[(gbol)"subname"]
    /// </summary>
    public class gbol : gobj { }
   
    /// <summary>
    /// int 可以通过强制转换进行更轻松的getset操作
    /// 例: line[(gint)"subname"]
    /// </summary>
    public class gint : gobj { }
  
    /// <summary>
    /// long 可以通过强制转换进行更轻松的getset操作
    /// 例: line[(gi64)"subname"]
    /// </summary>
    public class gi64 : gobj { }

    /// <summary>
    /// double 可以通过强制转换进行更轻松的getset操作
    /// 例: line[(gdbe)"subname"]
    /// </summary>
    public class gdbe : gobj { }
}
