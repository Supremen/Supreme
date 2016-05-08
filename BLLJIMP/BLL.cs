using LinKin.BLLEngine;
using LinKin.BLLJIMP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace LinKin.BLLJIMP
{
    /// <summary>
    /// 业务逻辑基类
    /// </summary>
    [Serializable]
    public class BLL:BLLBase
    {
        /// <summary>
        /// 重写数据库表名
        /// </summary>
        /// <param name="modelName"></param>
        /// <returns></returns>
        protected override string GetTableName(string modelName)
        {
            string tableName = modelName.EndsWith("Ex", true, null) ? modelName.Substring(0, modelName.Length - 2) : modelName;
            return "HBB_" + tableName;
        }
        /// <summary>
        /// 当前站点所有者
        /// </summary>
        public string WebsiteOwner
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["WebsiteOwner"] != null)
                    return (string)System.Web.HttpContext.Current.Session["WebsiteOwner"];
                return null;
            }
        }
        /// <summary>
        /// 检查是否登陆 已登陆返回true   未登录返回false
        /// </summary>
        public bool IsLogin
        {
            get {
                if (HttpContext.Current.Session["UserId"] != null)
                {
                    if (!string.IsNullOrEmpty(HttpContext.Current.Session["UserId"].ToString()))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// 获取当前登录用户信息
        /// </summary>
        /// <returns></returns>
        public UserInfo GetCurrentUserInfo()
        {
            try
            {
                return Get<UserInfo>(string.Format(" UserId='{0}' ",HttpContext.Current.Session["UserId"]));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取当前登录用户名
        /// </summary>
        /// <returns></returns>
        public string GetCurrentUserId()
        {
            try
            {
                return HttpContext.Current.Session["UserId"].ToString();
            }
            catch (Exception ex)
            {

                return "";
            }
        }

        /// <summary>
        /// 获取GUID
        /// </summary>
        /// <param name="tran"></param>
        /// <returns></returns>
        public string GetGUID(BLLJIMP.Enums.TransacType tran)
        {
            string strSql = string.Format(@"insert into HBB_GUID (UserId,Remark,CreateDate) 
                        values ('{0}',{1},GETDATE()) SELECT @@IDENTITY"
                        ,GetCurrentUserId(),(int)tran);
            return GetSingle(strSql).ToString();
        }



    }
}
