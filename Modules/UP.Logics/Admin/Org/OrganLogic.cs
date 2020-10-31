/****************************************************
* 功能：机构管理业务层
* 描述：
* 作者：胡家源
* 日期：2020/09/17 9:04:03
*********************************************************/

using QWPlatform.DataIntface.Builders;
using QWPlatform.IService;
using QWPlatform.SystemLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using UP.Basics;
using UP.Models.Admin.Org;
using UP.Models.DB.BasicData;

namespace UP.Logics.Admin.Org
{
    public class OrganLogic
    {
        /// <summary>
        /// 分页查询机构列表
        /// </summary>
        /// <param name="pageNum">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="key">条件查询关键字</param>
        /// <returns></returns>
        public ListPageModel<OrganDto> GetOrganPageList(int pageNum, int pageSize, string keyword)
        {
            ListPageModel<OrganDto> item = null;
            try
            {
                var param = new List<string>();
                using (var db = new DbContext())
                {
                    var sqlBuilder = db.Sql("");
                    //查询条件不为空
                    if (keyword.IsNotNullOrEmpty())
                    {
                        param.Add("条件查询");
                        sqlBuilder.Parameters("keyword", keyword);
                    }
                    //获取机构列表信息
                    var sqlStr = db.GetSql("CA00001-分页查询机构列表", null, param.ToArray());
                    //执行SQL脚本
                    var items = sqlBuilder.SqlText(sqlStr).Paging(pageNum, pageSize).GetModelList<OrganDto>(out int total);
                    item = new ListPageModel<OrganDto>()
                    {
                        Total = total,
                        PageList = items
                    };
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("获取机构异常错误!", ex);
            }
            return item;
        }

        /// <summary>
        /// 根据上级机构id查询
        /// </summary>
        /// <param name="pageNum">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="key">条件查询关键字</param>
        /// <param name="pid">上级id</param>
        /// <returns></returns>
        public List<OrganDto> GetOrganListByPid(int pid)
        {
            List<OrganDto> item = null;
            try
            {
                var param = new List<string>();
                using (var db = new DbContext())
                {
                    var sqlBuilder = db.Sql("");
                    //获取机构列表
                    var sqlStr = db.GetSql("CA00001-分页查询机构列表", null, param.ToArray());
                    //执行SQL脚本
                    item = sqlBuilder.SqlText(sqlStr).GetModelList<OrganDto>(out int total);
                    item = item.Select(x => new OrganDto
                    {
                        haveChild = x.数量 > 0 ? true : false,
                        名称 = x.名称,
                        简称 = x.简称,
                        电话 = x.电话,
                        编码 = x.编码,
                        地址 = x.地址,
                        id = x.id,
                        上级id = x.上级id,
                        上级机构名称 = x.上级机构名称,
                    }).ToList();
                    item = item.Where(d => d.上级id == pid).OrderBy(d => d.id).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("获取机构异常错误!", ex);
            }
            return item;
        }

        /// <summary>
        /// 查询所有机构
        /// </summary>
        /// <param name="keyword">条件查询关键字</param>
        /// <returns></returns>
        public List<Organ> GetOrganList(string keyword)
        {
            List<Organ> item = null;
            try
            {
                var param = new List<string>();
                using (var db = new DbContext())
                {
                    var sqlBuilder = db.Sql("");
                    //查询条件不为空
                    if (keyword.IsNotNullOrEmpty())
                    {
                        param.Add("条件查询");
                        sqlBuilder.Parameters("keyword", keyword);
                    }
                    var sqlStr = db.GetSql("CA00001-分页查询机构列表", null, param.ToArray());
                    //执行SQL脚本
                    item = sqlBuilder.SqlText(sqlStr).GetModelList<Organ>().OrderBy(d=>d.id).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("获取机构异常错误!", ex);
            }
            return item;
        }



        /// <summary>
        /// 根据机构id获取机构信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OrganDto GetOrganDtoInfo(Organ orginfo,string pName)
        {
            OrganDto model = new OrganDto();
            try
            {
                model.id = orginfo.id;
                model.上级id = orginfo.上级id;
                model.名称 = orginfo.名称;
                model.地址 = orginfo.地址;
                model.性质 = orginfo.性质;
                model.执业证代码 = orginfo.执业证代码;
                model.机构码 = orginfo.机构码;
                model.电话 = orginfo.电话;
                model.简码 = orginfo.简码;
                model.简称 = orginfo.简称;
                model.组织机构代码证 = orginfo.组织机构代码证;
                model.编码 = orginfo.编码;
                model.行政级别 = orginfo.行政级别;
                model.上级机构名称 = pName;
                //机构行政区划信息
                if (!string.IsNullOrEmpty(orginfo.所属行政区划))
                {
                    using (var db = new DbContext())
                    {
                        //根据编码查询行政区划
                        var sqlStr = db.GetSql("CA00004-根据编码查询行政区划", null, null);
                        ISqlBuilder sqlBuilder = db.Sql(sqlStr).Parameters("bianma", orginfo.所属行政区划);
                        //执行SQL脚本
                        var item = sqlBuilder.GetModel<OrgArea>();
                        //组装行政区划市区县数据，判断性质
                        if (item != null && item.level > 0)
                        {
                            var Areadata = item.code;
                            GetAreaData(ref model, item.level, item.code, ref Areadata, item.bmcode);
                            //根据性质反向查询
                            for (int i = 1; i <= item.level; i++)
                            {
                                if (Areadata == null)
                                {
                                    continue;
                                }
                                var dd = new OrgArea();
                                if (item.level - i >= 0)
                                {
                                    sqlStr = db.GetSql("CA00004-根据编码查询行政区划", null, null);
                                    sqlBuilder = db.Sql(sqlStr).Parameters("bianma", Areadata);
                                    //执行SQL脚本
                                    dd = sqlBuilder.GetModel<OrgArea>();
                                    GetAreaData(ref model, dd.level, dd.code, ref Areadata, dd.bmcode);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("根据机构id获取机构信息错误：!", ex);
            }
            return model;
        }

        /// <summary>
        /// 组装行政区划市区县数据
        /// </summary>
        /// <param name="organinfo"></param>
        /// <param name="level"></param>
        /// <param name="code"></param>
        /// <param name="Areadata"></param>
        /// <param name="bmcode"></param>
        private void GetAreaData(ref OrganDto organinfo, int level, string code, ref string Areadata, string bmcode)
        {
            if (level == 4)
            {
                organinfo.所属居委会 = bmcode;
            }
            else if (level == 3)
            {
                organinfo.所属街道 = bmcode;
            }
            else if (level == 2)
            {
                organinfo.所属区县 = bmcode;
            }
            else if (level == 1)
            {
                organinfo.所属市 = bmcode;
            }
            else
            {
                organinfo.所属省 = bmcode;
            }
            Areadata = code;
        }
    }
}
