using LinKin.BLLEngine;
using LinKin.BLLJIMP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinKin.BLLJIMP
{
    /// <summary>
    /// 用户 业务逻辑层
    /// </summary>
    public class BLLUserInfo:BLL
    {
        /// <summary>
        /// 根据用户id获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserInfo GetUserInfoById(string userId)
        {
            return Get<UserInfo>(string.Format("  UserId='{0}' ",userId));
        }
    }
}
