using System;

namespace UP.Basics
{
    /// <summary>
    /// 实现系统控制器接口加解密的特性说明
    /// </summary>
    public class UPEncryptionAttribute : Attribute
    {
        /// <summary>
        ///输入参数是否加密
        /// </summary>
        public bool IsInEncryption { get; set; }

        /// <summary>
        /// 输出参数是否加密
        /// </summary>
        public bool IsOutEncryption { get; set; }

        /// <summary>
        /// 参数加密描述
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// 控制器接口加解密描述
        /// </summary>
        /// <param name="Description">参数加密描述</param>
        /// <param name="IsInEncryption">输入参数是否加密</param>
        /// <param name="IsOutEncryption">输出参数是否加密</param>
        public UPEncryptionAttribute(string Description = "", bool IsInEncryption = true, bool IsOutEncryption = true)
        {
            this.IsInEncryption = IsInEncryption;
            this.IsOutEncryption = IsOutEncryption;
            this.Description = Description;
        }
    }
}