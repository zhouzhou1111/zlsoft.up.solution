/*********************************************************
* 功能：数字字典表模型信息
* 描述：
* 作者：贺伟
* 日期：2020/5/14 14:00
**********************************************************/

using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace UP.Models.Api
{
    /// <summary>
    /// 字典信息表
    /// </summary>
    [DataContract]
    public class DicInfo
    {
        /// <summary>
        /// tablename
        /// </summary>
        [JsonProperty("tablename")]
        public string 表名 { get; set; }

        /// <summary>
        /// code
        /// </summary>
        [JsonProperty("code")]
        public string 编码 { get; set; }

        /// <summary>
        /// short_code
        /// </summary>
        [JsonProperty("short_code")]
        public string 简码 { get; set; }

        /// <summary>
        /// name
        /// </summary>
        [JsonProperty("name")]
        public string 名称 { get; set; }
    }
}