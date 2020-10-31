using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UP.Basics;
using UP.Interface.Admin.BasicData;
using UP.Logics.Admin.BasicData;
using UP.Models.Admin.BasicData;
using UP.Models.Api;

namespace UP.Grains.Admin.BasicData
{
    public class BasicDataGrains : BasicGrains<BasicDataLogic>, IBasicData
    {

        /// <summary>
        /// 删除基础数据信息
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="type">基础数据类型</param>
        /// <returns></returns>
        public Task<ResponseModel> DelBasicData(string code, int type)
        {
            return Task.FromResult(this.Logic.DelBasicData(code, type));
        }

        /// <summary>
        /// 获取基础数据信息
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="type">基础数据类型</param>
        /// <returns></returns>
        public Task<ResponseModel> GetBasicDataInfo(string code, int type)
        {
            return Task.FromResult(this.Logic.GetBasicDataInfo(code, type));
        }


        /// <summary>
        /// 分页获取基础数据
        /// </summary>
        /// <param name="pageNum">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="keyword">查询条件</param>
        /// <param name="type">基础数据类型</param>
        /// <returns></returns>
        public Task<ResponseModel> GetBasicDataList(int pageNum, int pageSize, string keyword, int type)
        {
            return Task.FromResult(this.Logic.GetBasicDataList(pageNum, pageSize, keyword, type));
        }

        /// <summary>
        ///  新增或修改数据字典信息,只包含（编码,名称,简码字段的表）
        /// </summary>
        /// <param name="type">表名枚举类型</param>
        /// <param name="code">编码</param>
        /// <param name="name">名称</param>
        /// <param name="scode">简码</param>
        /// <param name="oldcode">旧编码</param>
        /// <returns></returns>
        public Task<ResponseModel> UpdateDicItems(int type, string code, string name, string scode, string oldcode,int typeData)
        {
            return Task.FromResult(this.Logic.UpdateDicItems(type, code, name, scode, oldcode, typeData));
        }
    }
}
