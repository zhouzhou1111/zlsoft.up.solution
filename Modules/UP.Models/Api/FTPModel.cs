using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UP.Models.Api
{
    /// <summary>
    /// ftp文件信息
    /// </summary>
    [DataContract]
    public class FTPModel
    {
        #region 公共属性

        /// <summary>
        /// 上传文件路径（不包含根目录名称），
        /// </summary>
        public string filePath { get; set; }

        /// <summary>
        /// 后缀名（文件格式）
        /// </summary>
        [DataMember]
        public string extensionName { get; set; }

        /// <summary>
        /// 文件id，long
        /// </summary>
        [DataMember]
        public long fileId { get; set; }

        /// <summary>
        ///文件类型： 1身份证,2头像，3协议照片,4面签照片,5个人信息二维码,6医院主页图片，7医院动态图片，8广告，9报告文件，10处方文件，11医生动态，，12 推送文件,13 医生签约签名,14 IM消息抄送,15投诉附件,16富文本文件,17远程审方
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// ftp文件名称 guid+后缀名
        /// </summary>
        public string ftpFileName
        {
            get
            {
                return this.md5 + "." + this.extensionName;
            }
        }

        /// <summary>
        /// 上传时间
        /// </summary>
        [DataMember]
        public DateTime uploadDate { set; get; }

        /// <summary>
        /// 实际名称
        /// </summary>
        [DataMember]
        public string realName { get; set; }

        [DataMember]
        public string md5 { set; get; }

        /// <summary>
        /// 水印
        /// </summary>
        public string watermark { set; get; }

        /// <summary>
        /// 文件byte[]
        /// </summary>
        [DataMember]
        public byte[] filedata { set; get; }

        /// <summary>
        /// 缩略图文件byte[]
        /// </summary>
        public List<byte[]> thumb_filedata { set; get; }

        /// <summary>
        /// 原始文件url，只在上传使用
        /// </summary>
        public string oldfile_url { get; set; }

        #endregion 公共属性
    }

    /// <summary>
    /// 上传文件信息
    /// </summary>
    public class FTPUploadFileModel
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
        public List<FTPFileData> file_items { get; set; }
    }

    public class FTPFileData
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

        /// <summary>
        /// 文件url地址
        /// </summary>
        public string file_url { get; set; }
    }

    /// <summary>
    /// 生成缩略图的方式 HW指定高宽缩放(可能变形),W指定宽，高按比例,H指定高，宽按比例,Cut定高宽裁减(不变形),默认HW
    /// </summary>
    public enum Mode
    {
        HW = 1, W = 2, H = 3, Cut = 4
    }

    /// <summary>
    /// 图片文件类型 1原图，2缩略图，3大图
    /// </summary>
    public enum ImgFileType
    {
        原图 = 1, 缩略图 = 2, 大图 = 3
    }
}
