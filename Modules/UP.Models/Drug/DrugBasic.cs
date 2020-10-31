using System;
using System.Collections.Generic;
using System.Text;

namespace UP.Models.Drug
{
     public  class DrugBasic
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
