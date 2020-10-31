using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UP.Web.Models.Public.File
{
    /// <summary>
    /// 查询文件信息参数
    /// </summary>
    public class FileParam
    {
        /// <summary>
        /// 文件id,多个以","分隔
        /// </summary>
        public string fileid { get; set; }
        /// <summary>
        /// 图片文件类型 1原图，2缩略图，3大图
        /// </summary>
        public int imgfiletype { get; set; }
    }
}
