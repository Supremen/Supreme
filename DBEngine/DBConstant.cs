using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
namespace LinKin.DALEngine
{
    public class DBConstant
    {
        /// <summary>
        ///获取连接字符串
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
                string conStringEncrypt = ConfigurationManager.AppSettings["ConStringEncrypt"];
                if (conStringEncrypt == "true")
                {
                    connectionString = "";
                }
                return connectionString;
            }
        }

        /// <summary>
        /// 得到web.config里配置项的数据库连接字符串。
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        public static string GetConnectionString(string configName)
        {
            string connectionString = ConfigurationManager.AppSettings[configName];
            string conStringEncrypt = ConfigurationManager.AppSettings["ConStringEncrypt"];
            if (conStringEncrypt == "true")
            {
                connectionString = "";
            }
            return connectionString;
        }


    }
}
