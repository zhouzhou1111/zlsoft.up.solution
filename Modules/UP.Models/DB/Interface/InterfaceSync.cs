using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace UP.Models.DB.Interface
{
    /// <summary>
    /// 接口同步方法DB实体类
    /// </summary>
    [QWPlatform.Models.Table("intfc_sync")]
    [DataContract]
    public class InterfaceSync
    {
        /// <summary>
        /// GUID
        /// </summary>
        [JsonProperty("id")]
        public string id { get; set; }

        /// <summary>
        /// 指向的接口id
        /// </summary>
        [JsonProperty("itemsId")]
        public string items_id { get; set; }

        /// <summary>
        /// 排序号，也是执行顺序号
        /// </summary>
        [JsonProperty("sno")]
        public string sno { get; set; }

        /// <summary>
        /// 引用的服务id
        /// </summary>
        [JsonProperty("serviceId")]
        public string service_id { get; set; }

        /// <summary>
        /// 消息转发调用地址或方法
        /// </summary>
        [JsonProperty("callAddress")]
        public string call_address { get; set; }

        /// <summary>
        /// 1:get,2:post
        /// </summary>
        [JsonProperty("callType")]
        public int call_type { get; set; }

        /// <summary>
        /// 1:Basic,2:token（通过业务系统进行授权）
        /// </summary>
        [JsonProperty("authType")]
        public int auth_type { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("userName")]
        public string user_name { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("password")]
        public string password { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("describe")]
        public string describe { get; set; }
    }
}