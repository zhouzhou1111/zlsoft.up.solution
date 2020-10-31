using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UP.Models.DB.Interface;

namespace UP.Web.Models.Admin.Interface
{
    /// <summary>
    ///
    /// </summary>
    public class InterfaceParamEditParam
    {
        /// <summary>
        /// GUID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 接口ID
        /// </summary>
        public string ItemsId { get; set; }

        /// <summary>
        ///参数名称，如name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 参数标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 参数类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        ///  序号
        /// </summary>
        public int Sno { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public InterfaceParam ConvertToDBModel()
        {
            return new InterfaceParam
            {
                id = Id,
                sno = Sno,
                describe = Describe,
                type = Type,
                title = Title,
                name = Name,
                items_id = ItemsId
            };
        }
    }
}