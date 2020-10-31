using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UP.Basics
{
    /// <summary>
    /// 模块信息
    /// </summary>
    public class MenuModel
    {
        /// <summary>
        /// 模块id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// pid:上级id，sys_menu.id
        /// </summary>
        public int? pid { get; set; }

        /// <summary>
        /// code:菜单代码
        /// </summary>
        [Required]
        public string code { get; set; }

        /// <summary>
        /// name:菜单名称
        /// </summary>
        [Required]
        public string name { get; set; }

        /// <summary>
        /// path:路径
        /// </summary>
        public string path { get; set; }


        /// <summary>
        /// icon:菜单图标
        /// </summary>
        public string icon { get; set; }

        /// <summary>
        /// is_first_page:是否首页，首页不能关闭，0-否;1-是
        /// </summary>
        [Required]
        public int is_first_page { get; set; }

        /// <summary>
        /// sort:排序
        /// </summary>
        [Required]
        public int sort { get; set; }

        /// <summary>
        /// 子级节点
        /// </summary>
        public List<MenuModel> children { get; set; }

        /// <summary>
        /// 操作功能集合（指定某个菜单下的功能操作集合）
        /// </summary>
        public List<ButtonModel> buttons { get; set; }

    }
}