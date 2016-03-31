using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinKin.BLLJIMP.Model
{
    /// <summary>
    /// 网站信息
    /// </summary>
    [Serializable]
    public class WebsiteInfo:BLLEngine.ModelTable
    {
        public WebsiteInfo() 
        {
        
        }

        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 网站名称
        /// </summary>
        public string WebsiteName { get; set; }
        /// <summary>
        /// 网站描述
        /// </summary>
        public string WebsiteDescription { get; set; }
        /// <summary>
        /// 网站logo
        /// </summary>
        public string WebsiteLogo { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string Creater { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}
