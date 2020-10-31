using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using UP.Models.DB.BasicData;

namespace UP.Models.Admin.Org
{
   public class OrganDto:Organ
    {
        [JsonProperty("pname")]
        public string 上级机构名称 { get; set; }

        [JsonProperty("province")]
        public string 所属省 { get; set; }
        [JsonProperty("city")]
        public string 所属市 { get; set; }
        [JsonProperty("district")]
        public string 所属区县 { get; set; }
        [JsonProperty("township")]
        public string 所属街道 { get; set; }
        [JsonProperty("neighborhood")]
        public string 所属居委会 { get; set; }

        ///// <summary>
        ///// 上级id
        ///// </summary>
        //[JsonProperty("pid")]
        //public int pid { get; set; }

        /// <summary>
        /// 下级数量
        /// </summary>
        [JsonProperty("count")]
        public int 数量 { get; set; }
        /// <summary>
        /// haveChild
        /// </summary>
        [JsonProperty("haveChild")]
        public bool haveChild { get; set; }
    }
}
