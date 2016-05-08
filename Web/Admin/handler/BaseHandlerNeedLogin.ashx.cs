using LinKin.BLLJIMP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.SessionState;

namespace LinKin.Ken.Web.Admin.handler
{
    /// <summary>
    /// BaseHandlerNeedLogin 的摘要说明
    /// </summary>
    public class BaseHandlerNeedLogin : IHttpHandler,IReadOnlySessionState
    {
        /// <summary>
        /// 用户逻辑层 
        /// </summary>
        BLLJIMP.BLLUserInfo bllUser = new BLLJIMP.BLLUserInfo();
        /// <summary>
        /// 当前用户信息
        /// </summary>
        UserInfo currentUserInfo;
        /// <summary>
        /// 响应模型
        /// </summary>
        protected BaseResponse resp = new BaseResponse();
        public virtual void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string result = "false";
            try
            {
                /*
                 * 检查是否登陆
                 */
                if (!bllUser.IsLogin)
                {
                    resp.msg = "请先登录";
                    resp.code = (int)BLLJIMP.Enums.APIErrCode.UserIsNotLogin;
                    context.Response.Write(LinKin.Common.JsonHelper.ObjectToJson(resp));
                    return;
                }
                else
                {
                    currentUserInfo = bllUser.GetCurrentUserInfo();
                }
                    
                string action=context.Request["action"];
                if (!string.IsNullOrEmpty(action))
                {
                    /*
                     * 指定搜索非公有方法 
                    */
                    MethodInfo method = this.GetType().GetMethod(action,BindingFlags.NonPublic|BindingFlags.Instance|BindingFlags.IgnoreCase);
                    if (method == null)
                    {
                        resp.code = (int)BLLJIMP.Enums.APIErrCode.ParameterIsEmpty;
                        resp.msg = "action not exist";
                        context.Response.Write(LinKin.Common.JsonHelper.ObjectToJson(resp));
                        return;
                    }
                    result = Convert.ToString(method.Invoke(this, new[] { context }));//调用方法
                }
                else
                {
                    resp.code = (int)BLLJIMP.Enums.APIErrCode.ParameterIsEmpty;
                    resp.msg = "action not exist";
                    result = LinKin.Common.JsonHelper.ObjectToJson(resp);
                }
            }
            catch (Exception ex)
            {
                resp.msg = ex.Message;
                resp.code = (int)BLLJIMP.Enums.APIErrCode.Exception;
                result = LinKin.Common.JsonHelper.ObjectToJson(resp);
            }
            context.Response.Write(result);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}