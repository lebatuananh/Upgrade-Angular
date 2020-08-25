using DVG.WIS.Business.Authenticator;
using DVG.WIS.Core;
using DVG.WIS.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DVG.CK.OMSApi.Middleware
{
    public class RetrieveUserMiddleware : BaseCustomMiddleware
    {
        private IUserService userService;
        public RetrieveUserMiddleware(RequestDelegate next) : base(next)
        {
            userService = new UserService();
        }

        public override async Task InvokeAsync(HttpContext context)
        {
            // các request không cần validate
            if (context.Request.Path.Value.ToLower() == "/api/account/dologin")
                await next(context);

            var token = string.Empty;
            if (!TryRetrieveToken(context.Request, out token))
            {
                context.Response.StatusCode = HttpStatusCode.Unauthorized.GetHashCode();
                //allow requests with no token - whether a action method needs an authentication can be set with the claimsauthorization attribute
                await context.Response.WriteAsync("Jwt token không hợp lệ!");
                return;
            }

            try
            {
                string sec = AppSettings.Instance.GetString(Const.JWTSecretKey);
                string issuer = AppSettings.Instance.GetString(Const.JWTIssuer);
                string audience = AppSettings.Instance.GetString(Const.JWTAudience);
                var now = DateTime.UtcNow;
                var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(sec));


                SecurityToken securityToken;
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

                // Đối tượng chưa check expire để lấy thông tin 
                TokenValidationParameters validationParameters = new TokenValidationParameters()
                {
                    ValidAudience = audience,
                    ValidIssuer = issuer,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    LifetimeValidator = this.LifetimeValidator,
                    IssuerSigningKey = securityKey,
                };

                // lấy checksumKey
                //IdentityModelEventSource.ShowPII = true;
                var currentPrincipal = handler.ValidateToken(token, validationParameters, out securityToken);
                if (currentPrincipal != null)
                {
                    var checksumObj = currentPrincipal.Claims.Where(x => x.Type == ClaimTypes.Sid).FirstOrDefault();
                    if (checksumObj != null)
                    {
                        var checksumKey = checksumObj.Value;
                        if (!string.IsNullOrEmpty(checksumKey))
                        {
                            // kiểm tra trong cache có tồn tài checksum không
                            if (userService.ChecksumJWTOnCache(checksumKey))
                            {
                                // kiểm tra xem token có tồn tại trong cache không
                                if (userService.CheckExitstJWTTokenOnCache(checksumKey, token))
                                {
                                    // Kiểm tra xem token đã bị expire chưa
                                    var expiredObj = currentPrincipal.Claims.Where(x => x.Type == ClaimTypes.Expired).FirstOrDefault();
                                    if (expiredObj != null)
                                    {
                                        var expired = expiredObj.Value.ToLong();
                                        var expiredDate = Utils.UnixTimeStampToDateTime(expired);

                                        // nếu token expired thì gen token mới
                                        if (System.DateTime.UtcNow >= expiredDate)
                                        {
                                            var userName = currentPrincipal.Identity.Name;
                                            //set the time when it expires
                                            System.DateTime expires = System.DateTime.UtcNow.AddMinutes(AppSettings.Instance.GetInt32(Const.JWTTimeout));
                                            var roles = string.Empty;
                                            var rolesObj = currentPrincipal.Claims.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault();
                                            if (rolesObj != null && !string.IsNullOrEmpty(rolesObj.Value)) roles = rolesObj.Value;

                                            var groups = string.Empty;
                                            var groupObj = currentPrincipal.Claims.Where(x => x.Type == JWTHelper.CustomizeClaimTypes.Groups).FirstOrDefault();
                                            if (groupObj != null && !string.IsNullOrEmpty(groupObj.Value)) groups = groupObj.Value;

                                            var userId = 0;
                                            var userIdObj = currentPrincipal.Claims.Where(x => x.Type == ClaimTypes.SerialNumber).FirstOrDefault();
                                            if (userIdObj != null && !string.IsNullOrEmpty(userIdObj.Value)) userId = userIdObj.Value.ToInt();

                                            var fullName = string.Empty;
                                            var fullNameObj = currentPrincipal.Claims.Where(x => x.Type == ClaimTypes.Surname).FirstOrDefault();
                                            if (fullNameObj != null && !string.IsNullOrEmpty(fullNameObj.Value)) fullName = fullNameObj.Value;

                                            var newToken = JWTHelper.Instance.CreateToken(userId, userName, fullName, checksumKey, expires, roles);
                                            userService.SaveJWTTokenOnCache(checksumKey, newToken);

                                            // lưu thêm token cũ expire 1p. Phòng trường hợp 2 request. Request sau đã bị obsolete token
                                            userService.SaveObsoleteJWTTokenOnCache(checksumKey, token);

                                            var Identity = currentPrincipal.Identity as ClaimsIdentity;
                                            Identity.AddClaim(new Claim(ClaimTypes.Authentication, newToken));
                                            currentPrincipal.AddIdentity(Identity);
                                        }

                                        context.User = currentPrincipal;
                                        await next(context);
                                    }
                                }
                                // check thêm obsolete token
                                else if (userService.CheckExitstObsoleteJWTTokenOnCache(checksumKey, token))
                                {
                                    context.User = currentPrincipal;
                                    await next(context);
                                }
                            }
                        }
                    }
                }

                throw new SecurityTokenValidationException();
            }
            catch (SecurityTokenValidationException e)
            {
                context.Response.StatusCode = HttpStatusCode.Unauthorized.GetHashCode();
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = ex.Message.Contains("JWT") ? HttpStatusCode.Unauthorized.GetHashCode() : HttpStatusCode.InternalServerError.GetHashCode();
            }

            return;
        }

        public bool LifetimeValidator(System.DateTime? notBefore, System.DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (expires != null)
            {
                //if (System.DateTime.UtcNow < expires)
                return true;
            }
            return false;
        }

        private bool TryRetrieveToken(HttpRequest request, out string token)
        {
            token = null;
            StringValues authzHeaders;
            if (!request.Headers.TryGetValue("Authorization", out authzHeaders) || authzHeaders.Count() > 1)
            {
                return false;
            }
            var bearerToken = authzHeaders.ElementAt(0);
            token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;
            return true;
        }
    }

}
