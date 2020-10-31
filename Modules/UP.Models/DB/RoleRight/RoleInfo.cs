/*********************************************************
* 功能：角色数据库模型
* 描述：
* 作者：贺伟
* 日期：2020/5/13 17:00
**********************************************************/

using Newtonsoft.Json;
using QWPlatform.Models;
using System;
using System.Runtime.Serialization;

namespace UP.Models.DB.RoleRight
{
    /// <summary>
    ///系统_角色
    /// </summary>
    [Table("系统_角色")]
    [DataContract]
    public class RoleInfo
    {
        [DataMember]
        public string id { get; set; }

        [JsonProperty("mc")]
        public string 名称 { get; set; }

        [JsonProperty("bz")]
        public string 描述 { get; set; }

        [JsonProperty("gzdw")]
        public string 工作单位 { get; set; }
        /// <summary>
        /// 0启用,1停用
        /// </summary>
        [JsonProperty("sfty")]
        public int 是否停用 { get; set; }

        [JsonProperty("isval")]
        public int 数据标识 { get; set; }

        [JsonProperty("cjr")]
        public string 创建人 { get; set; }

        [JsonProperty("cjsj")]
        public DateTime 创建时间 { get; set; }
    }
}