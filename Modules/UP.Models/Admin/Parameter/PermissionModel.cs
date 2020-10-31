using System.Collections.Generic;

namespace UP.Models.Admin.Parameter
{
    public class PermissionModel
    {
        /// <summary>
        /// 模块ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 父级模块ID
        /// </summary>
        public int ParentID { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 模块路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 模块图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 功能编码
        /// </summary>
        public string FunctionCode { get; set; }

        /// <summary>
        /// 授权ID
        /// </summary>
        public string GrantID { get; set; }

        /// <summary>
        /// 打开方式
        /// 0 默认 1 新页面链接打开 2 调用JS方法
        /// </summary>
        public int OpenMode { get; set; }

        /// <summary>
        /// 该模块下的子集合
        /// </summary>
        public List<PermissionModel> Childrens { get; set; }

        public string HomePage { get; set; }
    }

}
