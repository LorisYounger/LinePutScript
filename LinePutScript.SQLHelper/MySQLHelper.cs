using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;


namespace LinePutScript.SQLHelper
{
    public class MySQLHelper
    {
        private MySqlConnection conn = null;
        private MySqlCommand cmd = null;
        private MySqlDataReader sdr;
        private MySqlDataAdapter sda = null;
        public MySQLHelper()
        {
            string connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString; //获取MySql数据库连接字符串
            conn = new MySqlConnection(connStr); //数据库连接
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
        /// 执行不带参数的增删改SQL语句或存储过程
        /// </summary>
        /// <param name="cmdText">增删改SQL语句或存储过程的字符串</param>
        /// <param name="ct">命令类型</param>
        /// <returns>受影响的函数</returns>
        public int ExecuteNonQuery(string cmdText, CommandType ct)
        {
            int res;
            using (cmd = new MySqlCommand(cmdText, GetConn()))
            {
                cmd.CommandType = ct;
                res = cmd.ExecuteNonQuery();
            }
            return res;
        }

        /// <summary>
        /// 执行带参数的增删改SQL语句或存储过程
        /// </summary>
        /// <param name="cmdText">增删改SQL语句或存储过程的字符串</param>
        /// <param name="paras">往存储过程或SQL中赋的参数集合</param>
        /// <param name="ct">命令类型</param>
        /// <returns>受影响的函数</returns>
        public int ExecuteNonQuery(string cmdText, MySqlParameter[] paras, CommandType ct)
        {
            int res;
            using (cmd = new MySqlCommand(cmdText, GetConn()))
            {
                cmd.CommandType = ct;
                cmd.Parameters.AddRange(paras);
                res = cmd.ExecuteNonQuery();
            }
            return res;
        }

        /// <summary>
        /// 执行不带参数的查询SQL语句或存储过程
        /// </summary>
        /// <param name="cmdText">查询SQL语句或存储过程的字符串</param>
        /// <param name="ct">命令类型</param>
        /// <returns>查询到的DataTable对象</returns>
        public DataTable ExecuteQuery(string cmdText, CommandType ct)
        {
            DataTable dt = new DataTable();
            cmd = new MySqlCommand(cmdText, GetConn());
            cmd.CommandType = ct;
            using (sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                dt.Load(sdr);
            }
            return dt;
        }
        /// <summary>
        /// 执行不带参数的查询SQL语句或存储过程
        /// </summary>
        /// <param name="cmdText">查询SQL语句或存储过程的字符串</param>
        /// <param name="ct">命令类型</param>
        /// <returns>查询到的DataTable对象</returns>
        public LpsDocument ExecuteQuery(string cmdText)
        {
            DataTable dt = new DataTable();
            cmd = new MySqlCommand(cmdText, GetConn());
            cmd.CommandType = CommandType.Text;
            using (sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                dt.Load(sdr);
            }
            LpsDocument lps = new LpsDocument();
            Line tmp;
            for (int l = 0; l < dt.Rows.Count; l++)
            {
                tmp = new Line(dt.Columns[0].ColumnName, Convert.ToString(dt.Rows[l].ItemArray[0]));
                for (int s = 1; s < dt.Rows[l].ItemArray.Length; s++)
                {
                    tmp.AddSub(new Sub(dt.Columns[s].ColumnName, Convert.ToString(dt.Rows[l].ItemArray[s])));
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
        /// <param name="ct">命令类型</param>
        /// <returns></returns>
        public DataTable ExecuteQuery(string cmdText, MySqlParameter[] paras, CommandType ct)
        {
            DataTable dt = new DataTable();
            cmd = new MySqlCommand(cmdText, GetConn());
            cmd.CommandType = ct;
            cmd.Parameters.AddRange(paras);
            using (sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                dt.Load(sdr);
            }
            return dt;
        }

        /// <summary>
        /// 执行指定数据库连接字符串的命令,返回DataSet.
        /// </summary>
        /// <param name="strSql">一个有效的数据库连接字符串</param>
        /// <returns>返回一个包含结果集的DataSet</returns>
        public DataSet ExecuteDataset(string strSql)
        {
            DataSet ds = new DataSet();
            sda = new MySqlDataAdapter(strSql, GetConn());
            try
            {
                sda.Fill(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                GetConnClose();
            }
            return ds;
        }
    }
}
