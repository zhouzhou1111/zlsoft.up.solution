using MySqlX.XDevAPI.Common;
using QWPlatform.IService;
using System;
using System.Collections.Generic;
using System.Text;
using UP.Models.Drug;

namespace UP.Logics.Drug
{

    public class InventoryLogic
    {

        //初始化信息
        InventoryLogic inventoryLogic = new InventoryLogic();
        Inventory inventory = new Inventory();



        /// <summary>
        /// 获取库存信息。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Inventory getInventoryByID(int id)
        {
            try
            {
                using (var db = new DbContext())
                {
                    var model = db.Select("Inventory").Columns("*").Where("\"DrugID\"", id).GetModel<Inventory>();
                    return model;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }


        //初始化对象
        DrugLogic drugLogic = new DrugLogic();
        DrugBasic drugBasic = new DrugBasic();

        /// <summary>
        /// 药品入库
        /// </summary>
        /// <param name="iSche">入库信息</param>
        /// <returns></returns>
        public string inHouse(InventorySchedule iSche)
        {
            string rs = "入库失败";
            if (iSche.Count < 0)
            {
                rs = "数量不能小于0";

            }
            inventory = inventoryLogic.getInventoryByID(iSche.DrugID);

            if(inventory is null)
            {
                rs = "药品ID："+iSche+"不存在";
            }

            //插入库存sql
            string sqlInv = $"INSERT INTO \"Inventory\"( \"DrugID\", \"Amount\", \"UpdateTime\",\"Operator\", \"StroehouseID\") " +
                $"VALUES ('{iSche.DrugID}','{iSche.Count}',now(),'{iSche.Operator}','{iSche.Storehouse}')";
            //修改库存sql
            string sqlUpdate = $"UPDATE \"public\".\"Inventory\" SET \"Amount\" = '{iSche.Count}', \"UpdateTime\" = now()  WHERE \"DrugID\" " +
                $"= '{iSche.DrugID}'";
            //插入库存明细sql
            string sqlInvSchedule = $"INSERT INTO \"InventorySchedule\"( \"DrugID\", \"Count\",\"Operator\", \"OperatorTime\") " +
                $"VALUES ('{iSche.DrugID}','{iSche.Count}','{iSche.Operator}',now())";


             
            using (var db = new QWPlatform.IService.DbContext(true))
            {
                //插入数据明细
                try
                {
                    int j = db.Sql(sqlInvSchedule).Execute();
                    if (inventory.Amount==0)
                    {
                        //如果第一次入库，新增库存
                        int i = db.Sql(sqlInv).
                             Execute(); 
                    }

                    //如果非第一次入库，修改库存
                    int k = db.Sql(sqlUpdate).Execute();
                    if (k < 1)
                    {
                        rs = "入库失败";
                    }

                }
                catch (Exception ex)
                {
                    rs = ex.ToString();
                }

            }
            return rs;

        }

        /// <summary>
        /// 出库
        /// </summary>
        /// <param name="inventorySchedule">出库明细</param>
        /// <returns></returns>
        public bool outHouse(InventorySchedule inventorySchedule)
        {
            bool rs = false;
            inventory.Amount = inventoryLogic.getInventoryByID(inventorySchedule.DrugID).Amount;
            if (inventorySchedule.Count >inventory.Amount)
            {
                return rs;

            }
            //修改数量
            inventory.Amount -= inventorySchedule.Count;

             //修改库存sql
            string sqlUpdate = $"UPDATE \"public\".\"Inventory\" SET \"Amount\" = '{inventory.Amount }', \"UpdateTime\" = now()  WHERE \"DrugID\" " +
                $"= '{inventorySchedule.DrugID}'";
            //插入库存明细sql
            string sqlInvSchedule = $"INSERT INTO \"InventorySchedule\"( \"DrugID\", \"Count\",\"Operator\", \"OperatorTime\") " +
                $"VALUES ('{inventorySchedule.DrugID}','{-inventorySchedule.Count}','{inventorySchedule.Operator}',now())";

            try
            {
                using(var db = new QWPlatform.IService.DbContext(true))
                {
                    int j = db.Sql(sqlInvSchedule).Execute();
                    if (j > 0)
                    {
                        int i = db.Sql(sqlUpdate).Execute();                        
                    }

                }
            }
            catch (Exception ex)
            {
                
            }
            return rs;

        }

    }
}
