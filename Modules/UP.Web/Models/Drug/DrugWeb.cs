using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UP.Web.Models.Drug
{
	public class DrugWeb
	{ 
			/// <summary>
			/// ID:
			/// </summary>
			public int ID { get; set; }

			/// <summary>
			/// Name:
			/// </summary>
			public string Name { get; set; }

			/// <summary>
			/// Code:
			/// </summary>
			public string Code { get; set; }

			/// <summary>
			/// EnglishName:
			/// </summary>
			public string EnglishName { get; set; }

			/// <summary>
			/// DosageFormID:
			/// </summary>
			public int DosageFormID { get; set; }

			/// <summary>
			/// ManufacturerID:
			/// </summary>
			public int ManufacturerID { get; set; }
 
	}
}