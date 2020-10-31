/*********************************************************
* 功能：模块功能接口数据库模型
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
    ///系统_功能接口
    /// </summary>
    [Table("系统_功能接口")]
    [DataContract]
    public class ModulesFunctionInterface
    {
        [PrimaryKey(true)]
        [DataMember]
        public int id { get; set; }

        /// <summary>
        /// 模块id:
        /// </summary>
        [JsonProperty("mkid")]
        public int 模块id { get; set; }

        /// <summary>
        /// 功能id:
        /// </summary>
        [JsonProperty("gnid")]
        public int 功能id { get; set; }

        /// <summary>
        /// 命名空间名称:
        /// </summary>
        [JsonProperty("mmkjmc")]
        public string 命名空间名称 { get; set; }

        /// <summary>
        /// 控制器名称:
        /// </summary>
        [JsonProperty("kzqmc")]
        public string 控制器名称 { get; set; }

        /// <summary>
        /// 方法名:
        /// </summary>
        [JsonProperty("ffm")]
        public string 方法名 { get; set; }

        /// <summary>
        /// 序号:
        /// </summary>
        [JsonProperty("xh")]
        public int 序号 { get; set; }

        /// <summary>
        /// 是否停用:
        /// </summary>
        [JsonProperty("sfty")]
        public int 是否停用 { get; set; }

        /// <summary>
        /// 停用时间:
        /// </summary>
        [JsonProperty("tysj")]
        public DateTime 停用时间 { get; set; }

        /// <summary>
        /// 登记人:
        /// </summary>
        [JsonProperty("jdr")]
        public string 登记人 { get; set; }

        /// <summary>
        /// 登记时间:
        /// </summary>
        [JsonProperty("djsj")]
        public DateTime 登记时间 { get; set; }

        /// <summary>
        /// 更新时间:
        /// </summary>
        [JsonProperty("gxsj")]
        public DateTime 更新时间 { get; set; }
    }
}