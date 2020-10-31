using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UP.Basics.Plugins.ZlsoftFtp
{
    /// <summary>
    /// 上传文件信息
    /// </summary>
    public class UploadFileModel
    {
        /// <summary>
        /// 上传人id
        /// </summary>
        public string upload_userid { get; set; }

        /// <summary>
        /// 上传类别(1身份证,2头像，3协议照片,4面签照片,5个人信息二维码,6医院主页图片，7医院动态图片，8广告，9报告文件，10处方文件，11医生动态，，12 推送文件,13 医生签约签名,14 IM消息抄送,15投诉附件,16富文本文件,17远程审方)
        /// </summary>
        public int upload_type { get; set; }

        /// <summary>
        /// 水印名称
        /// </summary>
        public string watermark { get; set; }

        /// <summary>
        /// 是否生成缩略图 0否，1是
        /// </summary>
        public int is_thumb { get; set; }

        /// <summary>
        /// 文件数据
        /// </summary>
        public List<FileData> file_items { get; set; }
    }

    public class FileData
    {
        /// <summary>
        /// 文件名称，带后缀
        /// </summary>
        public string filename { get; set; }

        /// <summary>
        /// 文件byte[]
        /// </summary>
        public byte[] filedata { get; set; }

        /// <summary>
        /// 文件base64字符串
        /// </summary>
        public string filestr { get; set; }
    }
}