/****************************************************
* 功能：人员管理业务层
* 描述：
* 作者：胡家源
* 日期：2020/09/11 11:52:01
*********************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UP.Basics;
using UP.Interface.Admin.User;
using UP.Logics.Admin.User;
using UP.Models.Admin.User;
using UP.Models.DB.RoleRight;

namespace UP.Grains.Admin.User
{
    public class UserGrains : BasicGrains<SystemUserLogic>, IUser
    {
        public Task<ListPageModel<SystemUser>> GetSystemUserList(UserSelect model)
        {
            ListPageModel<SystemUser> list = this.Logic.GetSystemUserList(model);
            return Task.FromResult(list);
        }
    }
}
