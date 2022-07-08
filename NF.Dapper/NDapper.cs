using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;

/// <summary>
/// dxwang
/// 4638912@qq.com
/// </summary>
namespace NF.Dapper
{
    /// <summary>
    /// 执行主要操作的类,重写Dapper
    /// </summary>
    public class NDapper
    {
        public IDbTransaction DbTransaction { get; set; }
        private readonly IDbConnection conn;
        public DBType thisDBType;

        /// <summary>
        /// 构造函数
        /// </summary>
        public NDapper(IDbConnection conn, DBType thisDBType)
        {
            this.conn = conn;
            this.thisDBType = thisDBType;
        }

        /// <summary>
        /// 数据库地址连接字符串
        /// </summary>
        /// <returns></returns>
        public string strconn()
        {
            return conn.ConnectionString;
        }

        #region ListToDataTable
        /// <summary>
        /// ListToDataTable
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public DataTable ToDataTable<TEntity>(List<TEntity> entities)
        {
            if (entities == null) { return new DataTable(); }

            Type type = typeof(TEntity);
            PropertyInfo[] properties = type.GetProperties();
            DataTable dt = new DataTable(type.Name);
            foreach (var item in properties)
            {
                dt.Columns.Add(new DataColumn(item.Name) { DataType = item.PropertyType });
            }
            foreach (var item in entities)
            {
                DataRow row = dt.NewRow();
                foreach (var property in properties)
                {
                    row[property.Name] = property.GetValue(item);
                }
                dt.Rows.Add(row);
            }
            return dt;
        } 
        #endregion

        #region 判断数据库连接状态
        /// <summary>
        /// 判断数据库连接状态
        /// </summary>
        /// <returns></returns>
        public ConnectionState State()
        {
            try
            {
                conn.Open();
                return conn.State;
            }
            catch (Exception ex)
            {
                NDapperLog.write(ex.ToString(), "ConnectionState");
            }
            finally
            {
                conn.Close();
            }
            return conn.State;
        }
        #endregion


        #region 实例方法

        #region 查询

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="dbConnKey">数据库连接</param>
        /// <param name="param">sql查询参数</param>
        /// <param name="buffered">是否缓冲</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string sql, object param = null, bool buffered = true, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            if (DbTransaction == null)
            {
                try
                {
                    return conn.Query<T>(sql, param, null, buffered, commandTimeout, commandType);
                }
                catch (Exception ex)
                {
                    NDapperLog.write(ex.ToString(), "Query");
                    return new List<T>();
                    //return default(List<T>);
                }
            }
            else
            {
                return DbTransaction.Connection.Query<T>(sql, param, DbTransaction, buffered, commandTimeout, commandType);
            }

        }


