using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinKin.BLLJIMP.Model
{
    /// <summary>
    /// 用户
    /// </summary>
    [Serializable]
    public class UserInfo : BLLEngine.ModelTable
    {
        /// <summary>
        /// 空构造函数
        /// </summary>
        public UserInfo()
        {

        }

        /// <summary>
        /// 自增id  主键
        /// </summary>
        public int AutoID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string TrueName { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 座机
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// 扩展字段1
        /// </summary>
        public string Ex1 { get; set; }

        /// <summary>
        /// 扩展字段2
        /// </summary>
        public string Ex2 { get; set; }

        /// <summary>
        /// 扩展字段3
        /// </summary>
        public string Ex3 { get; set; }

        /// <summary>
        /// 扩展字段4
        /// </summary>
        public string Ex4 { get; set; }

        /// <summary>
        /// 扩展字段5
        /// </summary>
        public string Ex5 { get; set; }

        /// <summary>
        /// 扩展字段6
        /// </summary>
        public string Ex6 { get; set; }

        /// <summary>
        /// 扩展字段7
        /// </summary>
        public string Ex7 { get; set; }

        /// <summary>
        /// 扩展字段8
        /// </summary>
        public string Ex8 { get; set; }

        /// <summary>
        /// 扩展字段9
        /// </summary>
        public string Ex9 { get; set; }

        /// <summary>
        /// 扩展字段10
        /// </summary>
        public string Ex10 { get; set; }

    }
}
