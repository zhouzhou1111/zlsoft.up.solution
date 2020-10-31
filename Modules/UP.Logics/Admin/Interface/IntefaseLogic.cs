using QWPlatform.DataIntface.Builders;
using QWPlatform.IService;
using QWPlatform.SystemLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using UP.Basics;
using UP.Basics.Models;
using UP.Logics.Admin.Permission;
using UP.Models.Admin.RoleRight;
using UP.Models.DB.RoleRight;


namespace UP.Logics.Admin.Interface
{
    public class IntefaseLogic
    {
        /// <summary>
        /// 根据模块id和功能id获取接口列表
        /// </summary>
        /// <param name="mkid">模块id</param>
        /// <param name="gnid">功能id</param>
        /// <param name="roleid">角色id</param>
        /// <param name="type">0为加载全部,1为加载未停用</param>
        /// <returns></returns>
        public List<ModulesFunctionInterfaceDto> getInterfaceList(int mkid, int gnid,int roleid,int type=0)
        {
            List<ModulesFunctionInterfaceDto> item = null;
            try
            {
                using (var db = new DbContext())
                {
                    //获取角色的模块信息
                    var sqlStr = db.GetSql("AA00015-查询模块功能接口列表", null, null);
                    //执行SQL脚本
                    ISqlBuilder sqlBuilder = db.Sql(sqlStr)
                        .Parameters("mkid", mkid)
                        .Parameters("gnid", gnid)
                        .Parameters("roleModularId", roleid);
                    //执行SQL脚本
                    item = sqlBuilder.GetModelList<ModulesFunctionInterfaceDto>();
                    if (type==1)
                    {
                        item = item.Where(d => d.是否停用 == 0).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("查询人员登录信息发生异常：", ex);
            }
            return item;
        }

        /// <summary>
        /// 根据模块id和功能id获取接口列表
        /// </summary>
        /// <param name="mkid">模块id</param>
        /// <param name="gnid">功能id</param>
        /// <param name="roleid">角色id</param>
        /// <returns></returns>
        public List<ModulesFunctionInterfaceDto> getCheckInterfaceList(int mkid, int gnid, int roleid)
        {
            List<ModulesFunctionInterfaceDto> item = null;
            try
            {
                using (var db = new DbContext())
                {
                    //获取角色的模块信息
                    var sqlStr = db.GetSql("AA00017-查询角色接口列表", null, null);
                    //执行SQL脚本
                    ISqlBuilder sqlBuilder = db.Sql(sqlStr)
                        .Parameters("mkid", mkid)
                        .Parameters("gnid", gnid)
                        .Parameters("roleModularId", roleid);
                    //执行SQL脚本
                    item = sqlBuilder.GetModelList<ModulesFunctionInterfaceDto>();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("查询人员登录信息发生异常：", ex);
            }
            return item;
        }

        /// <summary>
        /// 保存角色模块功能接口
        /// </summary>
        /// <param name="roleid">角色id</param>
        /// <param name="mkid">模块id</param>
        /// <param name="gnid">功能id</param>
        /// <param name="interfaceids">接口ids</param>
        /// <returns></returns>
        public ResponseModel saveRoleModuleFuncInterFace(int roleid, int mkid, int gnid, string interfaceids)
        {
            //提示信息
            var result = new ResponseModel(ResponseCode.Success, "保存角色模块功能接口成功!");
            try
            {
                using (var db = new DbContext())
                {
                    var value = db.DelegateTrans<bool>(() =>
                    {
                        var arr = interfaceids.Split(',');
                        var count = 0;
                        if (arr != null && !arr.Any())
                        {
                            return false;
                        }
                        //根据角色id,模块id,功能id 删除系统_角色功能接口 所有数据
                        db.Delete("系统_角色功能接口").Where("角色id", roleid).Where("模块id", mkid).Where("功能id", gnid).Execute();
                        //循环添加系统_角色功能接口 
                        foreach (var item in arr)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                count = db.Insert("系统_角色功能接口").Column("角色id", roleid).Column("模块id", mkid).Column("功能id", gnid).Column("接口id", item.ToInt32()).Execute();
                            }
                            if (count <= 0)
                            {
                                db.Rollback();
                                return false;
                            }
                        }
                        return true;
                    });
                    if (!value)
                    {
                        result.code = ResponseCode.Error.ToInt32();
                        result.msg = "保存角色模块功能接口失败!";
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("保存角色模块功能接口异常!", ex);
                result.code = ResponseCode.Error.ToInt32();
                result.msg = "保存角色的模块失败!";
                throw;
            }
            return result;
        }
    }
}
