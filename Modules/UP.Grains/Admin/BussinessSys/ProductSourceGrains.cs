/****************************************************
* 功能：系统_产品服务源业务层
* 描述：
* 作者：胡家源
* 日期：2020/09/21 9:04:03
*********************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;
using UP.Basics;
using UP.Interface.Admin.BussinessSys;
using UP.Interface.Admin.Modules;
using UP.Interface.Admin.Org;
using UP.Logics.Admin.BussinessSys;
using UP.Logics.Admin.Modules;
using UP.Logics.Admin.Org;
using UP.Models.Admin.BusinessSys;
using UP.Models.Admin.Org;

namespace UP.Grains.Admin.BussinessSys
{
    public class ProductSourceGrains : BasicGrains<ProductSourceLogic>, IProductSource
    {
        /// <summary>
        /// 分页获取系统_产品服务源
        /// </summary>
        /// <param name="pageNum">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="key">查询条件</param>
        /// <param name="productid">产品id</param>
        /// <returns></returns>
        public Task<ListPageModel<ProductSourceDto>> GetProductSourceList(int pageNum, int pageSize, string keyword,int productid)
        {
            return Task.FromResult(this.Logic.GetProductSourceList(pageNum, pageSize, keyword, productid));
        }
    }
}
