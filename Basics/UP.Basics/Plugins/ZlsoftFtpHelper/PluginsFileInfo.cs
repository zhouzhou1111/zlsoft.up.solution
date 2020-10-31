namespace UP.Basics.Plugins.ZlsoftFtp
{
    /// <summary>
    /// 插件返回文件对象
    /// </summary>
    public class PluginsFileInfo
    {
        #region 公共属性

        /// <summary>
        /// 文件id
        /// </summary>
        public string fileid { set; get; }

        /// <summary>
        /// 文件byte[]
        /// </summary>
        public byte[] filedata { set; get; }

        /// <summary>
        /// 后缀名（文件格式）
        /// </summary>
        public string format { set; get; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string fileName { set; get; }

        /// <summary>
        /// 水印名称
        /// </summary>
        public string watermark { set; get; }

        /// <summary>
        /// 文件url
        /// </summary>
        public string filepath { set; get; }

        #endregion 公共属性
    }
}