using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace UP.Models.Admin.BasicData
{
    public class BasicDataDto
    {
        /// <summary>
        /// 编码:
        /// </summary>
        [JsonProperty("code")]
        public string 编码 { get; set; }

        /// <summary>
        /// 名称:
        /// </summary>
        [JsonProperty("name")]
        public string 名称 { get; set; }


        /// <summary>
        /// 简码:
        /// </summary>
        [JsonProperty("scode")]
        public string 简码 { get; set; }
        /// <summary>
        /// 旧编码
        /// </summary>
        [JsonProperty("oldcode")]
        public string oldcode { get; set; }

        /// <summary>
        /// 总条数
        /// </summary>
        [JsonProperty("total")]
        public int total { get; set; }

        /// <summary>
        /// 基础数据枚举表类型
        /// </summary>
        [JsonProperty("tabletype")]
        public int tabletype { get; set; }

        /// <summary>
        /// 药物:
        /// </summary>
        [JsonProperty("medicine")]
        public int 药物 { get; set; }


        /// <summary>
        /// 层次:
        /// </summary>
        [JsonProperty("arrangement")]
        public int  层次 { get; set; }

        /// <summary>
        /// 层次:
        /// </summary> 
        public int typeData { get; set; }
    }
}
