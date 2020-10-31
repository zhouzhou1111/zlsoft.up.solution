using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UP.Basics;
using UP.Basics.Models;
using UP.Models.Api;

namespace UP.Interface.Api
{
    /// <summary>
    /// 公共业务块接口
    /// </summary>
    public interface IPublic : IBasic
    {
        /// <summary>
        /// 查询数据字典信息
        /// </summary>
        /// <returns></returns>
        Task<JsonMsg<List<DicInfo>>> GetDicItems(DicParam entity);

 
    }
}
