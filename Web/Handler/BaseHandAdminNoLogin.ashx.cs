using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.SessionState;

namespace LinKin.Ken.Web.Handler
{
    /// <summary>
    /// BaseHandAdminNoLogin 的摘要说明
    /// 基类处理文件 不需要登陆 [后台]
    /// </summary>
    public class BaseHandAdminNoLogin : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 响应模型
        /// </summary>
        protected BaseResponse resp = new BaseResponse();
        public virtual void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            this.GetType().GetMethod("ProcessRequest").Invoke(this, new[] { context });
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