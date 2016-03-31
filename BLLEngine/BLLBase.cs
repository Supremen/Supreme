using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LinKin.BLLEngine
{
    [Serializable]
    abstract public class BLLBase
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public bool Exists(ModelTable model,string col)
        {
            List<string> columns = new List<string>();
            columns.Add(col);
            return Exists(model, columns);
        }
        public bool Exists(ModelTable model, List<string> cols)
        {
            Type t = model.GetType();
            Dictionary<string, string> columns = new Dictionary<string, string>();
            PropertyInfo[] properties = t.GetProperties();
            //缓存用的语句
            string where = "";
            foreach (string col in cols)
            {
                foreach (var p in properties)
                {
                    if (p.Name == col)
                    {
                        string value = p.GetValue(model, null).ToString();
                        columns.Add(col, value);
                        where += col + ":" + value + "_";
                        break;
                    }
                }
            }
            return false;
        }



    }
}
