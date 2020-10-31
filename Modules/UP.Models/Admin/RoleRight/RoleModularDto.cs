using Newtonsoft.Json;
using UP.Models.DB.RoleRight;

namespace UP.Models.Admin.RoleRight
{
    public class RoleModularDto: RoleUserDto
    {
         
        [JsonProperty("modular_count")]
        public int 模块权限数量 { get; set; }
    }
}
