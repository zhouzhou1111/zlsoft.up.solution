using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using UP.Models.DB.BasicData;

namespace UP.Models.Admin.BasicData
{
   public class CatgoryDto:sys_code_catgory
    {
        [JsonProperty("pname")]
        public string pname { get; set; }
    }
}
