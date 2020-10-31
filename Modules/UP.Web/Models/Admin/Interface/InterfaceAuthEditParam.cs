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
    public class InterfaceAuthEditParam
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
        /// 产品ID
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// 每分钟调用次数
        /// </summary>
        public int FrequencyInterval { get; set; }

        /// <summary>
        ///可调用时间范围
        /// </summary>
        public DateTime Efctime { get; set; }

        /// <summary>
        ///失效的时间
        /// </summary>
        public DateTime Expitdate { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        /// 转换为DB model
        /// </summary>
        /// <returns></returns>
        public InterfaceAuth ConvertToDBModel()
        {
            return new InterfaceAuth
            {
                id = Id,
                items_id = ItemsId,
                product_id = ProductId,
                describe = Describe,
                efctime = Efctime,
                expitdate = Expitdate,
                frequency_interval = FrequencyInterval
            };
        }
    }
}