using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Face.Web.Utils
{
    public class DBConfig
    {
        static string DefaultSqlConnection = "server=127.0.0.1;user id=root;pwd=159357;database=`face-app`;SslMode=none;allowuservariables=True;";
        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <param name="sqlConnectionString"></param>
        /// <returns></returns>
        public static IDbConnection GetSqlConnection(string sqlConnectionString = null)
        {

            //json文件的路径

            if (string.IsNullOrWhiteSpace(sqlConnectionString))
            {
                sqlConnectionString = DefaultSqlConnection;
            }

            if (sqlConnectionString == "MySQL")
            {
                sqlConnectionString = DefaultSqlConnection;

            }

            //Mysql连接字符串 MySQLConnection
            IDbConnection conn = new MySqlConnection(sqlConnectionString);
            conn.Open();
            return conn;
        }

    }
}
