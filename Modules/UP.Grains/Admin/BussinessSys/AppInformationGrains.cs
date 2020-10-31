using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UP.Basics;
using UP.Interface.Admin.BussinessSys;
using UP.Logics.Admin.BussinessSys;
using UP.Models.Admin.BusinessSys;

namespace UP.Grains.Admin.BussinessSys
{
    public class AppInformationGrains : BasicGrains<AppInformationLogic>, IAppInformation
    {
        /// <summary>
        /// 分页获取系统_应用信息
        /// </summary>
        /// <param name="pageNum">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="key">查询条件</param>
        /// <param name="productid">产品id</param>
        /// <returns></returns>
        public Task<ListPageModel<AppInformationDto>> GetAppInformationList(int pageNum, int pageSize, string keyword, int productid)
        {
            return Task.FromResult(this.Logic.GetAppInformationList(pageNum, pageSize, keyword, productid));
        }
    }
}
