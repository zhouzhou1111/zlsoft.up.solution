using Microsoft.AspNetCore.Mvc;
using QWPlatform.SystemLibrary.Utils;
using System;
using UP.Basics;
using UP.Interface.Admin.BussinessSys;
using UP.Models.DB.BusinessSys;
using UP.Web.Models.Admin.BussinessSys;

namespace UP.Web.Controllers.Admin.BusinessSysManager
{
    /// <summary>
    /// 产品数据源管理（胡家源）
    /// </summary>
    [ApiGroup(ApiGroupNames.Admin)]
    public class DataSourceController : BasicsController
    {
        /// <summary>
        /// 分页查询所有产品数据源的列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("查询所有产品数据源的列表", "分页查询产品数据源的列表", "胡家源", "2020-09-21")]
        public IActionResult GetDataScoureList(BussinessSysPram model)
        {
            var resModel = new ResponseModel(ResponseCode.Error, "操作失败");
            try
            {
                //实例化数据源接口
                var dataSource = this.GetInstance<IDataSource>();
                //分页查询数据源列表
                var dataSourceList = dataSource.GetDataSourceList(model.page_num, model.page_size, model.keyword, model.productid, model.databasetype)?.Result;
                return Json(new ResponseModel(ResponseCode.Success, "查询成功!", dataSourceList));
            }
            catch (Exception ex)
            {
                LogError("分页查询产品数据源失败", ex);
                resModel.msg = "分页查询产品数据源异常";
            }
            return Json(resModel);
        }

        /// <summary>
        /// 新增或修改数据源
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("新增或修改数据源", "新增或修改数据源", "胡家源", "2020-09-21")]
        public IActionResult AddoUpdate(DataSource model)
        {
            //待返回对象
            var result = new ResponseModel(ResponseCode.Success, "保存数据源成功!");
            var row = 0;
            try
            {
                //新增数据源
                if (model.id == 0)
                {
                    model.密码 = Strings.StrToMD5(model.密码);
                    row = this.Add(model).Execute();
                }
                //修改
                else
                {
                    row = this.Update(model).Columns("产品id", "名称", "数据库类型", "ip", "端口", "实例名", "用户名", "说明")
                            .Where("id", model.id).Execute();
                }
                if (row < 1)
                {
                    result.msg = "服务器内部异常!";
                    result.code = (int)ResponseCode.Error;
                }
            }
            catch (Exception ex)
            {
                LogError("保存数据源失败", ex);
                result.code = (int)ResponseCode.Error;
                result.msg = "服务器内部异常";
            }
            return Json(result);
        }

        /// <summary>
        /// 根据数据源ID 查询数据源信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("根据数据源ID 查询数据源信息", "根据数据源ID查询数据源信息", "胡家源", "2020-09-21")]
        public IActionResult GetDataScoureInfo(BussinessSysPram model)
        {
            //待返回对象
            var result = new ResponseModel(ResponseCode.Success, "查询数据源成功!");

            try
            {
                if (model.datasoureid != 0)
                {
                    //查询数据源
                    var dataScoure = this.Query<DataSource>().Where("id", model.datasoureid).GetModel();
                    //判断数据源是否为空
                    if (dataScoure != null)
                    {
                        result.data = dataScoure;
                    }
                    else
                    {
                        result.msg = "服务器内部异常";
                        result.code = (int)ResponseCode.Error;
                    }
                }
                else
                {
                    result.msg = "传入参数异常";
                    result.code = (int)ResponseCode.Error;
                }
            }
            catch (Exception ex)
            {
                //记录日志
                LogError("传入参数异常", ex);
                result.code = (int)ResponseCode.Error;
                result.msg = "服务器内部异常";
            }
            return Json(result);
        }

        /// <summary>
        /// 根据数据源ID删除数据源
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("根据数据源ID删除数据源", "根据数据源ID删除数据源", "胡家源", "2020-09-21")]
        public IActionResult DelDataScoure(BussinessSysPram model)
        {
            //待返回对象
            var result = new ResponseModel(ResponseCode.Success, "删除数据源成功!");
            try
            {
                //删除数据源
                var row = this.Delete<DataSource>().Where("id", model.datasoureid).Execute();
                if (row < 1)
                {
                    result.code = (int)ResponseCode.Error;
                    result.msg = "数据源删除失败";
                }
            }
            catch (Exception ex)
            {
                LogError("传入参数异常", ex);
                result.code = (int)ResponseCode.Error;
                result.msg = "服务器内部异常";
            }
            return Json(result);
        }

        /// <summary>
        /// 重置数据源密码(胡家源)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [RIPAuthority("重置数据源密码", "重置数据源密码", "胡家源", "2020-09-21")]
        public IActionResult ResetPassword(BussinessSysPram model)
        {
            var result = new ResponseModel(ResponseCode.Success, "重置成功!");
            try
            {
                if (model.datasoureid == 0)
                {
                    result.code = (int)ResponseCode.Forbidden;
                    result.msg = "缺少参数";
                    return Json(result);
                }
                //通过机构人员id查询账户
                DataSource dataScoure = new DataSource()
                {
                    密码 = Strings.StrToMD5("123456")
                };
                var count = this.Update(dataScoure).Columns("密码").Where("id", model.datasoureid).Execute();
                if (count > 0)
                {
                    result.code = (int)ResponseCode.Success;
                }
                else
                {
                    result.code = (int)ResponseCode.Error;
                    result.msg = "重置失败";
                }
            }
            catch (Exception ex)
            {
                LogError("重置数据源密码操作失败", ex);
                result.code = (int)ResponseCode.Error;
                result.msg = "服务器内部异常";
            }
            return Json(result);
        }
    }
}