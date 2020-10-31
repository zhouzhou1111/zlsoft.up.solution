using System.Collections.Generic;

namespace UP.WebRoot
{
    /// <summary>
    /// 配置的JWTmodel
    /// </summary>
    public class JwtConfigModel
    {
        /// <summary>
        /// 发行者
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 拥护者
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 加密key
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// 生命周期
        /// </summary>
        public int Lifetime { get; set; }

        /// <summary>
        /// 是否验证生命周期
        /// </summary>
        public bool ValidateLifetime { get; set; }

        /// <summary>
        /// 登录失败的尝试次数配置
        /// </summary>
        public int TryLoginCount { get; set; }

        /// <summary>
        /// 忽略验证的url
        /// </summary>
        public List<string> IgnoreUrls { get; set; }
    }
}
