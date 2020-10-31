using System;
using System.Collections.Generic;
using System.Text;

namespace UP.Models.Admin.Sync
{
	/// <summary>
	///sys_sync_table
	/// </summary>
	public class sys_sync_table
	{
		/// <summary>
		/// id:
		/// </summary>
		public string id { get; set; }

		/// <summary>
		/// database_id:
		/// </summary>
		public string database_id { get; set; }

		/// <summary>
		/// table_name:
		/// </summary>
		public string table_name { get; set; }

		/// <summary>
		/// sno:
		/// </summary>
		public decimal sno { get; set; }

		/// <summary>
		/// 是否同步中，0:否，1：是
		/// </summary>
        public int is_sync { get; set; }

    }
}
