using KYSharp.SM;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZHSZ.Basics
{
    /// <summary>
    /// SM4加密解密
    /// </summary>
    public class SM4
    {
        /// <summary>
        /// 加密KEY
        /// </summary>
        private const string KEY = "JeF8U9wHFOMfs2Y8";

        /// <summary>
        /// 加密向量
        /// </summary>
        private const string IV = "UISwD9fW6cFh9SNS";

        //sm4加密对象
        private static SM4Utils _sm4 = new SM4Utils();

        static SM4()
        {
            //设置加密的key
            _sm4.secretKey = KEY;
            _sm4.iv = IV;
            _sm4.hexString = false;
        }

        /// <summary>
        /// 加密内容_ECB模式
        /// </summary>
        /// <param name="text">要加密的文本</param>
        /// <returns>返回加密的文本</returns>
        public static string Encrypt_ECB(string text)
        {
            return _sm4.Encrypt_ECB(text);
        }

        /// <summary>
        /// 内容解密(ECB)
        /// </summary>
        /// <param name="text">密文</param>
        /// <returns>返回解密后的内容</returns>
        public static string Decrypt_ECB(string text)
        {
            return _sm4.Decrypt_ECB(text);
        }

        /// <summary>
        /// CBC加密
        /// </summary>
        /// <param name="text">密文</param>
        /// <returns></returns>
        public static string Encrypt_CBC(string text)
        {
            return _sm4.Encrypt_CBC(text);
        }

        /// <summary>
        /// CBC解密
        /// </summary>
        /// <param name="text">密文</param>
        /// <returns>返回解密后的结果</returns>
        public static string Decrypt_CBC(string text)
        {
            return _sm4.Decrypt_CBC(text);
        }
    }
}
