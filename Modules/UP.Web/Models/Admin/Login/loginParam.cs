namespace UP.Web.Models.Admin.Login
{
    /// <summary>
    /// 登录入参
    /// </summary>
    public class LoginPara
    {
        /// <summary>
        /// 手机号
        /// </summary>
        public string account_num { get; set; }

        /// <summary>
        /// 用户来源
        /// </summary>
        public string apptype { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string imagecode { get; set; }

        public string imgkey { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }
    }
}