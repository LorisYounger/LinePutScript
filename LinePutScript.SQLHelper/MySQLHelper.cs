using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;


namespace LinePutScript.SQLHelper
{
    /// <summary>
    /// LinePutScript -> MySQL
    /// </summary>
    public class MySQLHelper
    {
        private MySqlConnection conn = null;
        //private MySqlCommand cmd = null;
        //private MySqlDataReader sdr;
        //private MySqlDataAdapter sda = null;
        /// <summary>
        /// 新建数据库连接
        /// </summary>
        /// <param name="connStr">连接数据库所用数据
        /// eg:Server=localhost;port=3306;Database=dbname;User=dbuser;Password=dbpassword;</param>
        public MySQLHelper(string connStr)
        {
            conn = new MySqlConnection(connStr); //数据库连接
        }
        /// <summary>
        /// 新建数据库连接
        /// </summary>
        /// <param name="Server">服务器</param>
        /// <param name="port">端口</param>
        /// <param name="DatabaseName">数据库名</param>
        /// <param name="User">数据库用户名</param>
        /// <param name="Password">数据库密码</param>
        /// <param name="Other">其他参数</param>
        public MySQLHelper(string Server, string port, string DatabaseName, string User, string Password, string Other = null)
        {
            conn = new MySqlConnection($"Server={Server};port={port};Database={DatabaseName};User={User};Password={Password};{Other}"); //数据库连接
        }

        /// <summary>
        /// 打开数据库链接
        /// </summary>
        /// <returns></returns>
        private MySqlConnection GetConn()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            return conn;
        }

