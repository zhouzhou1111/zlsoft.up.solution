using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UP.Web.Models.Admin.ModulesFunctionInterface
{
    public class ModulesFuncInterfaceParam
    {

        /// <summary>
        ///主键id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 模块id
        /// </summary>
        public int mkid { get; set; }
        /// <summary>
        /// 功能id
        /// </summary>
        public int gnid { get; set; }
        /// <summary>
        /// 角色id
        /// </summary>
        public int jsid { get; set; }
        /// <summary>
        /// 接口ids
        /// </summary>
        public string jkids { get; set; }

        /// <summary>
        ///名称/关键字
        /// </summary>
        public string keyword { get; set; }

        public string Url { get; set; }

        /// <summary>
        /// 判断接口列表显示全部或者显示启用（0为显示全部,1为显示启用）
        /// </summary>
        public int type { get; set; }
    }
}
