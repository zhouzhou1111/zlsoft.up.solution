/****************************************************
* 功能：系统管理接口层
* 描述：
* 作者：贺伟
* 日期：2020/05/19 19:04:03
*********************************************************/
using System.Threading.Tasks;
using UP.Basics;
namespace UP.Interface.DBTable
{
    /// <summary>
    /// 系统操作接口
    /// </summary>
    public interface SystemInterface : IBasic
    {
        /// <summary>
        /// 初始化加载系统应用信息
        /// </summary>
        /// <returns></returns>
        Task<bool> InitSystemApplyInfo();
    }
}
