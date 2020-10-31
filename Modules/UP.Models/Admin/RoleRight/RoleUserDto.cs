using Newtonsoft.Json;
using UP.Models.DB.RoleRight;

namespace UP.Models.Admin.RoleRight
{
    public class RoleUserDto:RoleInfo
    {
        [JsonProperty("maxcount")] public string Maxcount { get; set; }
        [JsonProperty("cjrmc")]
        public string 创建人名称 { get; set; }
    }
}
