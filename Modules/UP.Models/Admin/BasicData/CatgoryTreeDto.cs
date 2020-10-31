using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using UP.Models.DB.BasicData;

namespace UP.Models.Admin.BasicData
{
    public class CatgoryTreeDto : sys_code_catgory
    {
        [JsonProperty("items")]
        public List<CatgoryTreeDto> Items { get; set; }
        [JsonProperty("text")]
        public string text { get; set; }
    }
}
