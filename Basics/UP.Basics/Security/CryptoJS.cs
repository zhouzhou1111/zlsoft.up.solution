using System;
using System.Security.Cryptography;
using System.Text;

namespace UP.Basics
{
    /*
     * 引入 js库
     * 1.http://cdn.bootcss.com/crypto-js/3.1.9/crypto-js.js
     * 
     * html前端调用方式：
     * <script>
     * 2.调用
     *   var data = "1234" // 需要加密的字符串
     *   var key_base = "12345678900000001234567890000000"; // 加密秘钥的基值 需和后端一致 
     *   var iv_base = "1234567890000000"; // 加密所需iv基值 需和后端一致
     *
     *   // 建议混淆js操作
     *   * 定义加密函数
     *   * @param  {[type]} a [形参，需要加密的值]
     *   * @return {[type]}   [加密后的值]
     *
     *   var get = function(a) {
     *          var key_hash = key_base; 
     *          var key = CryptoJS.enc.Utf8.parse(key_hash);
     *          var iv = CryptoJS.enc.Utf8.parse(iv_base);
     *          var res = CryptoJS.AES.encrypt(a, key, { iv: iv, mode: CryptoJS.mode.CBC, padding: CryptoJS.pad.Pkcs7 });
     *      
     *          return res.toString()
     *      }

        //调用
        console.log(get(data)) // tPYJv39 

    *   </script>
    */

    /// <summary>
    /// 前端JS加密，后端解密 
    /// </summary>
    public class CryptoJS
    {
        //加密向量
        const string AES_IV = "1234567890000000";//16位    

        /// <summary>
        /// CryptoJS 加密
        /// </summary>
        /// <param name="encStr"></param>
        /// <param name="key">16位</param>
        /// <param name="iv">16位</param>
        /// <returns></returns>
        /// <exception cref="T:System.OverflowException">数组是多维数组并且包含多个 <see cref="F:System.Int32.MaxValue" /> 元素。</exception>
        public static string Encrypt(string encStr, string key)
        {
            byte[] keyArray = Encoding.UTF8.GetBytes(key);
            byte[] ivArray = Encoding.UTF8.GetBytes(AES_IV);
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(encStr);
            using (var rDel = new RijndaelManaged())
            {
                rDel.Key = keyArray;
                rDel.IV = ivArray;
                rDel.Mode = CipherMode.CBC;
                rDel.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = rDel.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
        }


        /// <summary>
        /// CryptoJS 解密
        /// </summary>
        /// <param name="decryptStr">base64加密字符串</param>
        /// <param name="key">16位</param>
        /// <param name="iv">16位</param>
        /// <returns></returns>
        /// <exception cref="T:System.OverflowException">数组是多维数组并且包含多个 <see cref="F:System.Int32.MaxValue" /> 元素。</exception>
        public static string Decrypt(string decryptStr, string key)
        {
            byte[] keyArray = Encoding.UTF8.GetBytes(key);
            byte[] ivArray = Encoding.UTF8.GetBytes(AES_IV);
            byte[] toEncryptArray = Convert.FromBase64String(decryptStr);
            using (RijndaelManaged rDel = new RijndaelManaged())
            {
                rDel.Key = keyArray;
                rDel.IV = ivArray;
                rDel.Mode = CipherMode.CBC;
                rDel.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = rDel.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                return Encoding.UTF8.GetString(resultArray);
            }
        }
    }
}
