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
    public class InterfaceSyncEditParam
    {
        /// <summary>
        /// GUID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 指向的接口id
        /// </summary>
        public string ItemsId { get; set; }

        /// <summary>
        /// 排序号，也是执行顺序号
        /// </summary>
        public string Sno { get; set; }

        /// <summary>
        /// 引用的服务id
        /// </summary>
        public string ServiceId { get; set; }

        /// <summary>
        /// 消息转发调用地址或方法
        /// </summary>
        public string CallAddress { get; set; }

        /// <summary>
        /// 1:get,2:post
        /// </summary>
        public int CallType { get; set; }

        /// <summary>
        /// 1:Basic,2:token（通过业务系统进行授权）
        /// </summary>
        public int AuthType { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        /// 转换为DB Model
        /// </summary>
        /// <returns></returns>
        public InterfaceSync ConvertToDBModel()
        {
            return new InterfaceSync
            {
                id = Id,
                items_id = ItemsId,
                sno = Sno,
                service_id = ServiceId,
                call_address = CallAddress,
                call_type = CallType,
                auth_type = AuthType,
                describe = Describe,
                user_name = UserName,
                password = Password
            };
        }
    }
}