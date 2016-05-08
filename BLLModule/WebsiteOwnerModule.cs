using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace LinKin.BLLModule
{
    public class WebsiteOwnerModule:IHttpModule
    {
        /// <summary>
        /// BLL层
        /// </summary>
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public void Dispose()
        {

        }

        public void Init(HttpApplication context)
        {
            context.AcquireRequestState += new EventHandler(context_AcquireRequestState);
        }

        void context_AcquireRequestState(object sender,EventArgs e)
        {
            /*
                * 获取当前域名
                * 
                * 判断是否是已经存在站点配置，如果存在，判断域名相同则不再处理，不相同则进行重新获取配置处理
                * 不存在站点配置则立即进行获取配置处理
                * 
                * 根据域名取得站点配置
                * 如果取得站点配置着保存当前配置
                * 否则跳到域名未注册页面
             
                * 站点配置信息 WebsiteInfoModel , 站点配置域名 WebsiteDomain
             */
            //try
            //{
            //    HttpApplication application = sender as HttpApplication;
            //    string webSite = application.Request.Url.Host.ToLower();
            //    LinKin.BLLJIMP.Model.WebsiteDomainInfo DomainModel = bll.Get<LinKin.BLLJIMP.Model.WebsiteDomainInfo>(
            //        string.Format(" WebsiteDomain='{0}'",webSite));
            //    if (DomainModel==null)
            //    {
            //        return;
            //    }
            //    else
            //    {
            //        LinKin.BLLJIMP.Model.WebsiteInfo webSiteModel = bll.Get<LinKin.BLLJIMP.Model.WebsiteInfo>(
            //            string.Format(" WebsiteOwner='{0}'", DomainModel.WebsiteOwner));
            //        application.Session["WebsiteOwner"] = webSiteModel.WebsiteOwner;
            //        application.Session["WebsiteOwnerInfo"] = webSiteModel;
            //    }

            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }


    }
}
