using System.Runtime.Serialization;

namespace UP.Basics
{
    /// <summary>
    /// 用户请求来源应用信息
    /// </summary>
    [DataContract]
    public class UserAppLyInfo
    {
        /// <summary>
        /// 应用id
        /// <summary>
        [DataMember]
        public string app_id { get; set; }

        /// <summary>
        /// 应用名称:
        /// <summary>
        [DataMember]
        public string app_name { get; set; }

        /// <summary>
        /// 使用平台 PC=1; h5=2;安卓=3;IOS=4
        /// </summary>
        [DataMember]
        public int sourse { get; set; }

        /// <summary>
        /// 公钥:调用加密密钥
        /// <summary>
        [DataMember]
        public string public_key { get; set; }

        /// <summary>
        /// 私钥:平台解密密钥
        /// <summary>
        [DataMember]
        public string private_key { get; set; }
    }
}