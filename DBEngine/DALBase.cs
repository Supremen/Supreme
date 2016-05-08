using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LinKin.DALEngine
{
    [Serializable]
    abstract public class DALBase
    {

        private static MetaTables _metaTables = null;

        #region GetCount
        /// <summary>
        /// 获取数量
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
       public static int GetCount(string tableName,string strWhere)
        {
            int count = 0;
            if (string.IsNullOrEmpty(strWhere))
                count = (int)DBHelper.GetSingle(string.Format("SELECT COUNT(1) FROM {0}", tableName));
            else
                count = (int)DBHelper.GetSingle(string.Format("SELECT COUNT(1) FROM {0} WHERE {1} ",tableName,strWhere));
            return count;
        }
        /// <summary>
        /// 获取去重复数量
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="disCol"></param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public static int GetCount(string tableName, string disCol, string strWhere)
        {
            int count = 0;
            if (string.IsNullOrEmpty(strWhere))
                count = (int)DBHelper.GetSingle(string.Format("SELECT COUNT (DISTINCT({0})) FROM {1}", disCol, tableName));
            else
                count = (int)DBHelper.GetSingle(string.Format("SELECT COUNT(DISTINCT({0}) FROM {1} WHERE {2})",disCol,tableName,strWhere));
            return count;
        }
        #endregion


        #region GetSingle
        /// <summary>
        /// 传入SQL语句 查单值
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public static object GetSingle(string strSql)
        {
            return DBHelper.GetSingle(strSql);
        }
        public static object GetSingle(string tableName,string colName,string strWhere)
        {
            MetaTable metaTable = DALEngine.DALBase.GetMetas().Tables[tableName];
            StringBuilder strSql = new StringBuilder();

            strSql.AppendFormat("select top 1 {0} from {1} ",colName,tableName);

            if (!string.IsNullOrEmpty(strWhere))
                strSql.AppendFormat(" where {0} ",strWhere);

            DataSet ds = DBHelper.Query(strSql.ToString());

            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][0];
            else
                return null;

        }
        #endregion

        #region ExecuteSql
        /// <summary>
        /// 传入增删改类型的SQL语句
        /// </summary>
        /// <param name="SQLString"></param>
        /// <returns></returns>
        public static int ExecuteSql(string SQLString)
        {
            return DBHelper.ExecuteSql(SQLString);
        }

        public static int ExecuteSql(string SQLString, SqlTransaction trans)
        {
            int count = 0;
            try
            {
                if (trans == null)
                    count = ExecuteSql(SQLString);
                else
                    count = DBHelper.ExecuteSql(SQLString, trans);
                return count;
            }
            catch (Exception ex)
            {

                return -1;
            }
        }

        /// <summary>
        /// 批量执行增删改的SQL语句
        /// </summary>
        /// <param name="SQLStringList"></param>
        /// <returns></returns>
        public static int ExecuteSqlTran(List<string> SQLStringList)
        {
            return DBHelper.ExecuteSqlTran(SQLStringList);
        }
        #endregion

        #region  Add
        public static  bool Add(string tableName,object model)
        {
            StringBuilder strSql;
            List<SqlParameter> parameters;
            GetAddSqlStrParams(tableName, model,out  strSql,out parameters);
            int rows = DBHelper.ExecuteSql(strSql.ToString(),SqlParameterListToArray(parameters));
            if (rows > 0)
                return true;
            else
                return false;
        }

        public static bool Add(string tableName,object model,SqlTransaction trans)
        {
            StringBuilder strSql;
            List<SqlParameter> parameters;
            GetAddSqlStrParams(tableName, model, out  strSql, out parameters);

            int rows = 0;
            try
            {
                if (trans == null)
                    return Add(tableName, model);
                else
                    rows = DBHelper.ExecuteSql(strSql.ToString(),trans,SqlParameterListToArray(parameters));
            }
            catch (Exception ex)
            {

                return false;
            }
            if (rows > 0)
                return true;
            else
                return false;
        }

        
        private static void GetAddSqlStrParams(string tableName,object model,out StringBuilder strSql,out List<SqlParameter> parameters)
        {
            Type t=model.GetType();

            MetaTable metaTable=GetMetas().Tables[tableName];

            PropertyInfo[] property=t.GetProperties();

            strSql=new StringBuilder();
            parameters = new List<SqlParameter>();

            StringBuilder strColumns = new StringBuilder();
            StringBuilder strValues = new StringBuilder();

            int i = 0;
            foreach (PropertyInfo p in property)
            {
                object val = p.GetValue(model,null);

                if (!metaTable.Columns.ContainsKey(p.Name) || null == val || p.Name.ToLower().Equals("autoid")) continue;
                if (i == 0)
                {
                    strColumns.AppendFormat("{0} ",p.Name);
                    strValues.AppendFormat("@{0} ",p.Name);
                }
                else
                {
                    strColumns.AppendFormat(",{0} ",p.Name);
                    strValues.AppendFormat(",@{0} ",p.Name);
                }

                parameters.Add(new SqlParameter(string.Format("@{0} ",p.Name),metaTable.Columns[p.Name]));
                parameters[i].Value = val;
                i++;
                   
            }
            strSql.AppendFormat("insert into {0}",metaTable.Name);
            strSql.AppendFormat("({0}) values ({1})",strColumns,strValues);
        }
        #endregion

        #region Update
        public static bool Update(string tableName,object model)
        {
            StringBuilder strSql;
            List<SqlParameter> parameters;
            GetUpdateStrParams(tableName,model,out strSql,out parameters);
            int rows = DBHelper.ExecuteSql(strSql.ToString(),SqlParameterListToArray(parameters));
            if (rows > 0)
                return true;
            else
                return false;
        }

        public static bool Update(string tableName,object model,SqlTransaction trans)
        {
            StringBuilder strSql;
            List<SqlParameter> parameters;
            GetUpdateStrParams(tableName,model,out strSql,out parameters);
            int rows = 0;
            try
            {
                if (trans == null)
                    return Update(tableName, model);
                else
                    rows = DBHelper.ExecuteSql(strSql.ToString(), trans, SqlParameterListToArray(parameters));
            }
            catch (Exception ex)
            {
                return false;
            }

            if (rows > 0)
                return true;
            else
                return false;
        }

        private static void GetUpdateStrParams(string tableName,object model,out StringBuilder strSql,out List<SqlParameter> parameters)
        {
            Type t = model.GetType();
            PropertyInfo[] properties = t.GetProperties();

            strSql = new StringBuilder();
            parameters = new List<SqlParameter>();

            MetaTable metaTable = GetMetas().Tables[tableName];
            strSql.AppendFormat("Update {0} set ",metaTable.Name);

            int paramIndex = 0;
            foreach (var col in metaTable.Columns)
            {
                if (metaTable.Keys[0].Contains(col.Key)) continue;
                PropertyInfo p = properties.FirstOrDefault(k => k.Name == col.Key);
                if (p == null) continue;

                object val = p.GetValue(model, null);
                if (paramIndex == 0)
                    strSql.AppendFormat(" {0}=@{0}", col.Key);
                else
                    strSql.AppendFormat(",{0}=@{0}", col.Key);

                parameters.Add(new SqlParameter(string.Format("@{0}",col.Key),metaTable.Columns[col.Key]));
                parameters[paramIndex++].Value = val;
            }
            if (metaTable.Keys.Count == 0 || metaTable.Keys[0].Count == 0)
                throw new Exception("没有主键,不能执行修改");

            int keyIndex = 0;
            foreach (var key in metaTable.Keys[0])
            {
                if (keyIndex == 0)
                    strSql.AppendFormat(" where {0}=@{0}", key);
                else
                    strSql.AppendFormat(" and {0}=@{0}", key);

                keyIndex++;
                parameters.Add(new SqlParameter(string.Format("@{0}", key), metaTable.Columns[key]));

                PropertyInfo p = properties.FirstOrDefault(k => k.Name == key);
                parameters[paramIndex++].Value = p.GetValue(model, null);
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="setPms"></param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public static int Update(string tableName, string setPms, string strWhere)
        {
            return DBHelper.Update(tableName, setPms, strWhere);
        }
        public static int Update(string tableName, string setPms, string strWhere, SqlTransaction trans)
        {
            int rows = 0;
            try
            {
                if (trans == null)
                    rows = Update(tableName, setPms, strWhere);
                else
                    rows = DBHelper.ExecuteSql(string.Format("UPDATE {0} SET {1} WHERE {2} ", tableName, setPms, strWhere), trans);
                return rows;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        #endregion

        #region Query
        public static DataSet Query(string SqlStr)
        {
            return DBHelper.Query(SqlStr);
        }
        #endregion


        #region Delete
        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public static int Delete(string tableName,string strWhere)
        {
            return DBHelper.ExecuteSql(string.Format("DELETE FROM {0} WHERE {1}",tableName,strWhere));
        }
        public static int Delete(string tableName,string strWhere,SqlTransaction trans)
        {
            int count = 0;
            try
            {
                if (trans == null)
                    count = Delete(tableName, strWhere);
                else
                    count = DBHelper.ExecuteSql(string.Format("DELETE FROM {0} WHERE {1}",tableName,strWhere),trans);
                return count;
            }
            catch (Exception ex)
            {

                return -1;
            }
        }

        public static int Delete(string tableName,Dictionary<string,object> dicCol,bool Paras)
        {
            StringBuilder strSql;
            List<SqlParameter> parameters;
            GetDeleteStrParams(tableName, dicCol, Paras, out strSql, out parameters);
            return DBHelper.ExecuteSql(strSql.ToString(), SqlParameterListToArray(parameters));
        }

        public static  int Delete(string tableName,Dictionary<string,object> dicCol,bool param,SqlTransaction trans)
        {
            StringBuilder strSql;
            List<SqlParameter> parameters;
            GetDeleteStrParams(tableName, dicCol, param,out strSql,out parameters);
            int rows = 0;
            try
            {
                if (trans == null)
                    rows = Delete(tableName, dicCol, param);
                else
                    rows = DBHelper.ExecuteSql(strSql.ToString(), trans, SqlParameterListToArray(parameters));
                return rows;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        private static void GetDeleteStrParams(string tableName,Dictionary<string,object> dicCol,bool Params,out StringBuilder strSql,out List<SqlParameter> parameters)
        {
            MetaTable metaTable = DALBase.GetMetas().Tables[tableName];
            strSql = new StringBuilder();
            strSql.AppendFormat(" Delete from {0} ",metaTable.Name);

            parameters = new List<SqlParameter>();
            int paraIndex = 0;
            foreach (KeyValuePair<string,object> kv in dicCol)
            {
                if (paraIndex == 0)
                {
                    strSql.AppendFormat(" {0} {1} =@{1}",kv.Key);
                }
                else
                {
                    strSql.AppendFormat(" {0} {1} =@{1}", Params==true?"and":"or",kv.Key);
                }
                parameters.Add(new SqlParameter(string.Format("@{0}",kv.Key),metaTable.Columns[kv.Key]));
                parameters[paraIndex++].Value = kv.Value;
            }
        }

        #endregion

        #region Get
        public static DataSet Get(int top, string tableName, string strWhere, string strOrder)
        {
            MetaTable metaTable = DALBase.GetMetas().Tables[tableName];
            StringBuilder strSql = new StringBuilder();
            if (top > 0)
            {
                strSql.AppendFormat("select top {0} ", top);
            }
            else
            {
                strSql.AppendFormat("select ");
            }

            int i = 0;
            foreach (KeyValuePair<string, SqlDbType> kv in metaTable.Columns)
            {
                if (i == 0)
                {
                    strSql.AppendFormat(" {0}", kv.Key);
                }
                else
                {
                    strSql.AppendFormat(", {0}", kv.Key);
                }
                ++i;
            }
            strSql.AppendFormat(" from {0} ", metaTable.Name);

            //strWhere值为空的时候where子句不加
            if (strWhere != null && strWhere != "")
                strSql.AppendFormat(" where {0}", strWhere);

            if (strOrder != null && strOrder.Trim().Length != 0)
            {
                strSql.AppendFormat(" order by {0}", strOrder);
            }

            DataSet ds = DBHelper.Query(strSql.ToString());
            ds.Tables[0].TableName = tableName;
            return ds;
        }

        public static DataSet Get(int pageSize, int pageIndex, string tableName, string strWhere)
        {
            MetaTable metaTable = DALEngine.DALBase.GetMetas().Tables[tableName];
            StringBuilder strOrder = new StringBuilder();
            foreach (var col in metaTable.Keys[0])
            {
                strOrder.Append(string.Format(", {0}", col));
            }
            strOrder = strOrder.Remove(0, 1);
            return Get(pageSize, pageIndex, tableName, strWhere, strOrder.ToString());
        }

        static public DataSet Get(int pageSize, int pageIndex, string tableName, string strWhere, string strOrder)
        {
            MetaTable metaTable = DALEngine.DALBase.GetMetas().Tables[tableName];
            StringBuilder strSql = new StringBuilder();

            if (strWhere != null && strWhere.Trim() != string.Empty)
            {
                strSql.AppendFormat(@"select top {4} * from 
                                (select top {5} row_number() over(order by {0}) as COL_ROWNUMBER, * from {1} where {2} ) TABLE_ORDERDATA
                                where COL_ROWNUMBER > {3} order by {0}",
                                                                       strOrder,
                                                                       tableName,
                                                                       strWhere,
                                                                       (pageIndex - 1) * pageSize,
                                                                       pageSize,
                                                                       pageSize * pageIndex
                                                                       );
            }
            else
            {
                strSql.AppendFormat(@"select top {3} * from 
                                (select top {4} row_number() over(order by {0}) as COL_ROWNUMBER, * from {1} ) TABLE_ORDERDATA
                                where COL_ROWNUMBER > {2} order by {0}",
                                                                       strOrder,
                                                                       tableName,
                                                                       (pageIndex - 1) * pageSize,
                                                                       pageSize,
                                                                       pageSize * pageIndex

                                                                       );
            }

            DataSet ds = DBHelper.Query(strSql.ToString());
            ds.Tables[0].TableName = tableName;
            return ds;
        }

        public static DataSet GetCol(int pageSize, int pageIndex, string tableName, string colName, string strWhere, string strOrder)
        {
            MetaTable metaTable = DALEngine.DALBase.GetMetas().Tables[tableName];
            StringBuilder strSql = new StringBuilder();
            if (strWhere != null && strWhere.Trim() != string.Empty)
            {
                strSql.AppendFormat(@"select top {4} * from 
                                (select top {5} row_number() over(order by {0}) as COL_ROWNUMBER, {6} from {1} where {2} ) TABLE_ORDERDATA
                                where COL_ROWNUMBER > {3} order by {0}",
                                                                       strOrder,
                                                                       tableName,
                                                                       strWhere,
                                                                       (pageIndex - 1) * pageSize,
                                                                       pageSize,
                                                                       pageSize * pageIndex,
                                                                       colName
                                                                       );
            }
            else
            {
                strSql.AppendFormat(@"select top {3} * from 
                                (select top {4} row_number() over(order by {0}) as COL_ROWNUMBER, {5} from {1} ) TABLE_ORDERDATA
                                where COL_ROWNUMBER > {2} order by {0}",
                                                                       strOrder,
                                                                       tableName,
                                                                       (pageIndex - 1) * pageSize,
                                                                       pageSize,
                                                                       pageSize * pageIndex,
                                                                       colName
                                                                       );
            }

            DataSet ds = DBHelper.Query(strSql.ToString());
            ds.Tables[0].TableName = tableName;
            return ds;
        }
        #endregion

        #region GetCol
        public static DataSet GetCol(int pageSize,int pageIndex,string tableName,string colName,string strWhere)
        {
            MetaTable metaTable = DALBase.GetMetas().Tables[tableName];
            StringBuilder strOrder = new StringBuilder();
            foreach (var item in metaTable.Keys[0])
            {
                strOrder.Append(string.Format(", {0}",item));
            }
            strOrder = strOrder.Remove(0,1);
            return GetCol(pageSize,pageIndex,tableName,colName,strWhere,strOrder.ToString());
        }

        #endregion

        public static MetaTables GetMetas()
        {
            if (null == _metaTables)
            {
                _metaTables = new MetaTables();
                try
                {
                    #region 新方法
                    DataTable dsTable = DBHelper.Query("SELECT name FROM sysobjects WHERE (xtype = 'U') order by name ").Tables[0];

                    DataTable dsFieldTable = DBHelper.Query(@"Select T0.name as 'colname', T2.name as 'coltype' ,T1.name 'tablename' from syscolumns T0 
                    							inner join sysobjects T3 on T3.xtype = 'U'
                                                inner join sysobjects T1 on T0.id = T1.id and T1.name = T3.name
                                                inner join systypes T2 on T0.xtype = T2.xtype and T2.name <> 'sysname' 
                                                order by T1.name,T0.colid ").Tables[0];

                    DataTable dsKeyTable = DBHelper.Query(@"select T0.indid as 'keyindex', T2.name as 'colname',T1.name 'tablename' from sysindexkeys T0 
                                                join sysobjects T1 on T0.id = T1.id and T1.xtype = 'U'
                                                join sys.syscolumns T2 on T0.colid = T2.colid and T1.id = T2.id
                                                order by T1.name,T0.indid").Tables[0];

                    foreach (DataRow dsRow in dsTable.Rows)
                    {
                        MetaTable metaTable = new MetaTable(dsRow[0].ToString());
                        string tableName = dsRow[0].ToString();
                        DataRow[] rowFields = dsFieldTable.Select(string.Format(" tablename='{0}' ", tableName));
                        DataRow[] rowKeys = dsKeyTable.Select(string.Format(" tablename='{0}' ", tableName));

                        foreach (DataRow nowField in rowFields)
                        {
                            metaTable.Columns.Add(nowField["colname"].ToString(), GetColType(nowField["coltype"].ToString()));
                        }

                        if (rowKeys.Length > 0)
                        {
                            int indid = int.Parse(rowKeys[0]["keyindex"].ToString());
                            List<string> key = new List<string>();
                            foreach (DataRow nowKey in rowKeys)
                            {
                                int keyindex = int.Parse(nowKey["keyindex"].ToString());
                                if (keyindex == indid)
                                {
                                    key.Add(nowKey["colname"].ToString());
                                }
                                else
                                {
                                    break;
                                }
                            }
                            metaTable.Keys.Add(key);
                        }
                        _metaTables.Tables.Add(tableName, metaTable);
                    }
                    #endregion
                  
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                }
            }
            return _metaTables;
        }

        public static SqlParameter[] SqlParameterListToArray(List<SqlParameter> parameterList)
        {
            SqlParameter[] arrayParameter = new SqlParameter[parameterList.Count];
            for (int i = 0; i < parameterList.Count; i++)
            {
                arrayParameter[i] = parameterList[i];
            }
            return arrayParameter;
        }

        public static SqlDbType GetColType(string colType)
        {
            switch (colType)
            {
                case "image":
                    return SqlDbType.Image;
                case "text":
                    return SqlDbType.Text;
                case "uniqueidentifier":
                    return SqlDbType.UniqueIdentifier;
                case "date":
                    return SqlDbType.Date;
                case "time":
                    return SqlDbType.Time;
                case "datetime2":
                    return SqlDbType.DateTime2;
                case "datetimeoffset":
                    return SqlDbType.DateTimeOffset;
                case "tinyint":
                    return SqlDbType.TinyInt;
                case "smallint":
                    return SqlDbType.SmallInt;
                case "int":
                    return SqlDbType.Int;
                case "smalldatetime":
                    return SqlDbType.SmallDateTime;
                case "real":
                    return SqlDbType.Real;
                case "money":
                    return SqlDbType.Money;
                case "datetime":
                    return SqlDbType.DateTime;
                case "float":
                    return SqlDbType.Float;
                case "sql_variant":
                    return SqlDbType.Variant;
                case "ntext":
                    return SqlDbType.NText;
                case "bit":
                    return SqlDbType.Bit;
                case "decimal":
                    return SqlDbType.Decimal;
                case "smallmoney":
                    return SqlDbType.SmallMoney;
                case "bigint":
                    return SqlDbType.BigInt;
                case "varbinary":
                    return SqlDbType.VarBinary;
                case "varchar":
                    return SqlDbType.VarChar;
                case "binary":
                    return SqlDbType.Binary;
                case "char":
                    return SqlDbType.Char;
                case "timestamp":
                    return SqlDbType.Timestamp;
                case "nvarchar":
                    return SqlDbType.NVarChar;
                case "nchar":
                    return SqlDbType.NChar;
                case "xml":
                    return SqlDbType.Xml;
                default:
                    return SqlDbType.NVarChar;
            }
        }
    }
}
