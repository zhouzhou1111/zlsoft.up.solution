using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using QWPlatform.SystemLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UP.Basics;
using UP.Basics.Plugins.ZlsoftFtp;
using UP.Web.Models.Public.File;

namespace UP.Web.Controllers.Api
{
    /// <summary>
    ///公共接口服务
    /// </summary>
    [ApiGroup(ApiGroupNames.PUBLIC)]
    public class FileController : BasicsController
    {
        /// <summary>
        /// 上传文件，传入格式以base64（贺伟）
        /// </summary>
        /// <param name="file">
        /// 需要上传的文件信息，文件内容使用base64
        ///</param>
        /// <returns>
        /// "本次上传的文件id（有多个用,隔开）"
        /// </returns>
        [HttpPost]
        [AllowAnonymous]
        [DisableRequestSizeLimit]
        [ProducesResponseType(typeof(UploadFileModel), 200)]
        [UPEncryptionAttribute("接口请求参数不需要加密，输出参数也不需要加密", false, false)]
        [RIPAuthority("上传文件", "上传文件，本次上传的文件id（有多个用,隔开）", "贺伟", "2020-07-10")]
        public IActionResult UploadByteArray(UploadFileModel file)
        {
            ResponseModel result = new ResponseModel(ResponseCode.Error, "文件上传失败");
            try
            {
                if (loginUser != null)
                {
                    file.upload_userid = loginUser.id.ToString();
                }
                //ftp文件上传
                var jsonresult = ZlsoftFtpHelper.Upload(file, HttpContext.Request.Host.Value);
                if (jsonresult.code == ResponseCode.Success)
                {
                    result.code = jsonresult.code.ToInt32();
                    result.data = jsonresult.data;
                    result.msg = jsonresult.msg;
                }
            }
            catch (Exception ex)
            {
                result.msg = ex.Message;
                Logger.Instance.Error("上传文件发生异常", ex);
            }
            return Json(result);
        }

        /// <summary>
        /// 获取多个连接信息
        /// </summary>
        /// <param name="param">请求文件信息</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<PluginsFileInfo>), 200)]
        [UPEncryptionAttribute("接口请求参数不需要加密，输出参数也不需要加密", false, false)]
        [RIPAuthority("获取多个连接信息", "获取多个连接信息，文件id（有多个用,隔开）", "贺伟", "2020-07-10")]
        public IActionResult GetFileUrl(FileParam param)
        {
            ResponseModel result = new ResponseModel(ResponseCode.Error, "获取文件url失败");
            try
            {
                //获取文件地址信息
                var jsonresult = ZlsoftFtpHelper.GetFileUrl(param.fileid, param.imgfiletype);
                if (jsonresult.code == ResponseCode.Success)
                {
                    result.code = jsonresult.code.ToInt32();
                    result.data = jsonresult.data;
                    result.msg = jsonresult.msg;
                }
            }
            catch (Exception ex)
            {
                result.msg = ex.Message;
                Logger.Instance.Error("获取多个连接信息发生异常", ex);
            }

            return Json(result);
        }

        /// <summary>
        /// 获取单个连接信息，直接返回url地址
        /// </summary>
        /// <param name="param">请求文件信息</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [UPEncryptionAttribute("接口请求参数不需要加密，输出参数也不需要加密", false, false)]
        [RIPAuthority("获取单个连接信息", "获取单个连接信息，文件id", "贺伟", "2020-07-10")]
        public IActionResult GetFileUrlItem(FileParam param)
        {
            string url = "";
            ResponseModel result = new ResponseModel(ResponseCode.Error, "获取文件url失败");
            try
            {
                //获取文件地址信息
                var jsonresult = ZlsoftFtpHelper.GetFileUrl(param.fileid, param.imgfiletype);
                if (jsonresult.code == ResponseCode.Success)
                {
                    if (jsonresult.data != null && jsonresult.data.Any())
                    {
                        url = jsonresult.data.FirstOrDefault().filepath;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("获取单个连接信息发生异常", ex);
            }
            return Json(url);
        }


        /// <summary>
        /// 获取单个文件url，直接返回url地址
        /// </summary>
        /// <param name="fileid">必填，文件id</param>
        /// <param name="imgfiletype">选填，图片文件类型 1原图，2缩略图，3大图</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [RIPAuthority("获取文件url", "获取文件url，文件id", "贺伟", "2020-07-10")]
        public IActionResult GetFileLinkUrl(string fileid, int imgfiletype)
        {
            string url = "";
            ResponseModel result = new ResponseModel(ResponseCode.Error, "获取文件url失败");
            try
            {
                //获取文件地址信息
                var jsonresult = ZlsoftFtpHelper.GetFileUrl(fileid, imgfiletype);
                if (jsonresult.code == ResponseCode.Success)
                {
                    if (jsonresult.data != null && jsonresult.data.Any())
                    {
                        url = jsonresult.data.FirstOrDefault().filepath;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("获取文件url发生异常", ex);
            }
            return Json(url);
        }


        /// <summary>
        /// 跳转单个文件url
        /// </summary>
        /// <param name="fileid">必填，文件id</param>
        /// <param name="imgfiletype">选填，图片文件类型 1原图，2缩略图，3大图</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [RIPAuthority("跳转单个文件url", "跳转单个文件url，文件id", "贺伟", "2020-07-10")]
        public IActionResult ToFileLinkUrl(string fileid, int imgfiletype)
        {
            string url = "";
            ResponseModel result = new ResponseModel(ResponseCode.Error, "获取文件url失败");
            try
            {
                //获取文件地址信息
                var jsonresult = ZlsoftFtpHelper.GetFileUrl(fileid, imgfiletype);
                if (jsonresult.code == ResponseCode.Success)
                {
                    if (jsonresult.data != null && jsonresult.data.Any())
                    {
                        url = jsonresult.data.FirstOrDefault().filepath;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("跳转单个文件url发生异常", ex);
            }
            //url跳转
            if (!url.IsNullOrEmpty())
                return Redirect(url);
            return Json(result);
        }
    }
}