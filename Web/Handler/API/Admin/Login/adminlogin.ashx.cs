using LinKin.BLLJIMP;
using LinKin.BLLJIMP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinKin.Ken.Web.Handler.API.Admin.Login
{
    /// <summary>
    /// adminlogin 的摘要说明 后台登陆 [2016-04-02]
    /// </summary>
    public class adminlogin : BaseHandAdminNoLogin
    {
        /// <summary>
        /// 用户信息 业务逻辑
        /// </summary>
        BLLUserInfo bllUser = new BLLUserInfo();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string userId = context.Request["_userid"];
            string passWord = context.Request["_password"];
            if (string.IsNullOrEmpty(userId))
            {
                resp.msg = "_userid 参数为空";
                resp.code = (int)BLLJIMP.Enums.APIErrCode.ParameterIsEmpty;
                context.Response.Write(LinKin.Common.JsonHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(passWord))
            {
                resp.msg = "_password 参数为空";
                resp.code = (int)BLLJIMP.Enums.APIErrCode.ParameterIsEmpty; 
                context.Response.Write(LinKin.Common.JsonHelper.ObjectToJson(resp));
                return;
            }
            UserInfo model = bllUser.GetUserInfoById(userId);
            if (model == null)
            {
                resp.msg = "用户不存在";
                resp.code = (int)BLLJIMP.Enums.APIErrCode.ObjectNotExist;
                context.Response.Write(LinKin.Common.JsonHelper.ObjectToJson(resp));
                return;
            }
            if (model.Password != passWord)
            {
                resp.msg = "密码错误";
                context.Response.Write(LinKin.Common.JsonHelper.ObjectToJson(resp));
                return;
            }
            resp.status = true;
            context.Response.Write(LinKin.Common.JsonHelper.ObjectToJson(resp));
        }

    }
}