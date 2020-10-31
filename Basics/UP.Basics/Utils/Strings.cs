using Microsoft.International.Converters.PinYinConverter;

namespace UP.Basics.Utils
{
    public static class Strings
    {
        /// <summary>
        /// 获取该字符串的拼音首字母
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetFirstPY(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                string r = string.Empty;
                foreach (char obj in str)
                {
                    try
                    {
                        ChineseChar chineseChar = new ChineseChar(obj);
                        if (chineseChar.Pinyins.Count > 0)
                        {
                            string t = chineseChar.Pinyins[0].ToString();
                            if (t.Length > 0)
                            {
                                r += t.Substring(0, 1);
                            }
                        }
                    }
                    catch
                    {
                        r += obj.ToString();
                    }
                }

                return r;
            }

            return string.Empty;
        }
    }
}