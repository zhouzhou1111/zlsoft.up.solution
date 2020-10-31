using QWPlatform.IService;
using QWPlatform.SystemLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UP.Basics;
using UP.Models.Admin.Parameter;

namespace UP.Logics.Admin.Permission
{
    public class PermissionLogic
    {
        private static volatile PermissionLogic permission;
        /// <summary>
        /// 实现静态访问接口
        /// </summary>
        public static PermissionLogic Instance
        {
            get
            {
                if (permission == null)
                {
                    permission = new PermissionLogic();
                }
                return permission;
            }
        }


        /// <summary>
        /// 根据用户id获取角色授权模块列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResponseModel AloneGetDeptInfo(object model)
        {
            //提示信息
            var result = new ResponseModel(ResponseCode.Success, "获取角色授权模块列表!");
            try
            {
                using (var db = new DbContext())
                {
                    //获取用户基本信息
                    var sqlStr = db.GetSql("A0000-模块配置", null, null);

                    //执行SQL脚本
                    result.data = db.Update(sqlStr)
                            .Parameters("Id", 0).Execute();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("获取角色授权模块列表!", ex);
                result.code = ResponseCode.Error.ToInt32();
                result.msg = "获取角色授权模块列表失败!";
            }
            return result;
        }


        /// <summary>
        /// 根据用户id查询该用户对应的模块列表
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public List<MenuModel> GetPermissionModules(string uid)
        {
            var modules = new List<MenuModel>();
            try
            {
                using (var db = new DbContext())
                {
                    //获取用户基本信息
                    var sqlStr = db.GetSql("AA00012-角色授权模块", null, null);
                    //获取到数据层次
                    var menu_list = db.Sql(sqlStr)
                                        .Parameters("uid", uid)
                                        .GetModelList<MenuModel>();
                    var list = new List<MenuModel>();
                    modules = GetUserMenus(menu_list);
                    //modules = GetTreeModelByTable(table, list, "ID", "上级ID", null);
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("获取角色授权模块列表!", ex);
            }
            return modules;
        }


        /* 描述: 根据数据表,查询树型节点
         */

        private List<PermissionModel> GetTreeModelByTable(DataTable dt, List<PermissionModel> list, string idColName, string parentColName, object initParentValue)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                //检查首层是否为空值
                string filer = string.Empty;
                if (initParentValue != null && initParentValue != DBNull.Value)
                {
                    var type = initParentValue.GetType();
                    if (type.Name != "String")
                    {
                        filer = string.Format("{0}={1}", parentColName, initParentValue);
                    }
                }
                else
                {
                    filer = string.Format("{0} is null", parentColName);
                }

                DataRow[] rows = dt.Select(filer);
                if (rows.Length > 0)
                {
                    foreach (DataRow row in rows)
                    {
                        var model = new PermissionModel();
                        model.ID = row.GetValueByName<int>("ID");
                        model.ParentID = row.GetValueByName<int>("上级ID");
                        model.Name = row.GetValueByName<string>("模块名称");
                        model.Path = row.GetValueByName<string>("模块路径");
                        model.Icon = row.GetValueByName<string>("模块图标");
                        model.GrantID = row.GetValueByName<string>("模块授权ID");
                        model.FunctionCode = row.GetValueByName<string>("模块编码");
                        model.OpenMode = row.GetValueByName<int>("打开方式");
                        model.HomePage = row.GetValueByName<string>("是否首页");
                        if (dt.Select(string.Format("{0}={1}", parentColName, row[idColName])).Length > 0)
                        {
                            List<PermissionModel> childrens = new List<PermissionModel>();
                            var r = GetTreeModelByTable(dt, childrens, idColName, parentColName, model.ID);
                            model.Childrens = r;
                        }

                        list.Add(model);
                    }
                }
            }

            return list;
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
