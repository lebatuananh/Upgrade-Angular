using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DVG.WIS.Core
{
    public class JWTHelper
    {
        private static readonly object LockObject = new object();
        private static JWTHelper _instance;

        public static JWTHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (LockObject)
                    {
                        if (_instance == null)
                            _instance = new JWTHelper();
                    }
                }

                return _instance;
            }
        }

        public string CreateToken(int userId, string username, string fullName, string checksumKey, System.DateTime expires, string roles)
        {
            //Set issued at date
            System.DateTime issuedAt = System.DateTime.UtcNow;

            //http://stackoverflow.com/questions/18223868/how-to-encrypt-jwt-security-token
            var tokenHandler = new JwtSecurityTokenHandler();

            //create a identity and add claims to the user which we want to log in
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.SerialNumber, userId.ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Surname, fullName),
                new Claim(ClaimTypes.Sid, checksumKey),
                new Claim(ClaimTypes.Expired, DVG.WIS.Utilities.Utils.DateTimeToUnixTime(expires).ToString()),
                new Claim(ClaimTypes.Role, roles),
            });

            string sec = AppSettings.Instance.GetString(Const.JWTSecretKey);
            string issuer = AppSettings.Instance.GetString(Const.JWTIssuer);
            string audience = AppSettings.Instance.GetString(Const.JWTAudience);
            var now = DateTime.UtcNow;
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(sec));
            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);

            //create the jwt
            var token =
                (JwtSecurityToken)
                    tokenHandler.CreateJwtSecurityToken(issuer: issuer, audience: audience,
                        subject: claimsIdentity, notBefore: issuedAt, expires: expires, signingCredentials: signingCredentials);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        public string GenerateChecksumKey(string username)
        {
            return string.Format("{0}:{1}", username.ToLower(), Guid.NewGuid().ToString());
        }

        public class CustomizeClaimTypes
        {
            public const string Groups = "groups_role";
        }
    }
}
