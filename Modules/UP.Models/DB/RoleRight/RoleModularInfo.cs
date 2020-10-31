/*********************************************************
* 功能：角色模块数据库模型
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
    ///系统_角色模块
    /// </summary>
    [Table("系统_角色模块")]
    [DataContract]
    public class RoleModularInfo 
    {
        [PrimaryKey(true)]
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("jsid")]
        public int 角色id { get; set; }

        [JsonProperty("mkid")]
        public int 模块id { get; set; }
    }
}