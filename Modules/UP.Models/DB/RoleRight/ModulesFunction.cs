/*********************************************************
* 功能：模块功能数据库模型
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
    ///模块功能
    /// </summary>
    [Table("系统_功能")]
    [DataContract]
    public class ModulesFunction
    {
        [DataMember]
        public string id { get; set; }

        [JsonProperty("mkid")]
        public int 模块id { get; set; }

        [JsonProperty("mc")]
        public string 名称 { get; set; }

        [JsonProperty("ms")]
        public string 描述 { get; set; }

        [JsonProperty("sx")]
        public int 顺序 { get; set; }

        [JsonProperty("jm")]
        public string 简码 { get; set; }

        [JsonProperty("tp")]
        public string 图片 { get; set; }

        [JsonProperty("sfty")]
        public int 是否停用 { get; set; }

        [JsonProperty("isval")]
        public int 数据标识 { get; set; }
        [JsonProperty("tyr")]
        public string 停用人 { get; set; }

        [JsonProperty("tysj")]
        public DateTime? 停用时间 { get; set; }
        [JsonProperty("xsfs")]
        public int  显示方式 { get; set; }
    }
}