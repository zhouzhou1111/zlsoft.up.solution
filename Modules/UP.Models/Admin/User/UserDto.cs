using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using UP.Models.DB.RoleRight;

namespace UP.Models.Admin.User
{
   public class UserDto:SystemUser
    {
        /// <summary>
        /// 逗号分隔角色名称
        /// </summary>
        [JsonProperty("jsnames")] public string 角色 { get; set; }
    }
}
