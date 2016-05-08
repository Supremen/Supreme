using LinKin.BLLJIMP;
using LinKin.BLLJIMP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace LinKin.Ken.Web.Admin.handler
{
    /// <summary>
    /// Login 的摘要说明  [后台登陆]
    /// </summary>
    public class NoLoginHandler : IHttpHandler,IRequiresSessionState
    {
        /// <summary>
        /// 响应模型
        /// </summary>
        protected BaseResponse resp = new BaseResponse();
        /// <summary>
        /// 用户信息 业务逻辑
        /// </summary>
        BLLUserInfo bllUser = new BLLUserInfo();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string result = "false";
            string action=context.Request["action"];
            switch (action)
            {
                case "AdminLogin":
                    result = AdminLogin(context);
                    break;
            }
            context.Response.Write(result);
        }

        /// <summary>
        /// 登陆 [传入用户名和密码]
        /// </summary>
        /// <returns></returns>
        private string AdminLogin(HttpContext context)
        {
            string userId = context.Request["username"];
            string passWord = context.Request["userpwd"];
            if (string.IsNullOrEmpty(userId))
            {
                resp.msg = "username 参数为空";
                resp.code = (int)BLLJIMP.Enums.APIErrCode.ParameterIsEmpty;
                return LinKin.Common.JsonHelper.ObjectToJson(resp);
                ;
            }
            if (string.IsNullOrEmpty(passWord))
            {
                resp.msg = "userpwd 参数为空";
                resp.code = (int)BLLJIMP.Enums.APIErrCode.ParameterIsEmpty;
                return LinKin.Common.JsonHelper.ObjectToJson(resp);
            }
            UserInfo model = bllUser.GetUserInfoById(userId);
            if (model == null)
            {
                resp.msg = "用户不存在";
                resp.code = (int)BLLJIMP.Enums.APIErrCode.ObjectNotExist;
                return LinKin.Common.JsonHelper.ObjectToJson(resp);
            }
            if (model.Password != passWord)
            {
                resp.msg = "密码错误";
                return LinKin.Common.JsonHelper.ObjectToJson(resp);
            }
            context.Session["UserId"] = model.UserId;
            context.Session["UserInfo"] = model;
            resp.status = true;
            return LinKin.Common.JsonHelper.ObjectToJson(resp);
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