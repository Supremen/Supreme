using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace LinKin.Ken.Web.Handler
{
    /// <summary>
    /// BaseHandNoLogin 的摘要说明
    /// </summary>
    public class BaseHandNoLogin : IHttpHandler
    {
        /// <summary>
        /// 响应模型
        /// </summary>
        protected BaseResponse resp = new BaseResponse();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
           

            context.Response.Write("Hello World");
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