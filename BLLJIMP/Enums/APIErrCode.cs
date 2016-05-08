using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinKin.BLLJIMP.Enums
{
    /// <summary>
    /// 错误代码
    /// </summary>
    public enum APIErrCode
    {
        /// <summary>
        /// 用户未登录
        /// </summary>
        UserIsNotLogin=10010,
        /// <summary>
        /// 必填参数为空
        /// </summary>
        ParameterIsEmpty=10011,
        /// <summary>
        /// 异常错误
        /// </summary>
        Exception=10012,
        /// <summary>
        /// 对象不存在
        /// </summary>
        ObjectNotExist=10013,
        /// <summary>
        /// 操作失败
        /// </summary>
        Operation=10014

        

    }
}
