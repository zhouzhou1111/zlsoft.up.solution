/***********************************************************
* 功能：获取数据字典表请求参数
* 描述：
* 作者：贺伟
* 日期：2020-06-18
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UP.Web.Models.Public
{
    /// <summary>
    /// 获取数据字典表请求参数
    /// </summary>
    public class ServerDicParam
    {

        /// <summary>
        /// 字典表类型:dic_婚姻状况=1, dic_既往项目分类=2, dic_民族=3, dic_社会关系=4, dic_学历=5, dic_证件类别=6, dic_政治面貌=7, dic_执业类别=8, dic_职称等级=9, dic_职务=10, dic_职业=11
        /// </summary>
        public int type { get; set; }
    }
}
