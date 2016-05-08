using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinKin.BLLPermission.Model
{
    public partial class MenuInfo
    {
        public MenuInfo()
        {
            ChildMenuInfo = new List<MenuInfo>();
        }
        /// <summary>
        /// 下级菜单
        /// </summary>
        public List<MenuInfo> ChildMenuInfo { get; set; }
    }
}
