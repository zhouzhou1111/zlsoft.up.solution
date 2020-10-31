using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UP.Models.DB.Interface;

namespace UP.Web.Models.Admin.Interface
{
    /// <summary>
    /// 接口分类、分组参数实体类
    /// </summary>
    public class InterfaceCatgoryParam
    {
        /// <summary>
        /// 分组ID、分类ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///分类序号
        /// </summary>
        public int Sno { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        /// 转换为DBmodel
        /// </summary>
        /// <returns></returns>
        public InterfaceCatgory ConvertToDBModel()
        {
            InterfaceCatgory model = new InterfaceCatgory()
            {
                id = this.Id,
                parent_id = this.ParentId,
                describe = this.Describe,
                code = this.Code,
                name = this.Name,
                sno = this.Sno
            };
            return model;
        }
    }
}