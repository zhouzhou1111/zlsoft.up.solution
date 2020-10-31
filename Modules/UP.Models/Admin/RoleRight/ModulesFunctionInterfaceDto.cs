using Newtonsoft.Json;
using UP.Models.DB.RoleRight;

namespace UP.Models.Admin.RoleRight
{
   public class ModulesFunctionInterfaceDto: ModulesFunctionInterface
    {
        [JsonProperty("sq")]
        public int 授权 { get; set; }
    }
}
