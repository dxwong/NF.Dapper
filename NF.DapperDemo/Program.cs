using NF.Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace NF.DapperDemo
{
    class Program
    {
        public class KLine
        {
            public int ID { get; set; }
            public string symbol { get; set; }
        }
        static void Main(string[] args)
        {
            NDapperLog.Receive += msg; 

            string ConnectionStr = "server=(local);UID=sa;PWD=sa;database=test";
            string mysqlhost = "Host = 127.0.0.1; UserName = root; Password = @; Database = KLine; Port = 3316; CharSet = utf8; Allow Zero Datetime = true;";

            NDapper dbSqlLite = DapperManager.CreateDatabase(@"symbo444.db", DBType.SqlLite);
            ConnectionState DapperState = dbSqlLite.State();
            string createtb = "create table  hsi1903_min1 (id int, symbol varchar(50))";
            int x = dbSqlLite.Execute(createtb);

            //匿名
            var result = dbSqlLite.Execute("Insert into hsi1903_min1 values (@id, @symbol)",  new { id = 565, symbol = "123'456@qq.com" });
            KLine listtest = dbSqlLite.QueryFirst<KLine>("select id,symbol from hsi1903_min001");

            //实体
            KLine k = new KLine();
            k.ID = 1222;
            k.symbol = "";
            var result2 = dbSqlLite.Execute("insert into hsi1903_min1(id,symbol) Values (@id, @symbol)", k);
            var listLite00 = dbSqlLite.Query<object>("select * from hsi1903_min1");
            foreach(object o in listLite00)
            {

            }
            //批量插入
            List<KLine> listk = new List<KLine>();
            listk.Add(new KLine { ID = 1, symbol = "vv'00'1" });
            listk.Add(new KLine { ID = 2, symbol = "vv'00'2" });
            var result3 = dbSqlLite.Execute("insert into hsi1903_min1(id,symbol) Values (@id, @symbol)", listk);

            var listSS = dbSqlLite.Query<KLine>("select id,symbol from hsi1903_min1");

            //查询
            var listLite = dbSqlLite.Query<KLine>("select UserName,Email from Users where UserId=@UserId", new { UserId = 2 });
            var listLite2 = dbSqlLite.QueryAsync<KLine>("select UserName,Email from Users where UserId=@UserId", new  { ID = 1 });



            NDapper dbss = DapperManager.CreateDatabase(ConnectionStr, DBType.SqlServer);
            var state = dbss.State();
            var list = dbss.Query<KLine>("select id,symbol from pp2009_min10");

            NDapper dbss1 = DapperManager.CreateDatabase(ConnectionStr, DBType.SqlServer);
            var list1 = dbss1.QueryAsync<KLine>("select id,symbol from pp2009_min11");

            NDapper dbss2 = DapperManager.CreateDatabase(ConnectionStr, DBType.SqlServer);
            var list2 = dbss2.Query<KLine>("select id,symbol from pp2009_min12");

            KLine ks = new KLine();
            dbss2.Query<KLine>("select UserName,Email from User  where UserId=@UserId", new KLine() { ID = 1 });

           
            Console.ReadKey();
        }

       static void  msg(string msg) {
            Console.WriteLine(msg+"\r\n\r\n");
        }
    }
}
