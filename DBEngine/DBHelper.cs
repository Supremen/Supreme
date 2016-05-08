using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinKin.DALEngine
{
    /// <summary>
    /// 数据库访问抽象基础类
    /// </summary>
    public abstract class DBHelper
    {
        /// <summary>
        /// 数据库连接字符串(web.config来配置).
        /// 多数据库可使用DBHelper来实现.
        /// </summary>
        public static string connectionString = DBConstant.ConnectionString;

        public DBHelper() 
        {

        }

        #region 公用方法
        /// <summary>
        /// 检查是否存在某表的某个字段.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static bool ExistsColumn(string tableName,string columnName)
        {
            string sql =" select count(1) from syscolumns where [id]=object_id('" + tableName + "') and [name]='" + columnName + "'";
            object result = GetSingle(sql);
            if (result == null)
            {
                return false;
            }
            return Convert.ToInt16(result) > 0;
        }
        /// <summary>
        /// 根据表面获取某个字段的最大ID.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static int GetMaxID(string tableName,string columnName)
        {
            string sql = "select max(" + columnName + ")+1 from " + tableName;
            object result = GetSingle(sql);
            if (result == null)
            {
                return 1;
            }
            else
            {
                return int.Parse(result.ToString());
            }
        }
        /// <summary>
        /// 检查是否存在该对象.
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public static bool Exists(string strSql)
        {
            Object obj = GetSingle(strSql);
            int count;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                count = 0;
            }
            else
            {
                count = int.Parse(obj.ToString());
            }
            if (count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static bool Exists(string sql, params SqlParameter[] cmdParms)
        {
            object obj = GetSingle(sql, cmdParms);
            int count;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                count = 0;
            }
            else
            {
                count = int.Parse(obj.ToString());
            }
            if (count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 检查表是否存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static bool ExistsTable(string tableName)
        {
            string sql = "select count(*) from sysobjects where id = object_id(N'[" + tableName + "]') and OBJECTPROPERTY(id, N'IsUserTable') = 1";
            object obj = GetSingle(sql);
            int count;
            if (Object.Equals(obj, null) || Object.Equals(obj, System.DBNull.Value))
            {
                count = 0;
            }
            else
            {
                count = int.Parse(obj.ToString());
            }
            if (count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion

        #region 增删改查
        /// <summary>
        /// 执行SQL语句,返回受影响的行数.
        /// </summary>
        /// <param name="SQLString"></param>
        /// <returns></returns>
        public static int ExecuteSql(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        cmd.CommandTimeout = 1000000;
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// 执行SQL语句,返回受影响的行数.
        /// </summary>
        /// <param name="SQLString"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static int ExecuteSql(string SQLString,SqlConnection connection)
        {
            using (SqlCommand cmd = new SqlCommand(SQLString, connection))
            {
                try
                {
                    connection.Open();
                    cmd.CommandTimeout = 1000000;
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    connection.Close();
                    throw e;
                }
            }
        }
        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString, params SqlParameter[] cmdParms)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        try
                        {
                            cmd.CommandTimeout = 1000000;
                            PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                            int rows = cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                            return rows;
                        }
                        catch (System.Data.SqlClient.SqlException e)
                        {
                            throw e;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static int ExecuteSql(string SQLString,SqlTransaction trans, params SqlParameter[] cmdParms)
        {

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        try
                        {
                            cmd.CommandTimeout = 1000000;
                            PrepareCommand(cmd, trans.Connection, null, SQLString, cmdParms);
                            int rows = cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                            return rows;
                        }
                        catch (System.Data.SqlClient.SqlException e)
                        {
                            throw e;
                        }
                    }
                }

        } 
        /// <summary>
        /// 执行SQL语句,返回受影响的行数
        /// </summary>
        /// <param name="SQLString"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public static int ExecuteSql(string SQLString,SqlTransaction trans)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cmd.CommandTimeout = 1000000;
                        PrepareCommand(cmd,trans.Connection,null,SQLString,null);
                        cmd.Transaction = trans;
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }
        public static int ExecuteSqlByTime(string SQLString, int Times)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        cmd.CommandTimeout = Times;
                        cmd.CommandTimeout = 1000000;
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }

        public static int ExecuteSqlTran(List<String> SQLString)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandTimeout = 1000000;
                cmd.Connection = conn;
                SqlTransaction trans = conn.BeginTransaction();
                cmd.Transaction = trans;
                try
                {
                    int count = 0;
                    for (int i = 0; i < SQLString.Count; i++)
                    {
                        string strSql = SQLString[i];
                        if (strSql.Trim().Length > 1)
                        {
                            cmd.CommandText = strSql;
                            count += cmd.ExecuteNonQuery();
                        }
                    }
                    trans.Commit();
                    return count;
                }
                catch 
                {
                    
                    trans.Rollback();
                    return 0;
                }
            }
        }
        #endregion

        #region 执行带参数的SQL语句
        /// <summary>
        /// 执行一条计算查询结果语句,返回查询结果（object）.
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        cmd.CommandTimeout = 1000000;
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }
        public static object GetSingle(string SQLString, SqlTransaction trans)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cmd.CommandTimeout = 1000000;
                        PrepareCommand(cmd, trans.Connection, null, SQLString, null);
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句,返回查询结果（object）.
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cmd.CommandTimeout = 1000000;
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        throw e;
                    }
                }
            }
        }
        /// <summary>
        /// 执行查询语句，返回SqlDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(SQLString, connection);
                cmd.CommandTimeout = 1000000;
                try
                {
                    connection.Open();
                    SqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    return myReader;
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    throw e;
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    SqlDataAdapter command = new SqlDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }
      
        /// <summary>
        /// 添加事务处理
        /// </summary>
        /// <param name="SQLString"></param>
        /// <param name="trans"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public static DataSet Query(string SQLString, SqlTransaction trans, params SqlParameter[] cmdParms)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    cmd.CommandTimeout = 1000000;
                    DataSet ds = new DataSet();

                    PrepareCommand(cmd, trans.Connection, null, SQLString, cmdParms);
                    cmd.Transaction = trans;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds, "ds");

                    cmd.Parameters.Clear();
                    return ds;
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    throw e;
                }
            }


        }
        
        #endregion

        #region 扩展方法
        /// <summary>
        /// 批量更新数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="setPms">被修改</param>
        /// <param name="strWhere">条件</param>
        /// <returns></returns>
        public static int Update(string tableName,string setPms,string strWhere)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.Append(" Update ");
            sbWhere.Append(tableName);
            sbWhere.Append(" set ");
            sbWhere.Append(setPms);
            sbWhere.Append(" where ");
            sbWhere.Append(strWhere);
            return ExecuteSql(sbWhere.ToString());
        }
        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="setPms"></param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public static int Delete(string tableName,string setPms,string strWhere)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.Append(" Delete from ");
            sbWhere.Append(tableName);
            sbWhere.Append(" where ");
            sbWhere.Append(strWhere);
            return ExecuteSql(sbWhere.ToString());
        }


        #endregion

        #region  内部调用
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.CommandTimeout = 1000000;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (SqlParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }
        #endregion

        #region  在同一个链接中执行SQL
        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet QueryInConn(string SQLString, SqlConnection conn)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter command = new SqlDataAdapter(SQLString, conn);
            command.Fill(ds, "ds");
            return ds;
        }
        #endregion
    }
}
