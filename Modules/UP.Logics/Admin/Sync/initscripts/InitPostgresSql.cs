using System;
using System.Collections.Generic;
using System.Text;

namespace UP.Logics.Admin.Sync
{
    /// <summary>
    /// 初始化PG脚本 
    /// </summary>
    public class InitPostgresSql
    {
        #region 行政区划
        public static List<string> 行政区划_初始化表
        {
            get
            {
                var list = new List<string>();
                list.Add("DROP TABLE IF EXISTS \"base\".\"b_行政区划\";");


                return list;
            }
        }
        #endregion
    }
}
