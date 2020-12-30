using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NF.Dapper
{
    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum DBType
    {
        SqlLite,
        SqlServer,
        MySql,
        Oracle,
        Npgsql
    }
}

//select 'public ' + case when DATA_TYPE IN('INT','BIGINT') THEN 'int '
//WHEN DATA_TYPE IN ('varchar','ntext','text','nchar') then 'string '
//when DATA_TYPE IN('date','datetime')  then 'DateTime '
//when DATA_TYPE IN('decimal','float','numeric') then 'double '
//when DATA_TYPE IN('bit') then 'bool ' END + COLUMN_NAME + '  { get; set; }',CHARACTER_MAXIMUM_LENGTH from information_schema.columns where 
//TABLE_NAME='sr105_min5'
