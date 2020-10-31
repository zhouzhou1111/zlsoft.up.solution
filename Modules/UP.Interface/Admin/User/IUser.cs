/****************************************************
* 功能：人员管理接口层
* 描述：
* 作者：胡家源
* 日期：2020/09/11 11:50:01
*********************************************************/
using System.Collections.Generic;
using System.Threading.Tasks;
using UP.Basics;
using UP.Models.DB.RoleRight;
using UP.Models.Admin.RoleRight;
using UP.Models.Admin.User;

namespace UP.Interface.Admin.User
{
   public interface IUser : IBasic
    {
        /// <summary>
        /// 分页查询人员列表
        /// </summary>
        /// <returns></returns>
        Task<ListPageModel<SystemUser>> GetSystemUserList(UserSelect model);
    }
}
