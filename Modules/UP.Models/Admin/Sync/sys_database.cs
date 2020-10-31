using System;
using System.Collections.Generic;
using System.Text;

namespace UP.Models.Admin.Sync
{
	/// <summary>
	///sys_database
	/// </summary>
	public class sys_database
	{
		/// <summary>
		/// id:guid
		/// </summary>
		public string id { get; set; }

		/// <summary>
		/// name:
		/// </summary>
		public string name { get; set; }

		/// <summary>
		/// dbtype:1：Oracle; 2：Pg;3：SqlServer;4：MySql
		/// </summary>
		public decimal dbtype { get; set; }

		/// <summary>
		/// serverip:
		/// </summary>
		public string serverip { get; set; }

		/// <summary>
		/// port:
		/// </summary>
		public decimal port { get; set; }

		/// <summary>
		/// instance:pg存储数据库名，oracle存储实例名
		/// </summary>
		public string instance { get; set; }

		/// <summary>
		/// user:
		/// </summary>
		public string user { get; set; }

		/// <summary>
		/// password:
		/// </summary>
		public string password { get; set; }

		/// <summary>
		/// describe:
		/// </summary>
		public string describe { get; set; }

	}
}
