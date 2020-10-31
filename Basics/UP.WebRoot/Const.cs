using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UP.WebRoot
{
    /// <summary>
    /// 分支版本的常量设置
    /// </summary>
    public class Const
    {
        /// <summary> 
        /// 加密密钥
        /// </summary>
        public const string SecurityKey = "+SitIeJSWtLJU8/Wz2m7gStexajkeD+Lka6DSTy8gt9UwfgVQo6uKjVLG5Ex7PiGOODVqAEghBuS7JzIYU5RvI543nNDAPfnJsas96mSA7L/mD7RTE2drj6hf3oZjJpMPZUQI/B1Qjb5H3K3PNwIDAQAB";

        /// <summary>
        /// 站点地址
        /// </summary>
        public const string Domain = "https://localhost:10010";

        /// <summary>
        /// 受理人，之所以弄成可变的是为了用接口动态更改这个值以模拟强制Token失效
        /// 真实业务场景可以在数据库或者redis存一个和用户id相关的值，生成token和验证token的时候获取到持久化的值去校验
        /// 如果重新登陆，则刷新这个值
        /// </summary>
        public static string ValidAudience;
    }
}
