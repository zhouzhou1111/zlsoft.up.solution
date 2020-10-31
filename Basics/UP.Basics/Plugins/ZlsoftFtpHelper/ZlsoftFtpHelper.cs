using QWPlatform.SystemLibrary;
using QWPlatform.SystemLibrary.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UP.Basics.Common;
using UP.Basics.Models;
using static UP.Basics.Plugins.ZlsoftFtp.FtpAjaxResult;

namespace UP.Basics.Plugins.ZlsoftFtp
{
    /// <summary>
    /// 创建ftp
    /// </summary>
    public class ZlsoftFtpHelper
    {
        /// <summary>
        /// 日志操作
        /// </summary>
        private static Logger Log = Logger.Instance;

        /// <summary>
        /// 获取ftp服务地址
        /// </summary>
        private static string ftpurl = "";

        /// <summary>
        /// 根据网络请求获取ftp地址信息
        /// </summary>
        /// <param name="hostname"></param>
        private static void SetFtpUrl(string hostname)
        {
            // 服务器配置的ftp网络请求
            string ftpsetting = AppConfigurtaionServices.Configuration["ftpsetting"];
            if (!ftpsetting.IsNullOrEmpty())
            {
                var ftpsettings = ftpsetting.Split('|');
                string[] items = null;
                //默认使用首项配置
                var ftpsetitem = ftpsettings.FirstOrDefault(t => t.Contains(hostname));
                if (ftpsetitem != null)
                {
                    items = ftpsetitem.Split("$");
                }
                else
                {
                    items = ftpsettings.FirstOrDefault().Split("$");
                }
                //ftp地址复制
                if (items.Length > 1)
                    ftpurl = items[1];
                else ftpurl = items[0];
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="uploadfile">上传文件信息</param>
        /// <returns>返回文件id</returns>
        public static JsonMsg<string> Upload(UploadFileModel uploadfile, string hostname = "localhost")
        {
            //提示信息
            var result = JsonMsg<string>.Error(null, "查询成功");
            if (uploadfile == null || uploadfile.file_items == null || !uploadfile.file_items.Any())
            {
                result.msg = "上传文件不能为空";
                return result;
            }
            //根据网络请求获取ftp地址信息
            SetFtpUrl(hostname);
            string fileid = string.Empty;
            var requesturl = ftpurl + "/FTPService.asmx/uploadBase64";
            foreach (var item in uploadfile.file_items)
            {
                try
                {
                    //fileData, string fileName, string type
                    Dictionary<string, string> parameters = new Dictionary<string, string>();
                    parameters.Add("fileData", item.filestr);
                    parameters.Add("fileName", item.filename);
                    parameters.Add("type", uploadfile.upload_type.ToString());
                    var resultStr = WebServiceHttpHelper.GetResponseString(requesturl, HttpMethod.Post, parameters, Encoding.UTF8, Encoding.UTF8, 30000);
                    if (!resultStr.IsNullOrEmpty())
                    {
                        //转换为ftp返回对象
                        FtpAjaxResult ftpresult = SerializeHelper.GetXmlData<FtpAjaxResult>(resultStr);
                        if (ftpresult != null && ftpresult.code == AjaxResultType.成功)
                        {
                            fileid += ftpresult.data.ToString()+",";
                            result.code = ResponseCode.Success;
                        }
                    }
                }
                catch (Exception ex)
                {
                    var msg = "调用ftp服务发生异常";
                    Log.Error(msg, ex);
                }
            }
            if (result.code == ResponseCode.Success) {
                result.data = fileid.Trim(',');
            }
            return result;
        }

        /// <summary>
        ///  根据文件id获取文件对象json字符串，id:,base64:,fileName:,format:
        /// </summary>
        /// <param name="fileId">文件id</param>
        /// <param name="imgFileType">图片类型 1原图，2缩略图，3大图</param>
        /// <returns></returns>
        public static JsonMsg<PluginsFileInfo> getNewBase64File(string fileId, int imgFileType = 1, string hostname = "localhost")
        {
            //提示信息
            var result = JsonMsg<PluginsFileInfo>.Error(null, "查询成功");
            if (fileId.IsNullOrEmpty())
            {
                result.msg = "文件id不能为空";
                return result;
            }
            //根据网络请求获取ftp地址信息
            SetFtpUrl(hostname);
            //判断文件id是否为空
            if (!fileId.IsNullOrEmpty())
            {
                var requesturl = ftpurl + "/FTPService.asmx/getNewBase64File";
                try
                {
                    //fileId, imgFileType
                    string filetype = "o";
                    if (imgFileType == 2)
                    {
                        filetype = "s";
                    }
                    else if (imgFileType == 3)
                    {
                        filetype = "m";
                    }
                    Dictionary<string, string> parameters = new Dictionary<string, string>();
                    parameters.Add("fileId", fileId);
                    parameters.Add("imgFileType", filetype);
                    var resultStr = WebServiceHttpHelper.GetResponseString(requesturl, HttpMethod.Post, parameters, Encoding.UTF8, Encoding.UTF8, 30000);
                    if (!resultStr.IsNullOrEmpty())
                    {
                        //转换为ftp返回对象
                        FtpAjaxResult ftpresult = SerializeHelper.GetXmlData<FtpAjaxResult>(resultStr);
                        if (ftpresult != null && ftpresult.code == AjaxResultType.成功)
                        {
                            result.data = ftpresult.data.CopyModel<PluginsFileInfo>();
                            result.code = ResponseCode.Success;
                        }
                    }
                }
                catch (Exception ex)
                {
                    var msg = "调用ftp服务发生异常";
                    Log.Error(msg, ex);
                }
            }
            return result;
        }

        /// <summary>
        ///  根据文件id获取文件url多个以","分隔，返回文件路径相对路径，需要自行拼接ftp的域:端口，空表示未找到文件
        /// </summary>
        /// <param name="fileId">文件id</param>
        /// <param name="imgFileType">图片类型 1原图，2缩略图，3大图</param>
        /// <returns></returns>
        public static JsonMsg<List<PluginsFileInfo>> GetFileUrl(string fileId, int imgFileType = 1, string hostname = "localhost")
        {
            //提示信息
            var result = JsonMsg<List<PluginsFileInfo>>.Error(null, "查询成功");
            if (fileId.IsNullOrEmpty())
            {
                result.msg = "文件id不能为空";
                return result;
            }
            //根据网络请求获取ftp地址信息
            SetFtpUrl(hostname);
            //判断文件id是否为空
            if (!fileId.IsNullOrEmpty())
            {
                var requesturl = ftpurl + "/FTPService.asmx/GetFileUrl";
                try
                {
                    //fileId, imgFileType
                    string filetype = "o";
                    if (imgFileType == 2)
                    {
                        filetype = "s";
                    }
                    else if (imgFileType == 3)
                    {
                        filetype = "m";
                    }
                    Dictionary<string, string> parameters = new Dictionary<string, string>();
                    parameters.Add("fileId", fileId);
                    parameters.Add("imgFileType", filetype);
                    var resultStr = WebServiceHttpHelper.GetResponseString(requesturl, HttpMethod.Post, parameters, Encoding.UTF8, Encoding.UTF8, 30000);
                    if (!resultStr.IsNullOrEmpty())
                    {
                        //转换为ftp返回对象
                        FtpAjaxResult ftpresult = SerializeHelper.GetXmlData<FtpAjaxResult>(resultStr);
                        if (ftpresult != null && ftpresult.code == AjaxResultType.成功)
                        {
                            var items = ftpresult.data.CopyModel<List<PluginsFileInfo>>();
                            if (items != null && items.Any())
                            {
                                foreach (var item in items)
                                {
                                    item.filepath = ftpurl + item.filepath;
                                }
                                result.data = items;
                                result.code = ResponseCode.Success;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    var msg = "调用ftp服务发生异常";
                    Log.Error(msg, ex);
                }
            }
            return result;
        }
    }

    /// <summary>
    /// ftp地址信息
    /// </summary>
    public class FtpurlInfo
    {
        /// <summary>
        /// 请求传入的域
        /// </summary>
        public string hosturl { get; set; }

        /// <summary>
        /// ftp调用的url
        /// </summary>
        public string requesturl { get; set; }
    }
}