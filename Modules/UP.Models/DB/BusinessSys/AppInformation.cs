using Newtonsoft.Json;
using QWPlatform.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace UP.Models.DB.BusinessSys
{
	/// <summary>
	///系统_应用信息
	/// </summary>
	[Table("系统_应用信息")]
	[DataContract]
	public class AppInformation
	{
		/// <summary>
		/// appid:
		/// </summary>
		[JsonProperty("appid")]
		public string appid { get; set; }

		/// <summary>
		/// 应用名称:
		/// </summary>
		[JsonProperty("appname")]
		public string 应用名称 { get; set; }

		/// <summary>
		/// 公钥:调用加密密钥
		/// </summary>
		[JsonProperty("publickey")]
		public string 公钥 { get; set; }

		/// <summary>
		/// 私钥:平台解密密钥
		/// </summary>
		[JsonProperty("privatekey")]
		public string 私钥 { get; set; }

		/// <summary>
		/// 应用状态:1:正常；0:停用
		/// </summary>
		[JsonProperty("appstate")]
		public int 应用状态 { get; set; }

		/// <summary>
		/// 登记时间:
		/// </summary>
		[JsonProperty("regtime")]
		public DateTime 登记时间 { get; set; }

		/// <summary>
		/// 登记人id:
		/// </summary>
		[JsonProperty("regpersonid")]
		public string 登记人id { get; set; }

		/// <summary>
		/// 变更时间:
		/// </summary>
		[JsonProperty("updatetime")]
		public DateTime 变更时间 { get; set; }

		/// <summary>
		/// 变更人id:
		/// </summary>
		[JsonProperty("updatepersonid")]
		public string 变更人id { get; set; }

		/// <summary>
		/// 数据标识: 1:正常； 0:删除
		/// </summary>
		[JsonProperty("state")]
		public int 数据标识 { get; set; }

		/// <summary>
		/// 使用平台:PC=1; h5=2;安卓=3;IOS=4
		/// </summary>
		[JsonProperty("useplatform")]
		public int 使用平台 { get; set; }

		/// <summary>
		/// 产品id:
		/// </summary>
		[JsonProperty("productid")]
		public int 产品id { get; set; }

		/// <summary>
		/// 有效期:
		/// </summary>
		[JsonProperty("validitytime")]
		public DateTime 有效期 { get; set; }

	}
}
