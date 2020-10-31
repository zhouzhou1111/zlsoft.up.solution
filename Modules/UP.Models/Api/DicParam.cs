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

namespace UP.Models.Api
{
    /// <summary>
    /// 获取数据字典表请求参数
    /// </summary>
    public class DicParam
    {

        /// <summary>
        /// 字典表类型: b_民族 = 1, b_社会性别 = 2, b_学历 = 3, b_职务 = 4
        /// </summary>
        public int type { get; set; }

    }
}
