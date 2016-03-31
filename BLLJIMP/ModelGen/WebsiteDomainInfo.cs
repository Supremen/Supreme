using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinKin.BLLJIMP.Model
{
    /// <summary>
    /// 网站域名
    /// </summary>
    [Serializable]
    public class WebsiteDomainInfo : BLLEngine.ModelTable
    {
        public WebsiteDomainInfo() 
        {

        }

        /// <summary>
        /// 域名
        /// </summary>
        public string WebsiteDomain { get; set; }

        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }
    }
}
