/*********************************************************
* 功能：角色功能数据库模型
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
    ///系统_角色功能
    /// </summary>
    [Table("系统_角色功能")]
    [DataContract]
    public class RoleFunction
    {
        [PrimaryKey(true)]
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("jsid")]
        public int 角色id { get; set; }

        [JsonProperty("jsmkid")]
        public int 模块id { get; set; }

        [JsonProperty("gnid")]
        public int 功能id { get; set; }
    }
}