        /// <summary>
        ///  关闭数据库链接
        /// </summary>
        private void GetConnClose()
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }
        /// <summary>
        /// 执行不带参数的增删改SQL语句或存储过程{不安全}
        /// </summary>
        /// <param name="cmdText">增删改SQL语句或存储过程的字符串{不安全}</param>
        /// <returns>受影响的函数</returns>
        public int ExecuteNonQuery(string cmdText)
        {
            int res;
            using (MySqlCommand cmd = new MySqlCommand(cmdText, GetConn()))
            {
                cmd.CommandType = CommandType.Text;
                res = cmd.ExecuteNonQuery();
            }
            return res;
        }

        /// <summary>
        /// 执行带参数的增删改SQL语句或存储过程
        /// </summary>
        /// <param name="cmdText">增删改SQL语句或存储过程的字符串</param>
        /// <param name="paras">往存储过程或SQL中赋的参数集合</param>
        /// <returns>受影响的函数</returns>
        public int ExecuteNonQuery(string cmdText, params Parameter[] paras)
        {
            int res;
            using (MySqlCommand cmd = new MySqlCommand(cmdText, GetConn()))
            {
                cmd.CommandType = CommandType.Text;
                foreach (Parameter pm in paras)
                    cmd.Parameters.Add(pm.ToMySqlParameter());
                res = cmd.ExecuteNonQuery();
            }
            return res;
        }

        /// <summary>
        /// 执行不带参数的查询SQL语句或存储过程{不安全}
        /// </summary>
        /// <param name="cmdText">查询SQL语句或存储过程的字符串{不安全}</param>
        /// <returns>查询到的DataTable对象</returns>
        public LpsDocument ExecuteQuery(string cmdText)
        {
            DataTable dt = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand(cmdText, GetConn()))
            {
                cmd.CommandType = CommandType.Text;
                using (var sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    dt.Load(sdr);
                }
            }
            LpsDocument lps = new LpsDocument();
            Line tmp;
            for (int l = 0; l < dt.Rows.Count; l++)
            {
                tmp = new Line(dt.Columns[0].ColumnName, Convert(dt.Rows[l].ItemArray[0].ToString()));
                for (int s = 1; s < dt.Rows[l].ItemArray.Length; s++)
                {
                    tmp.AddSub(new Sub(dt.Columns[s].ColumnName, Convert(dt.Rows[l].ItemArray[s].ToString())));
                }
                lps.AddLine(tmp);
            }
            return lps;
        }

        /// <summary>
        /// 执行带参数的查询SQL语句或存储过程
        /// </summary>
        /// <param name="cmdText">查询SQL语句或存储过程的字符串</param>
        /// <param name="paras">参数集合</param>
        /// <returns></returns>
        public LpsDocument ExecuteQuery(string cmdText, params Parameter[] paras)
        {
            DataTable dt = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand(cmdText, GetConn()))
            {
                cmd.CommandType = CommandType.Text;
                foreach (Parameter pm in paras)
                    cmd.Parameters.Add(pm.ToMySqlParameter());
                using (var sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    dt.Load(sdr);
                }
            }
            LpsDocument lps = new LpsDocument();
            Line tmp;
            for (int l = 0; l < dt.Rows.Count; l++)
            {
                tmp = new Line(dt.Columns[0].ColumnName, Convert(dt.Rows[l].ItemArray[0].ToString()));
                for (int s = 1; s < dt.Rows[l].ItemArray.Length; s++)
                {
                    tmp.AddSub(new Sub(dt.Columns[s].ColumnName, Convert(dt.Rows[l].ItemArray[s].ToString())));
                }
                lps.AddLine(tmp);
            }
            return lps;
        }

        public static string Convert(object obj)
        {
            switch (obj.GetType().FullName)
            {
                case "System.DateTime":
                    return ((DateTime)obj).ToString("yyyy-MM-dd HH:mm");

                default:
                    return System.Convert.ToString(obj.ToString());

            }
        }

        ///// <summary>
        ///// 执行指定数据库连接字符串的命令,返回DataSet.
        ///// </summary>
        ///// <param name="strSql">一个有效的数据库连接字符串</param>
        ///// <returns>返回一个包含结果集的DataSet</returns>
        //public DataSet ExecuteDataset(string strSql)
        //{
        //    DataSet ds = new DataSet();
        //    sda = new MySqlDataAdapter(strSql, GetConn());
        //    try
        //    {
        //        sda.Fill(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        GetConnClose();
        //    }
        //    return ds;
        //}

        /// <summary>
        /// 重构的SQL参数类
        /// </summary>
        /// 使用方法:"select * from [Users] where UserName=@UserName and Password=@Password" 
        /// "@UserName","DATA"
        /// "@Password","DATA"

        public struct Parameter
        {
            /// <summary>
            /// 参数化名称
            /// </summary>
            public string ParameterName;
            /// <summary>
            /// 参数化数据
            /// </summary>
            public object Value;

            /// <summary>
            /// 新建SQL参数类
            /// </summary>
            /// <param name="name">参数化名称</param>
            /// <param name="value">参数化数据</param>
            public Parameter(string name, object value)
            {
                ParameterName = name;
                Value = value;
            }

            //public enum DbType
            //{
            //    //
            //    // 摘要:
            //    //     MySql.Data.MySqlClient.MySqlDbType.Decimal
            //    //     A fixed precision and scale numeric value between -1038 -1 and 10 38 -1.
            //    Decimal = 0,
            //    //
            //    // 摘要:
            //    //     MySql.Data.MySqlClient.MySqlDbType.Byte
            //    //     The signed range is -128 to 127. The unsigned range is 0 to 255.
            //    Byte = 1,
            //    //
            //    // 摘要:
            //    //     MySql.Data.MySqlClient.MySqlDbType.Int16
            //    //     A 16-bit signed integer. The signed range is -32768 to 32767. The unsigned range
            //    //     is 0 to 65535
            //    Int16 = 2,
            //    //
            //    // 摘要:
            //    //     MySql.Data.MySqlClient.MySqlDbType.Int32
            //    //     A 32-bit signed integer
            //    Int32 = 3,
            //    //
            //    // 摘要:
            //    //     System.Single
            //    //     A small (single-precision) floating-point number. Allowable values are -3.402823466E+38
            //    //     to -1.175494351E-38, 0, and 1.175494351E-38 to 3.402823466E+38.
            //    Float = 4,
            //    //
            //    // 摘要:
            //    //     MySql.Data.MySqlClient.MySqlDbType.Double
            //    //     A normal-size (double-precision) floating-point number. Allowable values are
            //    //     -1.7976931348623157E+308 to -2.2250738585072014E-308, 0, and 2.2250738585072014E-308
            //    //     to 1.7976931348623157E+308.
            //    Double = 5,
            //    //
            //    // 摘要:
            //    //     A timestamp. The range is '1970-01-01 00:00:00' to sometime in the year 2037
            //    Timestamp = 7,
            //    //
            //    // 摘要:
            //    //     MySql.Data.MySqlClient.MySqlDbType.Int64
            //    //     A 64-bit signed integer.
            //    Int64 = 8,
            //    //
            //    // 摘要:
            //    //     Specifies a 24 (3 byte) signed or unsigned value.
            //    Int24 = 9,
            //    //
            //    // 摘要:
            //    //     Date The supported range is '1000-01-01' to '9999-12-31'.
            //    Date = 10,
            //    //
            //    // 摘要:
            //    //     Time
            //    //     The range is '-838:59:59' to '838:59:59'.
            //    Time = 11,
            //    //
            //    // 摘要:
            //    //     DateTime The supported range is '1000-01-01 00:00:00' to '9999-12-31 23:59:59'.
            //    DateTime = 12,
            //    //
            //    // 摘要:
            //    //     Datetime The supported range is '1000-01-01 00:00:00' to '9999-12-31 23:59:59'.
            //    Datetime = 12,
            //    //
            //    // 摘要:
            //    //     A year in 2- or 4-digit format (default is 4-digit). The allowable values are
            //    //     1901 to 2155, 0000 in the 4-digit year format, and 1970-2069 if you use the 2-digit
            //    //     format (70-69).
            //    Year = 13,
            //    //
            //    // 摘要:
            //    //     Obsolete Use Datetime or Date type
            //    Newdate = 14,
            //    //
            //    // 摘要:
            //    //     A variable-length string containing 0 to 65535 characters
            //    VarString = 15,
            //    //
            //    // 摘要:
            //    //     Bit-field data type
            //    Bit = 16,
            //    //
            //    // 摘要:
            //    //     JSON
            //    JSON = 245,
            //    //
            //    // 摘要:
            //    //     New Decimal
            //    NewDecimal = 246,
            //    //
            //    // 摘要:
            //    //     An enumeration. A string object that can have only one value, chosen from the
            //    //     list of values 'value1', 'value2', ..., NULL or the special "" error value. An
            //    //     ENUM can have a maximum of 65535 distinct values
            //    Enum = 247,
            //    //
            //    // 摘要:
            //    //     A set. A string object that can have zero or more values, each of which must
            //    //     be chosen from the list of values 'value1', 'value2', ... A SET can have a maximum
            //    //     of 64 members.
            //    Set = 248,
            //    //
            //    // 摘要:
            //    //     A binary column with a maximum length of 255 (2^8 - 1) characters
            //    TinyBlob = 249,
            //    //
            //    // 摘要:
            //    //     A binary column with a maximum length of 16777215 (2^24 - 1) bytes.
            //    MediumBlob = 250,
            //    //
            //    // 摘要:
            //    //     A binary column with a maximum length of 4294967295 or 4G (2^32 - 1) bytes.
            //    LongBlob = 251,
            //    //
            //    // 摘要:
            //    //     A binary column with a maximum length of 65535 (2^16 - 1) bytes.
            //    Blob = 252,
            //    //
            //    // 摘要:
            //    //     A variable-length string containing 0 to 255 bytes.
            //    VarChar = 253,
            //    //
            //    // 摘要:
            //    //     A fixed-length string.
            //    String = 254,
            //    //
            //    // 摘要:
            //    //     Geometric (GIS) data type.
            //    Geometry = 255,
            //    //
            //    // 摘要:
            //    //     Unsigned 8-bit value.
            //    UByte = 501,
            //    //
            //    // 摘要:
            //    //     Unsigned 16-bit value.
            //    UInt16 = 502,
            //    //
            //    // 摘要:
            //    //     Unsigned 32-bit value.
            //    UInt32 = 503,
            //    //
            //    // 摘要:
            //    //     Unsigned 64-bit value.
            //    UInt64 = 508,
            //    //
            //    // 摘要:
            //    //     Unsigned 24-bit value.
            //    UInt24 = 509,
            //    //
            //    // 摘要:
            //    //     Fixed length binary string.
            //    Binary = 600,
            //    //
            //    // 摘要:
            //    //     Variable length binary string.
            //    VarBinary = 601,
            //    //
            //    // 摘要:
            //    //     A text column with a maximum length of 255 (2^8 - 1) characters.
            //    TinyText = 749,
            //    //
            //    // 摘要:
            //    //     A text column with a maximum length of 16777215 (2^24 - 1) characters.
            //    MediumText = 750,
            //    //
            //    // 摘要:
            //    //     A text column with a maximum length of 4294967295 or 4G (2^32 - 1) characters.
            //    LongText = 751,
            //    //
            //    // 摘要:
            //    //     A text column with a maximum length of 65535 (2^16 - 1) characters.
            //    Text = 752,
            //    //
            //    // 摘要:
            //    //     A guid column.
            //    Guid = 800
            //}

            ///// <summary>
            ///// 所使用格式
            ///// </summary>
            //public DbType Type;

            //public Parameter(string name, object value, DbType type)
            //{
            //    ParameterName = name;
            //    Value = value;
            //    Type = type;
            //}
            //public Parameter(string name, string value, DbType type = DbType.VarChar)
            //{
            //    ParameterName = name;
            //    Value = value;
            //    Type = type;
            //}
            //public Parameter(string name, int value, DbType type = DbType.Int32)
            //{
            //    ParameterName = name;
            //    Value = value;
            //    Type = type;
            //}
            //public Parameter(string name, short value, DbType type = DbType.Int16)
            //{
            //    ParameterName = name;
            //    Value = value;
            //    Type = type;
            //}
            //public Parameter(string name, long value, DbType type = DbType.Int32)
            //{
            //    ParameterName = name;
            //    Value = value;
            //    Type = type;
            //}
            //public Parameter(string name, byte value, DbType type = DbType.Byte)
            //{
            //    ParameterName = name;
            //    Value = value;
            //    Type = type;
            //}
            /// <summary>
            /// 转换成MySql参数类
            /// </summary>
            /// <returns></returns>
            public MySqlParameter ToMySqlParameter()
            {
                //var sqlp =  new MySqlParameter(ParameterName, Value);
                //sqlp.DbType = (System.Data.DbType)(int)Type;
                return new MySqlParameter(ParameterName, Value);
            }
        }
    }
}
