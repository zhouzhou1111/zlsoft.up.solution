/****************************************************
* 功能：系统_产品接口层
* 描述：
* 作者：胡家源
* 日期：2020/09/21 9:04:03
*********************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;
using UP.Basics;
using UP.Models.Admin.Org;
using UP.Models.DB.BusinessSys;

namespace UP.Interface.Admin.BussinessSys
{
    public  interface IProduct : IBasic
    {
        /// <summary>
        /// 分页查询产品列表
        /// </summary>
        /// <param name="pageNum">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="key">查询条件</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        Task<ListPageModel<Product>> GetProductList(int pageNum, int pageSize, string keyword,int state);
    }
}
