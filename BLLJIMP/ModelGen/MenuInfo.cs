using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinKin.BLLJIMP.Model
{
    /// <summary>
    /// 菜单信息
    /// </summary>
    [Serializable]
    public class MenuInfo:BLLEngine.ModelTable
    {
        public MenuInfo() 
        {
        
        }

        /// <summary>
        /// 菜单ID
        /// </summary>
        public int MenuId { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; }
        /// <summary>
        /// 跳转路径
        /// </summary>
        public string MenuUrl { get; set; }
        /// <summary>
        /// 上级id
        /// </summary>
        public int PreId { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 是否隐藏
        /// </summary>
        public int IsHide { get; set; }
        /// <summary>
        /// 所属站点
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        public string Creater { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }


    }
}
