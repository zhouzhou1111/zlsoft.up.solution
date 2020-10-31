using QWPlatform.IService;
using QWPlatform.SystemLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using UP.Basics;
using UP.Basics.Models;
using UP.Logics.Admin.Permission;
using UP.Models.Admin.Login;
using UP.Models.DB.RoleRight;

namespace UP.Logics.Admin.Login
{
    public class LoginLogic
    {
        /// <summary>
        /// 医生登录,陈洁修改
        /// </summary>
        /// <param name="account_name">登录账户-手机号</param>
        /// <param name="password">登录密码</param>
        /// <returns></returns>
        public JsonMsg<LoginInfoModel> DoctorLogin(string account_name, string password)
        {
            string passwordMD5 = QWPlatform.SystemLibrary.Utils.Strings.StrToMD5(password);
            var result = JsonMsg<LoginInfoModel>.OK(null, "人员登录成功!");
            LoginInfoModel loginInfo = null;
            try
            {
                using (var db = new DbContext())
                {
                    //定义sql执行对象
                    var sqlBuilder = db.Sql("");
                    //SQL动态参数
                    List<string> list = new List<string>();
                    //如果包括account_name参数则使用account_name
                    sqlBuilder.Parameters("account", account_name);
                    list.Add("account");
                    sqlBuilder.Parameters("password", passwordMD5);
                    list.Add("password");
                    //获取执行的sql
                    string sql = db.GetSql("AB00001-平台身份认证", null, list.ToArray());
                    //执行数据查询
                    var items = sqlBuilder.SqlText(sql).GetModelList<LoginInfoModel>();
                    if (items == null || !items.Any())
                    {
                        result.msg = "账户或密码不正确！";
                        result.code = ResponseCode.Error;
                        return result;
                    }
                    loginInfo = items.FirstOrDefault(x => x.account_status == 1);
                    if (loginInfo == null)
                    {
                        result.msg = "账户已被停用,请与管理员联系！";
                        result.code = ResponseCode.Error;
                        return result;
                    }
                    loginInfo = items.FirstOrDefault(x => x.account_status == 1 && x.status == 1);
                    if (loginInfo == null)
                    {
                        result.msg = "该账户已被停用,请与管理员联系！";
                        result.code = ResponseCode.Error;
                        return result;
                    }
                    result.data = loginInfo;
                }
            }
            catch (Exception ex)
            {
                result.msg = "查询人员登录信息发生异常！";
                result.code = ResponseCode.Error;
                Logger.Instance.Error(result.msg, ex);
            }
            return result;
        }

