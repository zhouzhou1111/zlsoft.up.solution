using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UP.Basics
{
    /// <summary>
    /// 账号模型
    /// </summary>
    [DataContract]
    public class UserModel
    {
        /// <summary>
        /// account:登录账户
        /// </summary>
        [DataMember]
        public string account { get; set; }

        /// <summary>
        /// account_state:账户状态 -1-删除;0-停用;1-正常
        /// </summary>
        public int account_status { get; set; }

        /// <summary>
        /// id：人员id
        /// </summary>
        [DataMember]
        public string id { get; set; }

        /// <summary>
        /// name：人员姓名
        /// </summary>
        [DataMember]
        public string name { get; set; }

        /// <summary>
        /// status：人员状态 -1-删除;0-停用;1-正常
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// org_id：机构id
        /// </summary>
        [DataMember]
        public string org_id { get; set; }

        /// <summary>
        /// org_name：机构名称
        /// </summary>
        [DataMember]
        public string org_name { get; set; }

        /// <summary>
        /// tel:座机电话
        /// </summary>
        public string tel { get; set; }

        /// <summary>
        /// mobile:移动电话
        /// </summary>
        public string mobile { get; set; }

        /// <summary>
        /// token：身份令牌
        /// </summary>
        [DataMember]
        public string token { get; set; }

        /// <summary>
        /// token：身份令牌有效期(当前服务器时间戳毫秒数)
        /// </summary>
        [DataMember]
        public long token_effective_period { get; set; }

        /// <summary>
        /// 用户类型 管理员=1,医生=2,其他=3
        /// </summary>
        public int usertype { get; set; }

        /// <summary>
        /// 是否超级管理员 1是，其他否(医生时有效)
        /// </summary>
        public int is_super_admin { get; set; }

        /// <summary>
        /// 可访问的菜单集合，及菜单下的按钮集合(医生时有效)
        /// </summary>
        public List<MenuModel> menus { get; set; }

        /// <summary>
        /// 可访问的接口集合ids,（根据角色查询某个页面的接口目录,主要是指能调用访问的webapi接口，存储值使用类名,方法名）
        /// </summary>
        public List<ButtonActionModel> actions { get; set; }
    }
}