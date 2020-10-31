using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UP.Basics.Account
{
    public interface IWxUser : IBasic
    {
        /// <summary>
        /// 根据账户ID获取账户信息
        /// </summary>
        /// <param name="code">账户</param>
        /// <returns>返回账户信息</returns>
        Task<UserModel> GetAccountInfo(string login_name);
    }
}
