using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UP.Models.DB.Interface;

namespace UP.Web.Models.Admin.Interface
{
    /// <summary>
    ///
    /// </summary>
    public class InterfaceItemEditParam
    {
        /// <summary>
        /// GUID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 分类id
        /// </summary>
        public string CatgoryId { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 接口名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述描述
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        /// 1:post,2:get,3:put,4:delete
        /// </summary>
        public int AccessType { get; set; }

        /// <summary>
        /// 1:异常队列 2: 同步执行
        /// </summary>
        public int ProcType { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int Sno { get; set; }

        /// <summary>
        /// 转换为数据库实体类
        /// </summary>
        /// <returns></returns>
        public InterfaceItem ConvertToDBModel()
        {
            return new InterfaceItem
            {
                id = Id,
                code = Code,
                name = Name,
                describe = Describe,
                sno = Sno,
                catgory_id = CatgoryId,
                access_type = AccessType,
                proc_type = ProcType
            };
        }
    }
}