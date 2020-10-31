using System;

namespace UP.Basics
{
    /// <summary>
    /// 登录的model
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// 当前登录账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 登录授权token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
