using Newtonsoft.Json;
using UP.Models.DB.RoleRight;

namespace UP.Models.Admin.RoleRight
{
	public class ModulesRoleAuth
	{
		[JsonProperty("jsid")]
		public int 角色模块id { get; set; }
		[JsonProperty("mkid")]
		public int 模块id { get; set; }
		[JsonProperty("mksjid")]
		public int 模块上级id { get; set; }
		[JsonProperty("mc")]
		public string 名称 { get; set; }
		[JsonProperty("lj")]
		public string 路径 { get; set; }
		[JsonProperty("sx")]
		public short 顺序 { get; set; }
		[JsonProperty("sq")]
		public string 授权 { get; set; }

	}

}
