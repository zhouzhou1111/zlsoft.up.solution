using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace UP.Models.DB.Interface
{
    /// <summary>
    /// 接口异步方法DB实体类
    /// </summary>
    [QWPlatform.Models.Table("intfc_async")]
    [DataContract]
    public class InterfaceAsync
    {
        /// <summary>
        /// guid
        /// </summary>
        [JsonProperty("id")]
        public string id { get; set; }

        /// <summary>
        /// 接口id
        /// </summary>
        [JsonProperty("itemsId")]
        public string items_id { get; set; }

        /// <summary>
        /// MQ exchage 下拉选择一个路由器
        /// </summary>
        [JsonProperty("exchageId")]
        public string exchage_id { get; set; }

        /// <summary>
        /// 选择一个关键字
        /// </summary>
        [JsonProperty("keyword")]
        public string keyword { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [JsonProperty("describe")]
        public string describe { get; set; }
    }
}