/****************************************************
* 功能：系统_产品数据源接口层
* 描述：
* 作者：胡家源
* 日期：2020/09/21 9:04:03
*********************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;
using UP.Basics;
using UP.Models.Admin.BusinessSys;
using UP.Models.Admin.Org;

namespace UP.Interface.Admin.BussinessSys
{
    public  interface IDataSource : IBasic
    {
        /// <summary>
        /// 分页获取系统_产品数据源
        /// </summary>
        /// <param name="pageNum">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="key">查询条件</param>
        /// <param name="productid">产品id</param>
        /// <param name="databasetype">数据库类型</param>
        /// <returns></returns>
        Task<ListPageModel<DataSourceDto>> GetDataSourceList(int pageNum, int pageSize, string keyword,int productid, int databasetype);
    }
}
