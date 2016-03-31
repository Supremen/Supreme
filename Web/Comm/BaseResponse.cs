using System;
using System.Collections.Generic;
using System.Linq;

namespace LinKin.Ken.Web
{
    /// <summary>
    /// 响应模型 
    /// </summary>
    public class BaseResponse
    {
        /// <summary>
        /// 是否成功  true成功  false失败
        /// </summary>
        public bool status { get; set; }
        
        /// <summary>
        /// 状态码  对应枚举 APIErrCode
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 提示信息
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 业务层结果
        /// </summary>
        public dynamic result { get; set; }
    }
}