using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using UP.Models.DB.BusinessSys;

namespace UP.Models.Admin.BusinessSys
{
   public class ProductSourceDto : ProductSource
    {
        [JsonProperty("productname")]
        public string 产品名称 { get; set; }
        [JsonProperty("state")]
        public string 状态 { get; set; }
    }
}
