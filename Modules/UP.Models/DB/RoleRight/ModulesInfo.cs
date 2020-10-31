/*********************************************************
* 功能：模块配置数据库模型
* 描述：
* 作者：贺伟
* 日期：2020/5/13 17:00
**********************************************************/

using Newtonsoft.Json;
using QWPlatform.Models;
using System;
using System.Runtime.Serialization;

namespace UP.Models.DB.RoleRight
{
    /// <summary>
    ///模块配置
    /// </summary>
    [Table("sys_resource")]
    [DataContract]
    public class ModulesInfo
    {
        [DataMember]
        public string id { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>
        [DataMember]
        public string parent_id { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [DataMember]
        public string code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string name { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [DataMember]
        public string title { get; set; }

        /// <summary>
        /// 路径:访问路径
        /// </summary>
        [DataMember]
        public string path { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [DataMember]
        public int status { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        [DataMember]
        public int sno { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [DataMember]
        public string icon { get; set; }

        /// <summary>
        /// 是否首页:0否，1是
        /// </summary>
        [DataMember]
        public int is_firstpage { get; set; }

        /// <summary>
        /// 资源类型:1:菜单页面,2:功能按钮
        /// </summary>
        [DataMember]
        public int resource_type { get; set; }

        /// <summary>
        /// 是否子页:1:是，0：否（导航只加载为0的主页面）
        /// </summary>
        [DataMember]
        public int is_child_page { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DataMember]
        public string describe { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [DataMember]
        public DateTime update_time { get; set; }
    }
}