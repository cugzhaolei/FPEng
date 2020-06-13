using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using MySql.Data.MySqlClient;

namespace Face.Web.App.Utils
{
    public class DBConfig
    {
        static string DefaultSqlConnection = "server=127.0.0.1;user id=root;pwd=159357;database=`face-app`;SslMode=none;allowuservariables=True;";

        static string MSSqlConnection = "";
        /// <summary>
        /// 设置字符串连接
        /// </summary>
        /// <param name="type"></param>
        public static void SetDBConnectionType(string type)
        {
            if (type == "remote server")//远程服务器连接
            {
                DefaultSqlConnection = @"server=;database=;UID=sa;PWD=admin123;";

            }
            else if (type == "local server")//本地服务器连接 
            {
                DefaultSqlConnection = @"server=.;database=;UID=sa;PWD=Aa123456;";

            }
            else if (type == "local db")//配置自己的电脑连接
            {

                DefaultSqlConnection = @"server=.;database=;UID=sa;PWD=159357;";;

            }

        }


        /// <summary>
        /// 设置数据库连接字符串方法
        /// </summary>
        /// <param name="sqlConnectionString"></param>
        /// <returns></returns>
        public static IDbConnection GetSqlConnection(string sqlConnectionString = null)
        {
            //2020.3.18 设置网络连接
            //远程连接
            SetDBConnectionType("remote server");
            //SetDBConnectionType("local db");
            //SetDBConnectionType("local server");


            if (string.IsNullOrWhiteSpace(sqlConnectionString))
            {
                sqlConnectionString = DefaultSqlConnection;
                //Mysql连接字符串 MySQLConnection
                IDbConnection conn = new MySqlConnection(sqlConnectionString);
                conn.Open();
                return conn;
            }
            else if (sqlConnectionString == "MySql")
            {
                sqlConnectionString = DefaultSqlConnection;
                //Mysql连接字符串 MySQLConnection
                IDbConnection conn = new MySqlConnection(sqlConnectionString);
                conn.Open();
                return conn;
            }
            else if (sqlConnectionString == "MSSQL")
            {
                sqlConnectionString = MSSqlConnection;

                //Mysql连接字符串 MySQLConnection
                IDbConnection conn = new SqlConnection(sqlConnectionString);
                conn.Open();
                return conn;

            }
            IDbConnection dbConnection = new SqlConnection(sqlConnectionString);
            return dbConnection;
        }


        //读取Excel文件


    }

}

