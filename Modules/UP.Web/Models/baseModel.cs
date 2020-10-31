using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UP.Web.Models
{
    /// <summary>
    /// 所有业务基类
    /// </summary>
    public class BaseModel
    {
        /// </summary>
        public string token { get; set; }
        /// <summary>
        /// 帐户名称
        /// </summary>
        public string account_name { get; set; }
        public string userid { get; set; }
        public string username { get; set; }
        public string userphone { get; set; }
        public string orgid { get; set; }

    }
}
