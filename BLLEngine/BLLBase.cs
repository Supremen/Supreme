using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LinKin.BLLEngine
{
    [Serializable]
    abstract public class BLLBase
    {
        protected virtual string GetTableName(string tableName)
        {
            return tableName;
        }

        #region Exists
        /// <summary>
        /// 是否存在列
        /// </summary>
        /// <param name="model"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public bool Exists(ModelTable model,string col)
        {
            List<string> columns = new List<string>();
            columns.Add(col);
            return Exists(model, columns);
        }

        public bool Exists(ModelTable model, List<string> cols)
        {
            Type t = model.GetType();
            Dictionary<string, string> columns = new Dictionary<string, string>();
            PropertyInfo[] properties = t.GetProperties();
            //缓存用的语句
            string where = "";
            foreach (string col in cols)
            {
                foreach (var p in properties)
                {
                    if (p.Name == col)
                    {
                        string value = p.GetValue(model, null).ToString();
                        columns.Add(col, value);
                        where += col + ":" + value + "_";
                        break;
                    }
                }
            }
            return false;
        }
        #endregion

        #region Get
        public T Get<T>(string strWhere) where T : ModelTable, new()
        {
            List<T> listModels = GetList<T>(1, strWhere, "");
            if (listModels.Count > 0)
            {
                return listModels[0];
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region GetList
        public List<T> GetList<T>(int top, string strWhere, string strOrder) where T : ModelTable, new()
        {
            T model = new T();
            Type t = model.GetType();
            string RealTableName = GetTableName(t.Name);
            DataSet ds = DALEngine.DALBase.Get(top, RealTableName, strWhere, strOrder);
            return DataSetToModelList<T>(ds);
        }
        public List<T> GetList<T>(string  strWhere) where T:ModelTable,new ()
        {
            return GetList<T>(0, strWhere, "");
        }
        public List<T> GetList<T>() where T:ModelTable,new()
        {
            return GetList<T>(0,"","");
        }
        

        #endregion

        #region GetColList
        public List<T> GetColList<T>(int pageSize,int pageIndex,string strWhere,string colName) where T:ModelTable,new()
        {
            T model =new T();
            Type t = model.GetType();
            string realTableName = GetTableName(t.Name);
            DataSet ds = DALEngine.DALBase.GetCol(pageSize,pageIndex,realTableName,colName,strWhere);
            return DataSetToModelList<T>(ds);
        }

        public List<T> GetColLost<T>(int pageSize,int pageIndex,string strWhere,string strOrder,string colName) where T:ModelTable,new()
        {
            T model = new T();
            Type t = model.GetType();
            string realTableName = GetTableName(t.Name);

            DataSet ds = DALEngine.DALBase.GetCol(pageSize,pageIndex,realTableName,colName,strWhere,strOrder);
            return DataSetToModelList<T>(ds);
        }
        #endregion

        #region GetCount
        public int GetCount<T>(string strWhere) where T:ModelTable,new ()
        {
            T model = new T();
            Type t = model.GetType();
            string realTableName = GetTableName(t.Name);
            return DALEngine.DALBase.GetCount(realTableName,strWhere);
        }

        public int GetCount<T>(string disCol,string strWhere) where T:ModelTable,new()
        {
            T model = new T();
            Type t = model.GetType();
            string realTableName = GetTableName(t.Name);
            return DALEngine.DALBase.GetCount(realTableName,disCol,strWhere);
        }
        #endregion

        #region GetLit
        public List<T> GetLit<T>(int pageIndex, int pageSize, string strWhere) where T : ModelTable, new()
        {
            T model = new T();
            Type t = model.GetType();
            string tableName = GetTableName(t.Name);
            DataSet ds = DALEngine.DALBase.Get(pageSize, pageIndex, tableName, strWhere);
            return DataSetToModelList<T>(ds);
        }
        public List<T> GetLit<T>(int pageSize, int pageIndex, string strWhere, string strOrder) where T : ModelTable, new()
        {
            T model = new T();
            Type t = model.GetType();
            string tableName = GetTableName(t.Name);
            DataSet ds = DALEngine.DALBase.Get(pageSize, pageIndex, tableName, strWhere, strOrder);
            return DataSetToModelList<T>(ds);
        }
        #endregion

        #region GetSingle
        /// <summary>
        /// 传入sql语句 返回单值
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public static object GetSingle(string strSql)
        {
            return LinKin.DALEngine.DALBase.GetSingle(strSql);
        }

        public static object GetSingle(string tableName,string colName,string strWhere)
        {
            string realTableName = tableName.Contains("HBB_") ? tableName : "HBB_" + tableName;
            return DALEngine.DALBase.GetSingle(realTableName,colName,strWhere);
        }

        public static object GetSingle(string strSql,BLLTransaction trans)
        {
            object result = DALEngine.DALBase.GetSingle(strSql);
            return result;
        }
        #endregion

        #region Add
        /// <summary>
        /// 添加 [传入一个实体]
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(ModelTable model)
        {
            Type t = model.GetType();
            string tableName = GetTableName(t.Name);
            return LinKin.DALEngine.DALBase.Add(tableName,model);
        }

        public bool Add(ModelTable model,BLLTransaction trans)
        {
            Type t = model.GetType();
            bool result = false;
            string realTableName = GetTableName(t.Name);
            if (trans == null)
            {
                result = DALEngine.DALBase.Add(realTableName,model);
            }
            else
            {
                result = DALEngine.DALBase.Add(realTableName,model,trans.Transaction);
            }
            return result;
        }

        public bool AddList<T>(List<T> modelList) where T:ModelTable,new()
        {
            BLLTransaction tran = new BLLTransaction();
            foreach (T model in modelList)
            {
                if (!Add(model,tran))
                {
                    tran.Rollback();
                    return false;
                }
            }
            tran.Commit();
            return true;
        }
        #endregion

        #region Update
        /// <summary>
        /// 修改 [传入一个实体]
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(ModelTable model)
        {
            Type t = model.GetType();
            string tableName = GetTableName(t.Name);
            return LinKin.DALEngine.DALBase.Update(tableName,model);
        }
        public bool Update(ModelTable model,BLLTransaction trans)
        {
            Type t = model.GetType();
            string realTableName = GetTableName(t.Name);
            bool result = false;
            if (trans == null)
            {
                result = DALEngine.DALBase.Update(realTableName,model);
            }
            else
            {
                result = DALEngine.DALBase.Update(realTableName,model,trans.Transaction);
            }
            return result;
        }

        public int Update(ModelTable model,string setPms,string strWhere)
        {
            Type t = model.GetType();

            string realTableName = GetTableName(t.Name);

            int result = DALEngine.DALBase.Update(realTableName,setPms,strWhere);

            return result;
        }

        public int Update(ModelTable model,string setPms,string strWhere,BLLTransaction trans)
        {
            Type t = model.GetType();
            string realTableNmae = GetTableName(t.Name);
            int result = 0;
            if(trans==null){
                result = DALEngine.DALBase.Update(realTableNmae,setPms,strWhere);
            }
            else
            {
                result = DALEngine.DALBase.Update(realTableNmae,setPms,strWhere,trans.Transaction);
            }
            return result;
        }

        public bool UpdateList(List<ModelTable> modelList)
        {
            foreach (ModelTable model in modelList)
            {
                if (!Update(model))
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region Delete
        public int Delete(ModelTable model, string strWhere, BLLTransaction trans)
        {
            if (string.IsNullOrEmpty(strWhere))
                return 0;
            Type t = model.GetType();

            string realTableName = GetTableName(t.Name);

            int result = 0;
            if (trans == null)
                result = DALEngine.DALBase.Delete(realTableName, strWhere);
            else
                result -= DALEngine.DALBase.Delete(realTableName,strWhere,trans.Transaction);
            return result;
        }

        public int Delete(ModelTable model,string strWhere)
        {
            if (strWhere.Length == 0)
                return 0;

            Type t = model.GetType();

            string realTableName = GetTableName(t.Name);

            int result = DALEngine.DALBase.Delete(realTableName,strWhere);

            return result;
        }

        public int Delete(ModelTable model)
        {
            Type t = model.GetType();
            string realTableName = GetTableName(t.Name);
            DALEngine.MetaTable metaTable = DALEngine.DALBase.GetMetas().Tables[realTableName];
            Dictionary<string, object> dicCol = new Dictionary<string, object>();
            PropertyInfo[] properInfo = t.GetProperties();
            if (metaTable.Keys.Count==0||metaTable.Keys[0].Count==0)
            {
                throw new Exception("该表没有主键 不能通过model删除");
            }
            foreach (var keyCol in metaTable.Keys[0])
            {
                foreach (PropertyInfo p in properInfo)
                {
                    if (p.Name == keyCol)
                    {
                        dicCol.Add(keyCol,p.GetValue(model,null));
                    }
                }
            }
            int result = DALEngine.DALBase.Delete(realTableName,dicCol,true);
            return result;
        }

        public int Delete(ModelTable model,BLLTransaction trans)
        {
            Type t = model.GetType();
            string realTableName = GetTableName(t.Name);
            DALEngine.MetaTable metaTable=DALEngine.DALBase.GetMetas().Tables[realTableName];
            Dictionary<string, object> dicCol = new Dictionary<string, object>();
            PropertyInfo[] properInfo = t.GetProperties();
            if (metaTable.Keys.Count == 0 || metaTable.Keys[0].Count == 0)
            {
                throw new Exception("该表没有主键不能通过model删除");
            }
            foreach (string keyCol in metaTable.Keys[0])
            {
                foreach (PropertyInfo p in properInfo)
                {
                    if (p.Name == keyCol)
                    {
                        dicCol.Add(keyCol,p.GetValue(model,null));
                    }
                }
            }
            int result = DALEngine.DALBase.Delete(realTableName,dicCol,true,trans.Transaction);
            return result;
        }

        #endregion

        #region Query
        public static DataSet Query(string strSql)
        {
            return DALEngine.DALBase.Query(strSql);
        }

        public static List<T> Query<T>(string strSql) where T:ModelTable,new()
        {
            DataSet ds = DALEngine.DALBase.Query(strSql);
            return DataSetToModelList<T>(ds);
        }
        #endregion

        #region ExecuteSql
        public static int ExecuteSql(string strSql)
        {
            int result = DALEngine.DALBase.ExecuteSql(strSql);
            return result;
        }


        public static int ExecuteSql(string strSql,BLLTransaction trans)
        {
            return  DALEngine.DALBase.ExecuteSql(strSql,trans.Transaction);
        }

        public static int ExecuteSqlTran(List<String> SqlStrList)
        {
            return DALEngine.DALBase.ExecuteSqlTran(SqlStrList);
        }
        #endregion

        #region DataSetToModelList
        /// <summary>
        /// DataSet转List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static List<T> DataSetToModelList<T>(DataSet ds) where T : ModelTable, new()
        {
            T m = new T();
            Type type = m.GetType();
            List<T> modelList = new List<T>();
            PropertyInfo[] properties = type.GetProperties();

            for (int i = 0; i < ds.Tables[0].Rows.Count; ++i)
            {
                T model = new T();
                foreach (DataColumn dc in ds.Tables[0].Columns)
                {
                    PropertyInfo pi = properties.FirstOrDefault(p => p.Name == dc.ColumnName);
                    if (pi != null && ds.Tables[0].Rows[i][pi.Name] != DBNull.Value)
                    {
                        try
                        {
                            pi.SetValue(model, ds.Tables[0].Rows[i][pi.Name], null);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
                modelList.Add(model);
            }
            return modelList;
        }
        #endregion

    }
}
