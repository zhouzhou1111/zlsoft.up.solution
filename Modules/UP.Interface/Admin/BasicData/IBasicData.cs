using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UP.Basics;
using UP.Models.Admin.BasicData;
using UP.Models.Api;

namespace UP.Interface.Admin.BasicData
{
    public interface IBasicData : IBasic
    {   
        /// <summary>
         ///  新增或修改数据字典信息,只包含（编码,名称,简码字段的表）
         /// </summary>
         /// <param name="type">表名枚举类型</param>
         /// <param name="code">编码</param>
         /// <param name="name">名称</param>
         /// <param name="scode">简码</param>
         /// <param name="oldcode">旧编码</param>
         /// <returns></returns>
        Task<ResponseModel> UpdateDicItems(int type, string code, string name, string scode, string oldcode,int typeData);

        /// <summary>
        /// 分页获取基础数据
        /// </summary>
        /// <param name="pageNum">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="keyword">查询条件</param>
        /// <param name="type">基础数据类型</param>
        /// <returns></returns>
        Task<ResponseModel> GetBasicDataList(int pageNum, int pageSize, string keyword,int type);

        /// <summary>
        /// 获取基础数据信息
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="type">基础数据类型</param>
        /// <returns></returns>
        Task<ResponseModel> GetBasicDataInfo(string code, int type);

        /// <summary>
        /// 删除基础数据信息
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="type">基础数据类型</param>
        /// <returns></returns>
        Task<ResponseModel> DelBasicData(string code, int type);
    }
}
