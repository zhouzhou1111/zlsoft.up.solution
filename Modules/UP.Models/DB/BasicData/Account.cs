using Newtonsoft.Json;
using QWPlatform.Models;
using System;
using System.Runtime.Serialization;

namespace UP.Models.DB.BasicData
{/// <summary>
 ///b_账户
 /// </summary>
    [Table("b_账户")]
    [DataContract]
    public class Account
    {

        /// <summary>
        /// id:
        /// </summary>
        [PrimaryKey(true)]
        [JsonProperty("id")]
        public int id { get; set; }



        /// <summary>
        /// 人员id:关联人员id
        /// </summary>
       	[JsonProperty("orgpersonid")]
        public int 人员id { get; set; }

        /// <summary>
        /// 姓名:冗余存储人员表的姓名
        /// </summary>
       	[JsonProperty("name")]
        public string 姓名 { get; set; }

        /// <summary>
        /// 账户:登录的账户名
        /// </summary>
       	[JsonProperty("account")]
        public string 账户 { get; set; }

        /// <summary>
        /// 密码:加密存储登录密码，MD5加密
        /// </summary>
        [JsonProperty("password")]
        public string 密码 { get; set; }

        /// <summary>
        /// 状态:数据状态:-1-删除;0-停用;1-正常
        /// </summary>
       	[JsonProperty("state")]
        public decimal 状态 { get; set; }
    }
}
