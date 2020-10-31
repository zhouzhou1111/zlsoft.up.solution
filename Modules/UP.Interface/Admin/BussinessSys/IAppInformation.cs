using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UP.Basics;
using UP.Models.Admin.BusinessSys;

namespace UP.Interface.Admin.BussinessSys
{
   public interface IAppInformation : IBasic
    {
        /// <summary>
        /// 分页获取系统_应用信息
        /// </summary>
        /// <param name="pageNum">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="key">查询条件</param>
        /// <param name="productid">产品id</param>
        /// <returns></returns>
        Task<ListPageModel<AppInformationDto>> GetAppInformationList(int pageNum, int pageSize, string keyword, int productid);
    }
}
