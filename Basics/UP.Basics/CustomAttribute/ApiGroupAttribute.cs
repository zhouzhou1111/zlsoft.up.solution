/********************************************************
* 功能：自定义WebApi分组设置 
* 描述：
* 作者：王海洋
* 日期：2020-01-16
*********************************************************/
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System;

namespace UP.Basics
{
    /// <summary>
    /// 自定义分组属性
    /// </summary>
    public class ApiGroupAttribute : Attribute, IApiDescriptionGroupNameProvider
    {
        /// <summary>
        /// 指定Webapi分组设置
        /// </summary>
        /// <param name="name">分组名称</param>
        public ApiGroupAttribute(ApiGroupNames name)
        {
            GroupName = name.ToString();
        }

        /// <summary>
        /// 分组名称
        /// </summary>
        public string GroupName { get; set; }
    }
}
