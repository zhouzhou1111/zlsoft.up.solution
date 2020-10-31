using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UP.Web.Models.Admin.Login
{
    public class UpdatePasswordParam
    {
        /// <summary>
        /// 人员id
        /// </summary>
        public int userid { get; set; }
        /// <summary>
        /// 旧密码
        /// </summary>
        public string old_password { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        public string new_password { get; set; }
    }
}
