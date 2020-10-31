/****************************************************
* 功能：系统管理组件层
* 描述：
* 作者：贺伟
* 日期：2020/05/19 19:04:03
*********************************************************/
using QWPlatform.SystemLibrary;
using System.Linq;
using System.Threading.Tasks;
using UP.Basics;
using UP.Interface.DBTable;
using UP.Logics.DBTable;

namespace UP.Grains.DBTable
{
    /// <summary>
    /// 系统操作组件
    /// </summary>
    public class SystemGrains : BasicGrains<SystemLogic>, SystemInterface
    {
        /// <summary>
        /// 初始化加载系统应用信息
        /// </summary>
        /// <returns></returns>
        public Task<bool> InitSystemApplyInfo()
        {
            bool result = false;
            var appItems = this.Logic.SelectAppInfo();
            if (appItems != null && appItems.Any())
            {
                var userinfo_key = "applyitems";
                var obj = CacheManager.Create().Set(userinfo_key, appItems);
                result = true;
            }

            Logger.Instance.Info("初始化系统配置信息:" + result);
            return Task.FromResult(result);
        }
    }
}
