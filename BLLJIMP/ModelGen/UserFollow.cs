using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinKin.BLLJIMP.Model
{
    /// <summary>
    /// 用户关注
    /// </summary>
    [Serializable]
    public class UserFollow:BLLEngine.ModelTable
    {
        /// <summary>
        /// 空构造函数
        /// </summary>
        public UserFollow()
        {

        }

        /// <summary>
        /// 自增ID 主键
        /// </summary>
        public int AutoID { get; set; }

        /// <summary>
        /// 关注着
        /// </summary>
        public string FromUserId { get; set; }

        /// <summary>
        /// 被关注着
        /// </summary>
        public string ToUserId { get; set; }

        /// <summary>
        /// 站点
        /// </summary>
        public string WebsiteOwner { get; set; }

        /// <summary>
        /// 关注时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
    }
}
