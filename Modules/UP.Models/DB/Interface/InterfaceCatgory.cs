using Newtonsoft.Json;
using QWPlatform.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace UP.Models.DB.Interface
{
    /// <summary>
    /// 接口分类、分组映射对象
    /// </summary>
    [Table("intfc_catgory")]
    [DataContract]
    public class InterfaceCatgory
    {
        /// <summary>
        /// id
        /// </summary>
        [JsonProperty("id")]
        public string id { get; set; }

        /// <summary>
        /// 上级id
        /// </summary>
        [JsonProperty("parentId")]
        public string parent_id { get; set; }

        /// <summary>
        /// 分类、分组名称
        /// </summary>
        [JsonProperty("name")]
        public string name { get; set; }

        /// <summary>
        /// 分类编码
        /// </summary>
        [JsonProperty("code")]
        public string code { get; set; }

        /// <summary>
        /// 分类序号
        /// </summary>
        [JsonProperty("sno")]
        public int sno { get; set; }

        /// <summary>
        /// 分类描述
        /// </summary>
        [JsonProperty("describe")]
        public string describe { get; set; }
    }
}