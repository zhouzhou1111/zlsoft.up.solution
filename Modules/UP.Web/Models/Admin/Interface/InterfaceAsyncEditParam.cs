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
    public class InterfaceAsyncEditParam
    {
        /// <summary>
        /// guid
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 接口id
        /// </summary>
        public string ItemsId { get; set; }

        /// <summary>
        /// MQ exchage 下拉选择一个路由器
        /// </summary>
        public string ExchageId { get; set; }

        /// <summary>
        /// 选择一个关键字
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        /// 转换为DB Model
        /// </summary>
        /// <returns></returns>
        public InterfaceAsync ConvertToDBModel()
        {
            return new InterfaceAsync
            {
                id = Id,
                items_id = ItemsId,
                describe = Describe,
                keyword = Keyword,
                exchage_id = ExchageId
            };
        }
    }
}