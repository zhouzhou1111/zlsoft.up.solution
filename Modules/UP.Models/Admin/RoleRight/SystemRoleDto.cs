using Newtonsoft.Json;
using UP.Models.DB.RoleRight;

namespace UP.Models.Admin.RoleRight
{
   public class SystemRoleDto:SystemUser
    {
        [JsonProperty("jsid")]
        public int 角色id { get; set; }
        [JsonProperty("jsmc")]
        public string 名称 { get; set; }
        [JsonProperty("ryid")]
        public int 人员id { get; set; }
    }
}
