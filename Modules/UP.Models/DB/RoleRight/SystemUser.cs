using Newtonsoft.Json;
using QWPlatform.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace UP.Models.DB.RoleRight
{
	/// <summary>
	///系统_人员
	/// </summary>
	[Table("系统_人员")]
	[DataContract]
	public class SystemUser
    {
		[PrimaryKey(true)]
		[DataMember]
		public int id { get; set; }


		[JsonProperty("name")]
		public string 姓名 { get; set; }

		[JsonProperty("sortcode")]
		public string 简码 { get; set; }

		
		[JsonProperty("sex")]
		public string 性别 { get; set; }


		[JsonProperty("phone")]
		public string 手机 { get; set; }


		[JsonProperty("email")]
		public string 邮箱 { get; set; }


		[JsonProperty("workname")]
		public string 工作单位 { get; set; }

		/// <summary>
		/// 用户状态:1、启用 0 停用
		/// </summary>
		[JsonProperty("state")]
		public int 用户状态 { get; set; }


		[JsonProperty("account")]
		public string 账号 { get; set; }


		[JsonProperty("password")]
		public string 密码 { get; set; }

		/// <summary>
		/// face_token: 指纹认证token
		/// </summary>
		[DataMember]
		public string face_token { get; set; }

		/// <summary>
		/// 数据标识:1:正常； 0:删除
		/// </summary>
		[JsonProperty("isval")]
		public int 数据标识 { get; set; }

		[JsonProperty("head_portrait")]
		public string 照片 { get; set; }


		[JsonProperty("addtime")]
		public DateTime 登记时间 { get; set; }


		[JsonProperty("edit_time")]
		public DateTime 编辑时间 { get; set; }

	}
}
