using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UP.Basics;

namespace UP.WebRoot
{
    /// <summary>
    /// 生成Token的jwt实现类
    /// </summary>
    public class JwtToken : IJwt
    {
        //加密后的安全码
        private string _base64Secret;

        //jwt的配置信息对象
        private JwtConfigModel jwtconfigModel = new JwtConfigModel();

        /// <summary>
        /// 获取可以跳过的url
        /// </summary>
        public List<string> IgnoreUrls { get => jwtconfigModel.IgnoreUrls; }

        /// <summary>
        /// 当前登录人员的账户
        /// </summary>
        public string LoginAccount { get; set; }

        /// <summary>
        /// 尝试登录的次数
        /// </summary>
        public int TryCount { get => jwtconfigModel.TryLoginCount; }

        /// <summary>
        /// 初始化时注入configration
        /// </summary>
        /// <param name="configration">注入的对象</param>
        public JwtToken(IConfiguration configration)
        {
            //读取配置文件
            configration.GetSection("Jwt").Bind(jwtconfigModel);
            if (jwtconfigModel.Lifetime == 0)
            {//默认设置为120分钟
                jwtconfigModel.Lifetime = 120;
            }

            if (string.IsNullOrEmpty(jwtconfigModel.SecretKey))
            {//设置默认的key
                jwtconfigModel.SecretKey = Const.SecurityKey;
            }

            if (jwtconfigModel.IgnoreUrls?.Count == 0)
            {
                //没有默认可访问的url，添加默认的访问url
                jwtconfigModel.IgnoreUrls = new List<string>();
                jwtconfigModel.IgnoreUrls.AddRange(new string[]
                {
                    "/",
                    "/api/Login/Login",
                    "/api/login/login"
                });
            }

            //获取加密信息
            GetSecret();
        }

        /// <summary>
        /// 获取到加密串
        /// </summary>
        private void GetSecret()
        {
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(jwtconfigModel.SecretKey);
            byte[] messageBytes = encoding.GetBytes(jwtconfigModel.SecretKey);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                this._base64Secret = Convert.ToBase64String(hashmessage);
            }
        }

        /// <summary>
        /// 获取一个新生成的token
        /// </summary>
        /// <param name="Clims"></param>
        /// <returns></returns>
        public string GetToken(Dictionary<string, string> Clims)
        {
            List<Claim> claimsAll = new List<Claim>();
            foreach (var item in Clims)
            {
                claimsAll.Add(new Claim(item.Key, item.Value ?? ""));
            }
            var symmetricKey = Convert.FromBase64String(_base64Secret);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "Issuer",
                Audience = "Audience",
                Subject = new ClaimsIdentity(claimsAll),
                NotBefore = DateTime.Now,
                Expires = DateTime.Now.AddMinutes(jwtconfigModel.Lifetime), //颁发的令牌2小时内有效
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey),
                                           SecurityAlgorithms.HmacSha256Signature)
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);
        }

        /// <summary>
        /// 验证token是否有效
        /// </summary>
        /// <param name="token"></param>
        /// <param name="Clims"></param>
        /// <returns></returns>
        public bool ValidateToken(string token, out Dictionary<string, string> Clims)
        {
            Clims = new Dictionary<string, string>();
            ClaimsPrincipal principal = null;
            if (string.IsNullOrWhiteSpace(token))
            {
                return false;
            }
            var handler = new JwtSecurityTokenHandler();
            try
            {
                var jwt = handler.ReadJwtToken(token);

                if (jwt == null)
                {
                    return false;
                }

                //解密内容
                var secretBytes = Convert.FromBase64String(_base64Secret);
                var validationParameters = new TokenValidationParameters
                {
                    RequireExpirationTime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretBytes),
                    ClockSkew = TimeSpan.FromMinutes(5),
                    ValidateIssuer = true,//是否验证Issuer
                    ValidateAudience = true,//是否验证Audience
                    ValidateLifetime = true,//是否验证失效时间
                    ValidateIssuerSigningKey = true,//是否验证SecurityKey
                    ValidAudience = "Audience",
                    ValidIssuer = "Issuer"
                };
                SecurityToken securityToken;
                principal = handler.ValidateToken(token, validationParameters, out securityToken);
                foreach (var item in principal.Claims)
                {
                    Clims.Add(item.Type, item.Value);
                    if (item.Type == "userName")
                    {//如果是登录账户则记录当前登录人员
                        this.LoginAccount = item.Value;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {//验证失败
                return false;
            }
        }
    }
}