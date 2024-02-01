using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinePutScript
{

    /// <summary>
    /// Get Object 可以通过强制转换进行更轻松的getset操作
    /// </summary>
    public abstract class gobj
    {
        /// <summary>
        /// 储存在gobject中的值
        /// </summary>
        public string VALUE;
        /// <summary>
        /// 生成 Gobj
        /// </summary>
        public gobj(string value) => VALUE = value;
    }
    /// <summary>
    /// string 可以通过强制转换进行更轻松的getset操作
    /// 例: line[(gstr)"subname"]
    /// </summary>
    public class gstr : gobj
    { /// <summary>
      /// 生成 Gobj
      /// </summary>
        public gstr(string value) : base(value)
        {
        }

        /// <summary>
        /// 强制转换gobjcet成string
        /// </summary>
        /// <param name="gobject">gobject</param>
        public static explicit operator string(gstr gobject) => gobject.VALUE;
        /// <summary>
        /// 强制转换string成gobjcet
        /// </summary>
        /// <param name="str">string</param>
        public static explicit operator gstr(string str) => new gstr(str);
    }

    /// <summary>
    /// bool 可以通过强制转换进行更轻松的getset操作
    /// 例: line[(gbol)"subname"]
    /// </summary>
    public class gbol : gobj
    { /// <summary>
      /// 生成 Gobj
      /// </summary>
        public gbol(string value) : base(value)
        {
        }

        /// <summary>
        /// 强制转换gobjcet成string
        /// </summary>
        /// <param name="gobject">gobject</param>
        public static explicit operator string(gbol gobject) => gobject.VALUE;
        /// <summary>
        /// 强制转换string成gobjcet
        /// </summary>
        /// <param name="str">string</param>
        public static explicit operator gbol(string str) => new gbol(str);
    }

    /// <summary>
    /// int 可以通过强制转换进行更轻松的getset操作
    /// 例: line[(gint)"subname"]
    /// </summary>
    public class gint : gobj
    { /// <summary>
      /// 生成 Gobj
      /// </summary>
        public gint(string value) : base(value)
        {
        }

        /// <summary>
        /// 强制转换gobjcet成string
        /// </summary>
        /// <param name="gobject">gobject</param>
        public static explicit operator string(gint gobject) => gobject.VALUE;
        /// <summary>
        /// 强制转换string成gobjcet
        /// </summary>
        /// <param name="str">string</param>
        public static explicit operator gint(string str) => new gint(str);
    }

    /// <summary>
    /// long 可以通过强制转换进行更轻松的getset操作
    /// 例: line[(gi64)"subname"]
    /// </summary>
    public class gi64 : gobj
    { /// <summary>
      /// 生成 Gobj
      /// </summary>
        public gi64(string value) : base(value)
        {
        }

        /// <summary>
        /// 强制转换gobjcet成string
        /// </summary>
        /// <param name="gobject">gobject</param>
        public static explicit operator string(gi64 gobject) => gobject.VALUE;
        /// <summary>
        /// 强制转换string成gobjcet
        /// </summary>
        /// <param name="str">string</param>
        public static explicit operator gi64(string str) => new gi64(str);
    }

    /// <summary>
    /// double 可以通过强制转换进行更轻松的getset操作
    /// 例: line[(gdbe)"subname"]
    /// </summary>
    public class gdbe : gobj
    { /// <summary>
      /// 生成 Gobj
      /// </summary>
        public gdbe(string value) : base(value)
        {
        }

        /// <summary>
        /// 强制转换gobjcet成string
        /// </summary>
        /// <param name="gobject">gobject</param>
        public static explicit operator string(gdbe gobject) => gobject.VALUE;
        /// <summary>
        /// 强制转换string成gobjcet
        /// </summary>
        /// <param name="str">string</param>
        public static explicit operator gdbe(string str) => new gdbe(str);
    }

    /// <summary>
    /// DateTime 可以通过强制转换进行更轻松的getset操作
    /// 例: line[(gdat)"subname"]
    /// </summary>
    public class gdat : gobj
    { /// <summary>
      /// 生成 Gobj
      /// </summary>
        public gdat(string value) : base(value)
        {
        }

        /// <summary>
        /// 强制转换gobjcet成string
        /// </summary>
        /// <param name="gobject">gobject</param>
        public static explicit operator string(gdat gobject) => gobject.VALUE;
        /// <summary>
        /// 强制转换string成gobjcet
        /// </summary>
        /// <param name="str">string</param>
        public static explicit operator gdat(string str) => new gdat(str);
    }

    /// <summary>
    /// double(long) 通过转换long获得更精确的小数
    /// 可以通过强制转换进行更轻松的getset操作
    /// 例: line[(gflt)"subname"]
    /// </summary>
    public class gflt : gobj
    { /// <summary>
      /// 生成 Gobj
      /// </summary>
        public gflt(string value) : base(value)
        {
        }

        /// <summary>
        /// 强制转换gobjcet成string
        /// </summary>
        /// <param name="gobject">gobject</param>
        public static explicit operator string(gflt gobject) => gobject.VALUE;
        /// <summary>
        /// 强制转换string成gobjcet
        /// </summary>
        /// <param name="str">string</param>
        public static explicit operator gflt(string str) => new gflt(str);
    }
}
