using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using UP.Models.DB.BasicData;

namespace UP.Models.Admin.Org
{
    public class OrgPersonDto : OrgPerson
    {
        [JsonProperty("orgname")]
        public string 机构名称 { get; set; }
        [JsonProperty("account")]
        public string  账户 { get; set; }
        [JsonProperty("password")]
        public string  密码 { get; set; }
        [JsonProperty("accountid")]
        public int 账户id { get; set; }
    }
}
