namespace UP.Web.Models.Business.Auth
{
    /// <summary>
    /// 身份认证参数信息
    /// </summary>
    public class AuthPara
    {
        /// <summary>
        /// 登录账号
        /// </summary>
        public string account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }
    }
}