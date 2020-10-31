using Newtonsoft.Json;
using UP.Models.DB.RoleRight;

namespace UP.Models.Admin.RoleRight
{
	public class ModulesFunctionDto: ModulesFunction
	{
		[JsonProperty("sq")]
		public int 授权 { get; set; }
	}

}
