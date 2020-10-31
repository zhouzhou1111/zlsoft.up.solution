using QWPlatform.IService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using UP.Models.Admin.Sync;
using UP.Models.Drug;

namespace UP.Logics.Drug
{

    public class DrugLogic
    {
       

        /// <summary>
        /// 获取药品列表
        /// </summary>
        /// <returns>返回药品列表</returns>
        public Task<List<DrugBasic>> getDrugList(string searchChar)
        {
            //如果查询code为空，则返回所有的药品信息
            if (string.IsNullOrEmpty(searchChar))
            {
                using (var db = new DbContext())
                {
                   var model = db.Select("drug").Columns("*").GetModelList<DrugBasic>();
                    //var model = db.Sql("select * from drug ").Execute();
                    return Task.FromResult(model);
                }
            }
            //如果查询code不为空，模糊查询
            else
            {
                using (var db = new DbContext())
                {
                    var model = db.Select("drug").Columns("*").Where("code", searchChar).GetModelList<DrugBasic>();
                    return Task.FromResult(model);
                }
            }

        }
        /// <summary>
        /// 用id查询药品列表
        /// </summary>
        /// <param name="id">药品ID</param>
        /// <returns></returns>
        public Task<DrugBasic> getDrugById(int id)
        {
            try
            {
                using (var db = new DbContext())
                {
                    var model = db.Select("drug").Columns("*").Where("\"id\"", id).GetModel<DrugBasic>();
                    return Task.FromResult(model);
                }
            }
            catch(Exception ex)
            {
                return null;
            }
            
        }
        /// <summary>
        /// 用药品名称查询药品列表
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Task<List<DrugBasic>> getDrugByName(string name)
        {
            using (var db = new DbContext())
            {
                var model = db.Select("drug").Columns("*").Where("\"name\"", name).GetModelList<DrugBasic>();
                return Task.FromResult(model);
            }
        }

        /// <summary>
        /// 新增药品
        /// </summary>
        /// <param name="drugBasic">新增药品</param>
        /// <returns></returns>
        public Task<int>  DrugAdd(DrugBasic drugBasic)
        {
            int rs = 1;
            
            try
            {
                using (var db = new DbContext(true))
                {
                    db.Insert("drug")
                  .Column("name", drugBasic.Name)
                  .Column("code", drugBasic.Code)
                  .Column("englishname", drugBasic.EnglishName)
                  .Column("dosageformid", drugBasic.DosageFormID)
                  .Column("manufacturerid", drugBasic.ManufacturerID)
                  .Execute();
                }
                rs = 1;
            }
            
            catch(Exception ex)
            {
                Console.WriteLine("新增药品执行失败：" + ex);
                rs = 0;
            }
            
            return Task.FromResult(rs);
           

        }

        /// <summary>
        /// 编辑药品
        /// </summary>
        /// <param name="drugBasic">新增药品</param>
        /// <returns></returns>
        public Task<DrugBasic> EditAdd(DrugBasic drugBasic)
        {
            int rs = 1;
            DrugBasic drugBasic1 = new DrugBasic();
            try
            {
                using (var db = new DbContext(true))
                {
                    db.Update("drug")
                  .Column("name", drugBasic.Name)
                  .Column("code", drugBasic.Code)
                  .Column("englishname", drugBasic.EnglishName)
                  .Column("dosageformid", drugBasic.DosageFormID)
                  .Column("manufacturerid", drugBasic.ManufacturerID)
                  .Execute();
                }
                rs = 1;
            }

            catch (Exception ex)
            {
               Console.WriteLine("修改药品执行失败："+ex) ;
            }
          
            return Task.FromResult(drugBasic1);
        }

    }
}


