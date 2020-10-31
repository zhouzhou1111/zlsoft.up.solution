using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace UP.Models.DB.Interface
{
    /// <summary>
    /// 外部接口参数
    /// </summary>
    [QWPlatform.Models.Table("intfc_parameters")]
    [DataContract]
    public class InterfaceParam
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
        ///参数名称，如name
        /// </summary>

        [JsonProperty("name")]
        public string name { get; set; }

        /// <summary>
        /// 参数标题
        /// </summary>

        [JsonProperty("title")]
        public string title { get; set; }

        /// <summary>
        /// 参数类型
        /// </summary>
        [JsonProperty("type")]
        public int type { get; set; }

        /// <summary>
        ///  序号
        /// </summary>
        [JsonProperty("sno")]
        public int sno { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [JsonProperty("describe")]
        public string describe { get; set; }
    }
}