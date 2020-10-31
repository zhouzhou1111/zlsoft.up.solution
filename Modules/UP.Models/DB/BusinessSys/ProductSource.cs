using Newtonsoft.Json;
using QWPlatform.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace UP.Models.DB.BusinessSys
{
	/// <summary>
	///系统_产品服务源
	/// </summary>
	[Table("系统_产品服务源")]
	[DataContract]
	public class ProductSource
	{
		/// <summary>
		/// id:服务id
		/// </summary>
		[PrimaryKey(true)]
		[JsonProperty("id")]
		public int id { get; set; }

		/// <summary>
		/// 产品id:产品id
		/// </summary>
		[JsonProperty("productid")]
		public int 产品id { get; set; }

		/// <summary>
		/// 名称:服务名称
		/// </summary>
		[JsonProperty("name")]
		public string 名称 { get; set; }

		/// <summary>
		/// 服务地址:访问服务的地址
		/// </summary>
		[JsonProperty("serviceadd")]
		public string 服务地址 { get; set; }

		/// <summary>
		/// 授权码:如果是通过授权码访问则使用
		/// </summary>
		[JsonProperty("authcode")]
		public string 授权码 { get; set; }

		/// <summary>
		/// 用户名:访问的用户
		/// </summary>
		[JsonProperty("account")]
		public string 用户名 { get; set; }

		/// <summary>
		/// 密码:加密后存储的访问密码
		/// </summary>
		[JsonProperty("password")]
		public string 密码 { get; set; }

		/// <summary>
		/// 授权方式:1.token,2:basic
		/// </summary>
		[JsonProperty("authtype")]
		public decimal 授权方式 { get; set; }
	}
}
