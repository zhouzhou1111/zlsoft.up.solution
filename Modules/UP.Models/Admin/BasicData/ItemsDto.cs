using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using UP.Models.DB.BasicData;

namespace UP.Models.Admin.BasicData
{
    public class ItemsDto : sys_code_items
    {
        [JsonProperty("pname")]
        public string pname { get; set; }
        [JsonProperty("typename")]
        public string typename { get; set; }
    }
}