        /// <summary>
        /// 查询(异步版本)
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="dbConnKey">数据库连接</param>
        /// <param name="param">sql查询参数</param>
        /// <param name="buffered">是否缓冲</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, bool buffered = true, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            if (DbTransaction == null)
            {
                try
                {
                    return await conn.QueryAsync<T>(sql, param, null, commandTimeout, commandType);
                }
                catch (Exception ex)
                {
                    NDapperLog.write(ex.ToString(), "QueryAsync");
                    return new List<T>();
                }
            }
            else
            {
                return await DbTransaction.Connection.QueryAsync<T>(sql, param, DbTransaction, commandTimeout, commandType);
            }

        }


        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="dbConnKey">数据库连接</param>
        /// <param name="param">sql查询参数</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public T QueryFirst<T>(string sql, object param = null, bool buffered = true, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            if (DbTransaction == null)
            {
                try
                {
                    return conn.QueryFirstOrDefault<T>(sql, param, null, commandTimeout, commandType);
                }
                catch (Exception ex)
                {
                    NDapperLog.write(ex.ToString(), "QueryFirst");
                    return default(T);
                }
            }
            else
            {
                return DbTransaction.Connection.QueryFirstOrDefault<T>(sql, param, DbTransaction, commandTimeout, commandType);
            }

        }

        /// <summary>
        /// 查询(异步版本)
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="dbConnKey">数据库连接</param>
        /// <param name="param">sql查询参数</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public async Task<T> QueryFirstAsync<T>(string sql, object param = null, bool buffered = true, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            if (DbTransaction == null)
            {
                try
                {
                    return await conn.QueryFirstOrDefaultAsync<T>(sql, param, null, commandTimeout, commandType);
                }
                catch (Exception ex)
                {
                    NDapperLog.write(ex.ToString(), "QueryFirstAsync");
                    return default(T);
                }
            }
            else
            {
                return await DbTransaction.Connection.QueryFirstOrDefaultAsync<T>(sql, param, DbTransaction, commandTimeout, commandType);
            }

        }




        /// <summary>
        /// 查询返回 IDataReader
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="dbConnKey">数据库连接</param>
        /// <param name="param">sql查询参数</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            if (DbTransaction == null)
            {
                try
                {
                    return conn.ExecuteReader(sql, param, null, commandTimeout, commandType);
                }
                catch (Exception ex)
                {
                    NDapperLog.write(ex.ToString(), "ExecuteReader");
                    return null;
                }
            }
            else
            {
                return DbTransaction.Connection.ExecuteReader(sql, param, DbTransaction, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 查询单个返回值
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="dbConnKey">数据库连接</param>
        /// <param name="param">sql查询参数</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public T ExecuteScalar<T>(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            if (DbTransaction == null)
            {
                try
                {
                    return conn.ExecuteScalar<T>(sql, param, null, commandTimeout, commandType);
                }
                catch (Exception ex)
                {
                    NDapperLog.write(ex.ToString(), "ExecuteScalar");
                    return default(T);
                }
            }
            else
            {
                return DbTransaction.Connection.ExecuteScalar<T>(sql, param, DbTransaction, commandTimeout, commandType);
            }

        }
        #endregion

        /// <summary>
        /// 执行增删改sql
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="dbkey">数据库连接</param>
        /// <param name="param">sql查询参数</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public int Execute(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            if (DbTransaction == null)
            {
                try
                {
                    return conn.Execute(sql, param, null, commandTimeout, commandType);
                }
                catch (Exception ex)
                {
                    NDapperLog.write(ex.ToString(), "ExecuteSql");
                    return -1;
                }
            }
            else
            {
                return DbTransaction.Connection.Execute(sql, param, DbTransaction);
            }
        }

        /// <summary>
        /// 执行增删改sql(异步版本)
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="dbkey">数据库连接</param>
        /// <param name="param">sql查询参数</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public async Task<int> ExecuteSqlAsync(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            if (DbTransaction == null)
            {
                try
                {
                    return await conn.ExecuteAsync(sql, param, null, commandTimeout, commandType);
                }
                catch (Exception ex)
                {
                    NDapperLog.write(ex.ToString(), "ExecuteSqlAsync");
                    return -1;
                }
            }
            else
            {
                await DbTransaction.Connection.ExecuteAsync(sql, param, DbTransaction);
                return 0;
            }
        }
        #endregion

        #region 事务提交

        /// <summary>
        /// 事务开始
        /// </summary>
        /// <returns></returns>
        public NDapper BeginTransaction()
        {
            conn.BeginTransaction();//需要手动开启事务控制
            return this; ;
        }

        /// <summary>
        /// 提交当前操作的结果
        /// </summary>
        public int Commit()
        {
            try
            {
                if (DbTransaction != null)
                {
                    DbTransaction.Commit();
                    this.Close();
                }
                return 1;
            }
            catch (Exception ex)
            {
                NDapperLog.write(ex.ToString(), "Commit");
                return -1;
            }
            finally
            {
                if (DbTransaction == null)
                {
                    this.Close();
                }
            }
        }

        /// <summary>
        /// 把当前操作回滚成未提交状态
        /// </summary>
        public void Rollback()
        {
            this.DbTransaction.Rollback();
            this.DbTransaction.Dispose();
            this.Close();
        }

        /// <summary>
        /// 关闭连接 内存回收
        /// </summary>
        public void Close()
        {
            IDbConnection dbConnection = DbTransaction.Connection;
            if (dbConnection != null && dbConnection.State != ConnectionState.Closed)
            {
                dbConnection.Close();
            }
        }

        #endregion
    }
}