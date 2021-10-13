using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
//using Oracle.ManagedDataAccess.Client;需要请自行引用NuGet包

/// <summary>
/// dxwang
/// 4638912@qq.com
/// </summary>
namespace NF.Dapper
{
    public  partial class DapperManager
    {
        /// <summary>
        /// 创建Dapper对象；默认为SqlServer
        /// </summary>
        /// <param name="strconn">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <param name="WriteLog">是否写错误日志</param>
        /// <returns></returns>
        public static NDapper CreateDatabase(string strconn, DBType dbType = DBType.MSSQL)
        {
            if (dbType == DBType.SqlLite)
            {
                string dbfile = System.AppDomain.CurrentDomain.BaseDirectory + strconn;
                strconn = string.Format("Data Source={0}", dbfile);
                return new NDapper(new SQLiteConnection(strconn));
            }

            if (dbType == DBType.MySql)
            {
                return new NDapper(new MySqlConnection(strconn));
            }

            if (dbType == DBType.MSSQL)
            {
                return new NDapper(new SqlConnection(strconn));
            }

            if (dbType == DBType.Oracle)
            {
                return new NDapper(new OracleConnection(strconn));//请自行引用NuGet包
                //string connString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=130.147.246.144)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=ECMS)));Persist Security Info=True;User ID=system;Password=Service01;";
            }

            if (dbType == DBType.Npgsql)
            {
                //IDbConnection conn = new OracleConnection(strconn); 请自行引用NuGet包
                //return new DapperBase(conn, WriteLog);
            }
           
            return null;
        }

       
    }
}
