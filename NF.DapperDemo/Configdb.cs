using NF.Dapper;

public class Configdb
{
    /// <summary>
    /// 设置
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int Set(string key, string value)
    {
        NDapper dbSqlLite = DapperManager.CreateDatabase(@"config2.db", DBType.SqlLite);
        if (dbSqlLite.QueryFirst<config>("select * from sysConfig where key=@key", new config { key = key, value = value }) == null)
        {
            string sql = "insert into sysConfig(key,value)values(@key,@value)";
            int n = dbSqlLite.Execute(sql, new config { key = key, value = value });
            return n;
        }
        else
        {
            string sql = "update sysConfig set value=@value where key=@key";
            int n = dbSqlLite.Execute(sql, new config { key = key, value = value });
            return n;
        }
    }

    /// <summary>
    /// 获取,当key不存在时的默认值
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static string Get(string key, string devalue = null)
    {
        NDapper dbSqlLite = DapperManager.CreateDatabase(@"config.db", DBType.SqlLite);
        config c = dbSqlLite.QueryFirst<config>("select * from sysConfig where key=@key", new config { key = key });
        if (c != null)
        {
            return c.value;
        }
        if (devalue != null)
        {
            string sql = "insert into sysConfig(key,value)values(@key,@value)";
            int n = dbSqlLite.Execute(sql, new config { key = key, value = devalue });
        }
        return devalue;
    }

    public class config
    {
        public string key { get; set; }
        public string value { get; set; }
    }
}
