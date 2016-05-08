using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinKin.Common
{
    /// <summary>
    ///处理字符串
    /// </summary>
    public class StringHelper
    {
        /// <summary>
        /// 字符串反转
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReverseStr(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";

            char[] array = str.Trim().ToCharArray();
            Array.Reverse(array);
            StringBuilder sbWhere = new StringBuilder();
            foreach (char item in array)
            {
                sbWhere.Append(item.ToString());
            }
            return sbWhere.ToString();
        }


    }
}