        /// <summary>
        /// 获取医生信息及权限信息
        /// </summary>
        /// <param name="login_name">登录名</param>
        /// <returns></returns>
        public UserModel GetAccountInfo(string login_name)
        {
            if (!login_name.IsNullOrEmpty())
            {
                using (var db = new DbContext())
                {
                    var strs = new List<string>();
                    strs.Add("account");
                    //获取用户基本信息
                    var menusql = db.GetSql("BA00002-查询人员基本账户信息", null, strs.ToArray());
                    //执行SQL脚本
                    var model = db.Sql(menusql)
                                 .Parameters("account", login_name)
                                 .GetModel<UserModel>();
                    if (model != null && !model.id.IsNullOrEmpty())
                    {
                        //查询用户角色信息
                        var userroles = db.Select("sys_account_role")
                            .Columns("*")
                            .Where("account_id", model.id)
                            .OrderBy("role_id")
                            .GetModelList<SystemUserRole>();
                        //判断是否是超级管理员,角色id为1表示是超级管理员
                        if (userroles != null && userroles.Any())
                        {
                            //判断是否是超级管理员
                            if (userroles.Select(t => t.role_id).Contains("1"))
                            {
                                model.is_super_admin = 1;
                            }
                            List<MenuModel> menu_list = null;
                            var menu_sql = string.Empty;
                            //超级管理员拥有所有权限
                            if (model.is_super_admin == 1)
                            {
                                //查询系统所有的模块
                                menu_sql = db.GetSql("AA00011-获取系统所有模块", null, null);
                                menu_list = db.Sql(menu_sql)
                                                .GetModelList<MenuModel>();
                            }
                            else
                            {
                                //查询出该帐户可访问的菜单集合
                                menu_sql = db.GetSql("AA00012-角色授权模块", null, null);
                                menu_list = db.Sql(menu_sql)
                                                .Parameters("uid", model.id)
                                                .GetModelList<MenuModel>();
                            }
                            if (menu_list != null && menu_list.Any())
                            {
                                //菜单集合
                                model.menus = GetUserMenus(menu_list);

                                //查询出该用户菜单下可访问的功能集合
                                string rids = string.Join(',', userroles.Select(t => t.role_id));
                                var funsql = db.GetSql("AA00013-角色包含的所有功能", null, null);
                                var funList = db.Sql(funsql)
                                                .Parameters("mids", rids)
                                                .GetModelList<ButtonModel>();
                                if (funList != null && funList.Any())
                                {
                                    //循环每个菜单，将每个菜单下的功能添加到按钮集合中(并且判断是否有子集)
                                    model.menus?.ForEach(menu =>
                                    {
                                        //找出所有按钮
                                        SetSubMenusButtons(menu, funList);
                                    });
                                }

                                //查询出该用户接口访问集合
                                var actionSql = db.GetSql("AA00014-查询角色授权的所有接口", null, null);
                                var actions = db.Sql(actionSql)
                                               .Parameters("mids", rids)
                                               .GetModelList<ButtonActionModel>();
                                if (actions != null && actions.Any())
                                {
                                    model.actions = actions;
                                }
                            }
                        }
                    }
                    //返回执行结果
                    return model;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取医生基本信息，不包含医生角色及权限
        /// </summary>
        /// <param name="docids">医生id列表</param>
        /// <returns></returns>
        public List<UserModel> GetUserAccountItems(IEnumerable<int> docids)
        {
            if (docids != null && docids.Any())
            {
                using (var db = new DbContext())
                {
                    var strs = new List<string>();
                    strs.Add("ids");
                    //获取用户基本信息
                    var menusql = db.GetSql("CA00013-查询医生基本账户信息", null, strs.ToArray());
                    //执行SQL脚本
                    var items = db.Sql(menusql)
                                 .Parameters("ids", string.Join(',', docids))
                                 .GetModelList<UserModel>();

                    //返回执行结果
                    return items;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取登录模块信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<MenuModel> Get_DoctorModules(string userId)
        {
            return PermissionLogic.Instance.GetPermissionModules(userId);
        }

        #region 用于递归查询子级菜单

        /// <summary>
        /// 获取用户所有菜单集合（带有上下级关系）
        /// </summary>
        /// <param name="allmenus">所有菜单信息</param>
        /// <returns></returns>
        private List<MenuModel> GetUserMenus(List<MenuModel> allmenus)
        {
            var menus = new List<MenuModel>();
            if (allmenus != null)
            {
                //查询第一级菜单
                var firstMenus = allmenus.FindAll(p => p.pid == null || p.pid == 0);
                firstMenus?.ForEach(item =>
                {
                    //循环所有子级
                    MenuModel model = new MenuModel
                    {
                        id = item.id,
                        pid = item.pid,
                        name = item.name,
                        code = item.code,
                        icon = item.icon,
                        path = item.path,
                        is_first_page = item.is_first_page
                    };
                    //查询子级菜单
                    FindSubMenus(model, allmenus);

                    //添加到集合中
                    menus.Add(model);
                });
            }
            return menus;
        }

        /// <summary>
        /// 查询子级菜单
        /// </summary>
        /// <param name="model"></param>
        /// <param name="allmenus"></param>
        private void FindSubMenus(MenuModel model, List<MenuModel> allmenus)
        {
            //尝试查询子级菜单
            var submenus = allmenus?.FindAll(p => p.pid == model.id);
            submenus?.ForEach(item =>
            {
                //循环查询子级并添加
                if (model.children == null)
                {
                    //并集合初始化一次
                    model.children = new List<MenuModel>();
                }
                MenuModel submenusModel = new MenuModel
                {
                    //创建当前级别菜单
                    id = item.id,
                    pid = item.pid,
                    name = item.name,
                    code = item.code,
                    icon = item.icon,
                    path = item.path,
                    is_first_page = item.is_first_page
                };
                //再次检查当前子菜单是否还有子级菜单，如果有则添加到subMenusModel
                FindSubMenus(submenusModel, allmenus);
                //添加到子级菜单的集合中
                model.children.Add(submenusModel);
            });
        }

        /// <summary>
        /// chenyongpo
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="funs"></param>
        private void SetSubMenusButtons(MenuModel menu, List<ButtonModel> funs)
        {
            var buttons = funs.FindAll(fun => fun.menu_id == menu.id);
            if (buttons != null && buttons.Any())
            {
                menu.buttons = buttons;
            }
            //如果包括子集，则递归查询赋值
            menu.children?.ForEach(m =>
            {
                SetSubMenusButtons(m, funs);
            });
        }

        #endregion 用于递归查询子级菜单
    }
}