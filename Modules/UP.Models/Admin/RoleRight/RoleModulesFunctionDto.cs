using Newtonsoft.Json;
using UP.Models.DB.RoleRight;

namespace UP.Models.Admin.RoleRight
{
   public class RoleModulesFunctionDto: ModulesFunction
    {
        [JsonProperty("jsid")]
        public int 角色id { get; set; }
        [JsonProperty("gnid")]
        public int 功能id { get; set; }
    }
}
