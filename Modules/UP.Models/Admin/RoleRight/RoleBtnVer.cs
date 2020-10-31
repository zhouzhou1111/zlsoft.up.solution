using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace UP.Models.Admin.RoleRight
{
    public class RoleBtnVer
    {
        [JsonProperty("id")]
        public int id { get; set; }
        [JsonProperty("name")]
        public string 名称 { get; set; }
        [JsonProperty("easycode")]
        public string  简码 { get; set; }
        [JsonProperty("xsfs")]
        public int 显示方式 { get; set; }

        [JsonProperty("urladdress")]
        public string 路径 { get; set; }
    }
}
