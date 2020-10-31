using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UP.Basics
{
    /// <summary>
    /// 系统帐安全管理接口
    /// </summary>
    public interface ISysUser : IBasic
    {
        /// <summary>
        /// 根据账户ID获取账户信息
        /// </summary>
        /// <param name="code">账户</param>
        /// <returns>返回账户信息</returns>
        Task<UserModel> GetAccountInfo(string login_name);

        /// <summary>
        /// 获取医生基本信息，不包含医生角色及权限
        /// </summary>
        /// <param name="docids">医生id列表</param>
        /// <returns></returns>
        Task<List<UserModel>> GetUserAccountItems(IEnumerable<int> docids);
    }
}
