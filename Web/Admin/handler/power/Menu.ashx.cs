using LinKin.BLLPermission.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace LinKin.Ken.Web.Admin.handler.power
{
    /// <summary>
    /// Menu 的摘要说明   菜单
    /// </summary>
    public class Menu : BaseHandlerNeedLogin
    {
        /// <summary>
        /// 菜单逻辑层
        /// </summary>
        BLLPermission.BLLMenuInfo bllMenuInfo = new BLLPermission.BLLMenuInfo();

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string List(HttpContext context)
        {
            string keyWord=context.Request["keyword"];
            StringBuilder sbWhere = new StringBuilder();
            List<MenuInfo> menuList = bllMenuInfo.GetMenuInfoList(0,false);
            if (menuList.Count == 0)
            {
                resp.msg = "菜单为空.";
                resp.code = (int)BLLJIMP.Enums.APIErrCode.ObjectNotExist;
                return LinKin.Common.JsonHelper.ObjectToJson(resp);
            }
            List<dynamic> returnList = new List<dynamic>();
            List<TemMenuInfo> tempList = new List<TemMenuInfo>();
            foreach (var item in menuList)
            {
                RequestModel requestModel = new RequestModel();
                requestModel.menu_id = item.MenuId;
                requestModel.menu_name = item.MenuName;
                requestModel.menu_url = item.MenuUrl;
                requestModel.pre_id = item.PreId;
                requestModel.show_hide = item.ShowHide;
                requestModel.sort = item.Sort;
                foreach (var child in item.ChildMenuInfo)
                {
                    TemMenuInfo childList = new TemMenuInfo();
                    childList.menu_id = child.MenuId;
                    childList.menu_name = child.MenuName;
                    childList.menu_url = child.MenuUrl;
                    childList.show_hide = child.ShowHide;
                    childList.pre_id = child.PreId;
                    childList.sort = child.Sort;
                    tempList.Add(childList);
                }
                requestModel.child_list = tempList;
                returnList.Add(requestModel);
            }
            resp.msg = "查询完成";
            resp.status = true;
            resp.result = returnList;
            return LinKin.Common.JsonHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Add(HttpContext context)
        {
            string data=context.Request["data"];
            if (string.IsNullOrEmpty(data))
            {
                resp.msg = "必填参数为空.";
                resp.code = (int)BLLJIMP.Enums.APIErrCode.ParameterIsEmpty;
                return LinKin.Common.JsonHelper.ObjectToJson(resp);
            }
            RequestModel requestModel = null;
            try 
	        {	        
		        requestModel=LinKin.Common.JsonHelper.JsonToModel<RequestModel>(data);
	        }
	        catch (Exception)
	        {
		        resp.code=(int)BLLJIMP.Enums.APIErrCode.Exception;
                resp.msg="json数据格式错误.";
		        return LinKin.Common.JsonHelper.ObjectToJson(resp);
	        }
            if (requestModel.menu_id>0)
            {
                resp.code = (int)BLLJIMP.Enums.APIErrCode.ObjectNotExist;
                resp.msg = "菜单id不可传入.";
                return LinKin.Common.JsonHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(requestModel.menu_name))
            {
                resp.msg = "菜单名为空.";
                resp.code=(int)BLLJIMP.Enums.APIErrCode.ParameterIsEmpty;
                return LinKin.Common.JsonHelper.ObjectToJson(resp);
            }

            MenuInfo model = new MenuInfo();
            model.MenuId =int.Parse(bllMenuInfo.GetGUID(LinKin.BLLJIMP.Enums.TransacType.MenuID));
            model.MenuName = requestModel.menu_name;
            model.MenuUrl = requestModel.menu_url;
            model.PreId = requestModel.pre_id;
            model.ShowHide = requestModel.show_hide;
            model.Sort = requestModel.sort;
            model.Creater = bllMenuInfo.GetCurrentUserId();
            model.CreateDate = DateTime.Now;
            if (bllMenuInfo.Add(model))
            {
                resp.status = true;
                resp.msg = "添加完成.";
            }
            else
            {
                resp.code=(int)BLLJIMP.Enums.APIErrCode.Operation;
                resp.msg = "操作出错.";
            }
            return LinKin.Common.JsonHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Update(HttpContext context)
        {
            string data=context.Request["data"];
            if (string.IsNullOrEmpty(data))
            {
                resp.msg = "必填参数为空.";
                resp.code = (int)BLLJIMP.Enums.APIErrCode.ParameterIsEmpty;
                return LinKin.Common.JsonHelper.ObjectToJson(resp);
            }
            RequestModel requestModel = null;
            try
            {
                requestModel = LinKin.Common.JsonHelper.JsonToModel<RequestModel>(data);
            }
            catch (Exception)
            {
                resp.code = (int)BLLJIMP.Enums.APIErrCode.Exception;
                resp.msg = "json数据格式错误.";
                return LinKin.Common.JsonHelper.ObjectToJson(resp);
            }
            if (requestModel.menu_id <= 0)
            {
                resp.code = (int)BLLJIMP.Enums.APIErrCode.ParameterIsEmpty;
                resp.msg = "请传入正确的id.";
                return LinKin.Common.JsonHelper.ObjectToJson(resp);
            }
            MenuInfo model = bllMenuInfo.GetMenuInfo(requestModel.menu_id);
            if (model==null)
            {
                resp.msg = "不存在菜单信息.";
                resp.code = (int)BLLJIMP.Enums.APIErrCode.ObjectNotExist;
                return LinKin.Common.JsonHelper.ObjectToJson(resp);
            }
            if (!string.IsNullOrEmpty(requestModel.menu_id.ToString()))
            {
                resp.code = (int)BLLJIMP.Enums.APIErrCode.ObjectNotExist;
                resp.msg = "菜单id不可传入.";
                return LinKin.Common.JsonHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(requestModel.menu_name))
            {
                resp.msg = "菜单名为空.";
                resp.code = (int)BLLJIMP.Enums.APIErrCode.ParameterIsEmpty;
                return LinKin.Common.JsonHelper.ObjectToJson(resp);
            }
            if (bllMenuInfo.Update(model))
            {
                resp.msg = "编辑完成.";
                resp.status = true;
            }
            else
            {
                resp.code = (int)BLLJIMP.Enums.APIErrCode.Operation;
                resp.msg = "编辑出错.";
            }
            return LinKin.Common.JsonHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Delete(HttpContext context)
        {
            string ids=context.Request["ids"];
            if (string.IsNullOrEmpty(ids))
            {
                resp.msg = "菜单id为空.";
                resp.code = (int)BLLJIMP.Enums.APIErrCode.ParameterIsEmpty;
                return LinKin.Common.JsonHelper.ObjectToJson(resp);
            }

            return LinKin.Common.JsonHelper.ObjectToJson(resp);
        }



        public class RequestModel
        {
            public int menu_id { get; set; }
            /// <summary>
            /// 菜单名
            /// </summary>
            public string menu_name { get; set; }

            /// <summary>
            /// 菜单路径
            /// </summary>
            public string menu_url { get; set; }

            /// <summary>
            /// 上级id
            /// </summary>
            public int pre_id { get; set; }

            /// <summary>
            /// 排序
            /// </summary>
            public int sort { get; set; }

            /// <summary>
            /// 显示隐藏   [0显示  1隐藏]
            /// </summary>
            public int show_hide { get; set; }

            /// <summary>
            /// 下级菜单
            /// </summary>
            public List<TemMenuInfo> child_list { get; set; }
        }
        /// <summary>
        /// 下级菜单
        /// </summary>
        public class TemMenuInfo
        {
            public int menu_id { get; set; }
            /// <summary>
            /// 菜单名
            /// </summary>
            public string menu_name { get; set; }

            /// <summary>
            /// 菜单路径
            /// </summary>
            public string menu_url { get; set; }

            /// <summary>
            /// 上级id
            /// </summary>
            public int pre_id { get; set; }

            /// <summary>
            /// 排序
            /// </summary>
            public int sort { get; set; }

            /// <summary>
            /// 显示隐藏   [0显示  1隐藏]
            /// </summary>
            public int show_hide { get; set; }
        }


    }
}