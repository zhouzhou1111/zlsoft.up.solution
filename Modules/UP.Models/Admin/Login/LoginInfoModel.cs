using Newtonsoft.Json;
using System.Runtime.Serialization;
using UP.Models.DB.RoleRight;

namespace UP.Models.Admin.Login
{
    /// <summary>
    /// 获取身份信息对象
    /// </summary>
	[DataContract]
    public class LoginInfoModel : SystemUser
    {
        /// <summary>
        /// account:登录账户
        /// </summary>
        [DataMember]
        public string account { get; set; }

        /// <summary>
        /// account_state:账户状态 -1-删除;0-停用;1-正常
        /// </summary>
        public int account_status { get; set; }
        /// <summary>
        /// id：人员id
        /// </summary>
        [DataMember]
        public string id { get; set; }

        /// <summary>
        /// name：人员姓名
        /// </summary>
        [DataMember]
        public string name { get; set; }

        /// <summary>
        /// status：人员状态 -1-删除;0-停用;1-正常
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// org_id：机构id
        /// </summary>
        [DataMember]
        public string org_id { get; set; }

        /// <summary>
        /// org_name：机构名称
        /// </summary>
        [DataMember]
        public string org_name { get; set; }
        /// <summary>
        /// token：身份令牌
        /// </summary>
        [DataMember]
        public string token { get; set; }

        /// <summary>
        /// token：身份令牌有效期(当前服务器时间戳毫秒数)
        /// </summary>
        [DataMember]
        public long token_effective_period { get; set; }

        /// <summary>
        /// jsid：角色id
        /// </summary>
        [JsonProperty("role_id")]
        public int 角色id { get; set; }

        /// <summary>
        /// jsmc：角色名称
        /// </summary>
        [JsonProperty("role_name")]
        public string 角色名称 { get; set; }

        /// <summary>
        /// is_super_admin：1-超级管理员
        /// </summary>
        [JsonProperty("is_super_admin")]
        public int is_super_admin { get; set; }
    }
}