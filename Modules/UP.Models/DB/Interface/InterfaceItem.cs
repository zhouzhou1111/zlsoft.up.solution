using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace UP.Models.DB.Interface
{
    /// <summary>
    /// 外部注册接口DB映射类
    /// </summary>
    [QWPlatform.Models.Table("intfc_items")]
    [DataContract]
    public class InterfaceItem
    {
        /// <summary>
        /// GUID
        /// </summary>
        [JsonProperty("id")]
        public string id { get; set; }

        /// <summary>
        /// 分类id
        /// </summary>
        [JsonProperty("catgoryId")]
        public string catgory_id { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [JsonProperty("code")]
        public string code { get; set; }

        /// <summary>
        /// 接口名称
        /// </summary>
        [JsonProperty("name")]
        public string name { get; set; }

        /// <summary>
        /// 描述描述
        /// </summary>
        [JsonProperty("describe")]
        public string describe { get; set; }

        /// <summary>
        /// 接口的访问路径例如：/api/xxx/axxx
        /// </summary>
        [JsonProperty("path")]
        public string path { get; set; }

        /// <summary>
        /// 1:post,2:get,3:put,4:delete
        /// </summary>
        [JsonProperty("accessType")]
        public int access_type { get; set; }

        /// <summary>
        /// 1:异常队列 2: 同步执行
        /// </summary>
        [JsonProperty("procType")]
        public int proc_type { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [JsonProperty("sno")]
        public int sno { get; set; }
    }
}