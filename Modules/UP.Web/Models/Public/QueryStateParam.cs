/***********************************************************
* 功能：针对单个对象修改状态信息条件对象
* 描述：
* 作者：陈洁
* 日期：2020-05-20
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UP.Web.Models.Public
{
    /// <summary>
    /// 修改状态入参
    /// </summary>
    public class QueryStateParam
    {
        /// <summary>
        /// id
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 修改的状态,1启用,0禁用
        /// </summary>
        public int state { get; set; }
        
    }

}
