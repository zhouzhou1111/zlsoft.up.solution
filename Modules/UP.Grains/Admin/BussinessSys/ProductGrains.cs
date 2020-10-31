/****************************************************
* 功能：系统_产品业务层
* 描述：
* 作者：胡家源
* 日期：2020/09/21 9:04:03
*********************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;
using UP.Basics;
using UP.Interface.Admin.BussinessSys;
using UP.Logics.Admin.BussinessSys;

namespace UP.Grains.Admin.BussinessSys
{
    public class ProductGrains : BasicGrains<ProductLogic>, IProduct
    {
        /// <summary>
        /// 分页查询产品列表
        /// </summary>
        /// <param name="pageNum">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="key">查询条件</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public Task<ListPageModel<Models.DB.BusinessSys.Product>> GetProductList(int pageNum, int pageSize, string keyword,int state)
        {
            return Task.FromResult(this.Logic.GetProductList(pageNum, pageSize, keyword,state));
        }
    }
}
