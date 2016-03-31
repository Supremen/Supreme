using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace LinKin.Common
{
    public class JsonHelper
    {
        /// <summary>
        /// 将对象转换成JSON字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ObjectToJson(object obj)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(obj);
        }
    }
}
