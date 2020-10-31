using System.Collections.Generic;
using System.Threading.Tasks;
using UP.Basics;
using UP.Basics.Models;
using UP.Interface.Admin.Login;
using UP.Logics.Admin.Login;
using UP.Models.Admin.Login;

namespace UP.Grains.Admin.Login
{
    /// <summary>
    ///
    /// </summary>
    public class LoginGrains : BasicGrains<LoginLogic>, ILogin ,ISysUser
    {
        /// <summary>
        /// 医生登录
        /// </summary>
        /// <returns></returns>
        public Task<JsonMsg<LoginInfoModel>> DoctorLogin(string account_name, string password)
        {
            return Task.FromResult(Logic.DoctorLogin(account_name, password));
        }

        /// <summary>
        /// 获取权限接口
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<List<MenuModel>> Get_DoctorModules(string userId)
        {
            return Task.FromResult(Logic.Get_DoctorModules(userId));
        }

        /// <summary>
        /// 根据账户ID获取账户信息
        /// </summary>
        /// <param name="login_name">登录账号</param>
        /// <returns></returns>
        public Task<UserModel> GetAccountInfo(string login_name)
        {
            //获取医生信息及权限信息
            var model = this.Logic.GetAccountInfo(login_name);
            return Task.FromResult(model);
        }

        /// <summary>
        /// 获取医生基本信息，不包含医生角色及权限
        /// </summary>
        /// <param name="docids">医生id列表</param>
        /// <returns></returns>
        public Task<List<UserModel>> GetUserAccountItems(IEnumerable<int> docids)
        {
            //获取医生基本信息，不包含医生角色及权限
            var items = this.Logic.GetUserAccountItems(docids);
            return Task.FromResult(items);
        }
    }
}