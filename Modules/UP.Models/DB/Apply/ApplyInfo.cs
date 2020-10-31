/*********************************************************
* 功能：系统应用信息数据库 模型
* 描述：
* 作者：贺伟
* 日期：2020/5/13 17:00
**********************************************************/

using Newtonsoft.Json;
using QWPlatform.Models;
using System;
using System.Runtime.Serialization;

namespace UP.Models.Apply
{
    /// <summary>
    ///系统_应用信息
    /// </summary>
    [Table("系统_应用信息")]
    [DataContract]
    public class AppLyInfo
    {
        /// <summary>
        /// APPID:
        /// </summary>
        [JsonProperty("app_id")]
        public string APPID { get; set; }

        /// <summary>
        /// 应用名称:
        /// </summary>
        [JsonProperty("app_name")]
        public string 应用名称 { get; set; }

        /// <summary>
        /// 公钥:调用加密密钥
        /// </summary>
        [JsonProperty("public_key")]
        public string 公钥 { get; set; }

        /// <summary>
        /// 私钥:平台解密密钥
        /// </summary>
        [JsonProperty("private_key")]
        public string 私钥 { get; set; }

        /// <summary>
        ///  0停用;1 正常
        /// </summary>
        [JsonProperty("state")]
        public int 应用状态 { get; set; }

        /// <summary>
        /// 使用平台 PC=1; h5=2;安卓=3;IOS=4
        /// </summary>
        [JsonProperty("sourse")]
        public int 使用平台 { get; set; }

        /// <summary>
        /// 登记时间:
        /// </summary>
        [JsonProperty("addtime")]
        public DateTime 登记时间 { get; set; }

        /// <summary>
        /// 登记人:
        /// </summary>
        [JsonProperty("adduser_id")]
        public int 登记人id { get; set; }

        /// <summary>
        /// 变更时间:
        /// </summary>
        [JsonProperty("updatetime")]
        public DateTime 变更时间 { get; set; }

        /// <summary>
        /// 变更人:
        /// </summary>
        [JsonProperty("updateuser_id")]
        public int 变更人id { get; set; }

        /// <summary>
        /// 数据标识:  0 删除（停用）1 正常
        /// </summary>
        public int 数据标识 { get; set; }
    }
}