using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace UP.Models.DB.Interface
{
    /// <summary>
    /// 外部接口授权DB实体类
    /// </summary>
    [QWPlatform.Models.Table("intfc_auth")]
    [DataContract]
    public class InterfaceAuth
    {
        /// <summary>
        /// GUID
        /// </summary>
        [JsonProperty("id")]
        public string id { get; set; }

        /// <summary>
        /// 接口ID
        /// </summary>
        [JsonProperty("itemsId")]
        public string items_id { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        [JsonProperty("productId")]
        public string product_id { get; set; }

        /// <summary>
        /// 每分钟调用次数
        /// </summary>
        [JsonProperty("frequencyInterval")]
        public int frequency_interval { get; set; }

        /// <summary>
        ///可调用时间范围
        /// </summary>
        [JsonProperty("efctime")]
        public DateTime efctime { get; set; }

        /// <summary>
        ///失效的时间
        /// </summary>
        [JsonProperty("expitdate")]
        public DateTime expitdate { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [JsonProperty("describe")]
        public string describe { get; set; }
    }
}