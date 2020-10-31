/****************************************************
* 功能：系统_产品数据源业务层
* 描述：
* 作者：胡家源
* 日期：2020/09/21 9:04:03
*********************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;
using UP.Basics;
using UP.Interface.Admin.BussinessSys;
using UP.Logics.Admin.BussinessSys;
using UP.Models.Admin.BusinessSys;

namespace UP.Grains.Admin.BussinessSys
{
    public class DataSourceGrains : BasicGrains<DataSourceLogic>, IDataSource
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
        public Task<ListPageModel<DataSourceDto>> GetDataSourceList(int pageNum, int pageSize, string keyword,int productid, int databasetype)
        {
            return Task.FromResult(this.Logic.GetDataSourceList(pageNum, pageSize, keyword, productid, databasetype));
        }
    }
}
