using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace LinKin.Ken.Web.Handler
{
    /// <summary>
    /// BaseHandAdminNoLogin 的摘要说明
    /// 基类处理文件 不需要登陆 [后台]
    /// </summary>
    public class BaseHandAdminNoLogin : IHttpHandler
    {
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
                string action = context.Request["action"];
                if (!string.IsNullOrEmpty(action))
                {
                    //找到方法BindingFlags.NonPublic指定搜索非公有方法 
                    MethodInfo method = this.GetType().GetMethod(action, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    if (method == null)
                    {
                        resp.msg = "action not exist";
                        resp.code = (int)LinKin.BLLJIMP.Enums.APIErrCode.ParameterIsEmpty;
                        context.Response.Write(LinKin.Common.JsonHelper.ObjectToJson(resp));
                        return;
                    }
                    //调用方法
                    result = Convert.ToString(method.Invoke(this, new[] { context }));
                }
                else
                {
                    resp.msg = "action not exist";
                    resp.code = (int)LinKin.BLLJIMP.Enums.APIErrCode.ParameterIsEmpty;
                    context.Response.Write(LinKin.Common.JsonHelper.ObjectToJson(resp));
                    return;
                }
            }
            catch (Exception ex)
            {
                resp.code = (int)LinKin.BLLJIMP.Enums.APIErrCode.Exception;
                resp.msg = ex.Message;
                context.Response.Write(LinKin.Common.JsonHelper.ObjectToJson(resp));
                return;
            }
            //返回Json数据
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