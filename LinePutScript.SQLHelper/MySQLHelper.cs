using MySqlConnector;
using System;
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

        private static string Convert(object obj)
        {
            switch (obj.GetType().FullName)
            {
                case "System.DateTime":
                    return ((DateTime)obj).ToString("yyyy-MM-dd HH:mm:ss");

                default:
                    return System.Convert.ToString(obj.ToString());

            }
        }
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
