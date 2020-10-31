/****************************************************
* 功能：系统管理数据层
* 描述：
* 作者：贺伟
* 日期：2020/05/19 19:04:03
*********************************************************/
using QWPlatform.IService;
using QWPlatform.SystemLibrary;
using System;
using System.Collections.Generic;
using UP.Models.Apply;

namespace UP.Logics.DBTable
{
    /// <summary>
    /// 系统操作数据处理
    /// </summary>
    public class SystemLogic
    {

        /// <summary>
        /// 查询平台的应用信息
        /// </summary>
        /// <param name="Id">医生id</param>
        /// <returns></returns>
        public List<AppLyInfo> SelectAppInfo()
        {
            List<AppLyInfo> items = null;
            try
            {
                using (var db = new DbContext())
                {
                    //执行SQL脚本
                    items = db.Select("平台应用信息")
                        .Where("数据标识", 1)
                        .GetModelList<AppLyInfo>();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("查询平台的应用信息发生异常错误!", ex);
            }
            return items;
        }
    }
}
