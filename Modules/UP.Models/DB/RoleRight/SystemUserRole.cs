/*********************************************************
* 功能：模块配置数据库模型
* 描述：
* 作者：贺伟
* 日期：2020/5/13 17:00
**********************************************************/

using Newtonsoft.Json;
using QWPlatform.Models;
using System.Runtime.Serialization;

namespace UP.Models.DB.RoleRight
{
    /// <summary>
    ///系统_人员角色
    /// </summary>
    [Table("系统_人员角色")]
    [DataContract]
    public class SystemUserRole 
    {
        [JsonProperty("id")]
        public string id { get; set; }
        /// <summary>
        /// 账户id
        /// </summary>
        [JsonProperty("account_id")]
        public string account_id { get; set; }
        /// <summary>
        /// 角色id
        /// </summary>
        [JsonProperty("role_id")]
        public string role_id { get; set; }
    }
}