using LinKin.BLLPermission.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinKin.BLLPermission
{
    public class BLLMenuInfo:BLLJIMP.BLL
    {
        /// <summary>
        /// 获取菜单信息
        /// </summary>
        /// <returns></returns>
        public MenuInfo GetMenuInfo(int menuId)
        {
            return Get<MenuInfo>(string.Format(" MenuId={0} ",menuId));
        }

        public List<MenuInfo> GetMenus(int preId,bool showHide)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(" PreId={0} ",preId);
            if (!showHide) strSql.AppendFormat(" AND ShowHide= 0 ");
            strSql.AppendFormat(" Order by Sort ");
            List<MenuInfo> menus = GetList<MenuInfo>(strSql.ToString());
            return menus;

        }
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="preId"></param>
        /// <param name="showHide"></param>
        /// <returns></returns>
        public List<MenuInfo> GetMenuInfoList(int preId, bool showHide)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(" PreId={0} ",preId);
            if (!showHide) strSql.AppendFormat(" AND ShowHide= 0 ");
            strSql.AppendFormat(" Order by Sort ");
            List<MenuInfo> menus = GetList<MenuInfo>(strSql.ToString());
            if (menus.Count > 0)
            {
                for (int i = 0; i < menus.Count; i++)
                {
                    List<MenuInfo> childList = GetList<MenuInfo>(string.Format(" PreId={0} ",menus[i].MenuId));
                    if (childList.Count > 0)
                    {
                        menus[i].ChildMenuInfo.AddRange(childList);
                    }
                }
            }
            return menus;
        }


       
        
    }
}
