using System;
using System.Collections.Generic;
using System.Text;

namespace UP.Basics
{
    /// <summary>
    /// JWT接口
    /// </summary>
    public interface IJwt
    {
        /// <summary>
        /// 获取到一个新的token
        /// </summary>
        /// <param name="Clims"></param>
        /// <returns></returns>
        string GetToken(Dictionary<string, string> Clims);

        /// <summary>
        /// 验证Token是否有效
        /// </summary>
        /// <param name="token">验证的token</param>
        /// <param name="Clims">输出对象</param>
        /// <returns></returns>
        bool ValidateToken(string token, out Dictionary<string, string> Clims);

        /// <summary>
        /// 根据登录的token获取当前用户信息
        /// </summary> 
        string LoginAccount { get; set; }

        /// <summary>
        /// 登录失败尝试的次数
        /// </summary>
        int TryCount { get; }

        /// <summary>
        /// 允许登录的url列表，注意大小写
        /// </summary>
        List<string> IgnoreUrls { get; }
    }
}
