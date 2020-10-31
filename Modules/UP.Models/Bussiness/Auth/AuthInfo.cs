using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace UP.Models.Business.Auth
{
    /// <summary>
    /// 获取身份信息对象
    /// </summary>
	[DataContract]
    public class AuthInfo
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
    }
}