using DVG.WIS.Business.Account;
using DVG.WIS.Business.Base;
using DVG.WIS.Caching;
using DVG.WIS.Caching.Interfaces;
using DVG.WIS.Core;
using DVG.WIS.Core.Constants;
using DVG.WIS.Core.Enums;
using DVG.WIS.DAL.Account;
using DVG.WIS.Entities;
using DVG.WIS.PublicModel;
using DVG.WIS.Utilities;
using DVG.WIS.Utilities.Logs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace DVG.WIS.Business.Authenticator
{
    public class UserService : BaseService, IUserService
    {
        #region properties

        private IHttpContextAccessor _httpContextAccessor;
        private IAccountBo _accountBo;
        private static readonly int _weekExpiredInMinute = AppSettings.Instance.GetInt32(Const.KeyWeekCacheTime);
        protected int weekCacheDuration = StaticVariable.WeekCacheTime;

        private string UserName
        {
            get
            {
                return _httpContextAccessor.HttpContext.User.Identity.Name;
            }
        }

        private short Role
        {
            get
            {
                if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    var identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
                    var roles = identity.Claims.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault();
                    if (roles != null && !string.IsNullOrEmpty(roles.Value))
                    {
                        return roles.Value.ToShort();
                    }
                }
                return 0;
            }
        }

        private string CurrentCashier
        {
            get
            {
                var value = cacheClient.Get(ConstKeyCached.CashierConstraintKey);
                return value != null ? value : string.Empty;
            }
        }

        private bool allowProcessCashier
        {
            get
            {
                if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    var identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
                    var roles = identity.Claims.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault();
                    if (roles != null && !string.IsNullOrEmpty(roles.Value))
                    {
                        if (roles.Value == UserTypeEnum.Cashier.GetHashCode().ToString())
                        {
                            var userName = UserName;
                            if (!string.IsNullOrEmpty(userName))
                            {
                                var value = cacheClient.Get(ConstKeyCached.CashierConstraintKey);
                                return !string.IsNullOrEmpty(value) ? userName == value : false;
                            }
                            return false;
                        }
                    }
                }
                return true;
            }
        }

        private bool isAdmin
        {
            get
            {
                var user = GetUserLogin();
                if (user != null)
                {
                    return user.UserType == UserTypeEnum.Admin.GetHashCode();
                }
                return false;
            }
        }

        #endregion

        #region constructor

        //public UserService() : base(new RedisCached())
        //{
        //    _accountBo = new AccountBo(new AccountDal());
        //    _httpContextAccessor = new HttpContextAccessor();
        //}

        public UserService(IAccountBo accountBo, IHttpContextAccessor httpContextAccessor, ICached cacheClient) : base(cacheClient)
        {
            _accountBo = accountBo;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region public methods

        public string GetUserName()
        {
            return this.UserName;
        }

        public short GetRole()
        {
            return this.Role;
        }

        public string GetCurrentCashier()
        {
            return this.CurrentCashier;
        }

        public bool AllowProcessCashier()
        {
            return this.allowProcessCashier;
        }

        public bool IsAdmin()
        {
            return this.isAdmin;
        }

        public Tuple<int, string, List<int>> GetCurrentUser()
        
        {
            var identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userName = identity.Name;
                var UserIdObj = identity.Claims.Where(x => x.Type == ClaimTypes.SerialNumber).FirstOrDefault();
                if (UserIdObj != null && !string.IsNullOrEmpty(UserIdObj.Value))
                {

                    var roles = new List<int>();
                    var roleObj = identity.Claims.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault();
                    if (roleObj != null)
                    {
                        var roleStr = roleObj.Value;
                        if (roleStr.Length > 0)
                        {
                            var arr = roleStr.Split(',');
                            if (arr.Length > 0) roles = arr.Select(x => x.ToInt()).ToList();
                        }
                    }

                    return new Tuple<int, string, List<int>>(UserIdObj.Value.ToInt(), userName, roles);
                }
            }
            return null;
        }

        public bool IsLogin()
        {
            try
            {
                var isLogin = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
                // && !string.IsNullOrEmpty (HttpContext.Current.User.Identity.Name);
                return isLogin;
            }
            catch (Exception exception)
            {
                Logger.WriteLog(Logger.LogType.Error, exception.ToString());
                return false;
            }
        }

        public UserLogin GetUserLogin()
        {
            UserLogin userInfo = new UserLogin();
            try
            {
                if (IsLogin())
                {
                    var userName = _httpContextAccessor.HttpContext.User.Identity.Name;
                    if (userInfo == null || userInfo.UserId <= 0)
                    {
                        Entities.Account existsUsers = _accountBo.GetUserInfoByAccountName(userName);
                        if (existsUsers != null && existsUsers.Id > 0)
                        {
                            var userActived = existsUsers.Status != (int)UserStatusAdmin.Deactived &&
                                              existsUsers.Status != (int)UserStatusAdmin.Deleted;

                            if (!userActived) return userInfo = new UserLogin();

                            userInfo = new UserLogin
                            {
                                UserId = existsUsers.Id,
                                Email = existsUsers.Email,
                                FullName = existsUsers.FullName,
                                UserName = existsUsers.UserName,
                                Avatar = existsUsers.Avatar,
                                DisplayName = existsUsers.FullName,
                                CreatedDate = existsUsers.CreatedDate,
                                UserType = existsUsers.UserType,
                            };

                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.WriteLog(Logger.LogType.Error, exception.ToString());
            }
            return userInfo;
        }

        public ResponseData LoginForCms(string userName, string password, bool isSavedPassword = false, string secureCode = "")
        {
            var responseData = Login(userName, password, isSavedPassword, secureCode);

            if (responseData.Success)
            {
                if (!responseData.Success)
                {
                    responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.AccessDenined);
                }
            }

            return responseData;
        }

        private ResponseData Login(string userName, string password, bool isSavedPassword = false, string secureCode = "")
        {
            var responseData = new ResponseData();

            if (string.IsNullOrEmpty(userName))
            {
                responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.AccountLoginInvalidUserName);
            }
            if (string.IsNullOrEmpty(password))
            {
                responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.AccountLoginInvalidPassword);
            }

            password = Crypton.Encrypt(password);

            Entities.Account userEntity = _accountBo.ValidateLogin(userName, password);
            if (userEntity != null && userEntity.Id > 0)
            {
                if (userEntity.Status == (int)UserStatusAdmin.Deactived)
                {
                    responseData.Success = false;
                    responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.AccountLoginUserBanned);
                    responseData.ErrorCode = (int)ErrorCodes.AccountLoginUserBanned;
                    return responseData;
                }

                if (userEntity.Status == (int)UserStatusAdmin.Deleted)
                {
                    responseData.Success = false;
                    responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.AccountLoginUserRemoved);
                    responseData.ErrorCode = (int)ErrorCodes.AccountLoginUserRemoved;
                    return responseData;
                }

                responseData.Data = userEntity;

                DoLogin(userName, ref responseData, true);

                return responseData;
            }

            responseData.Success = false;
            responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.AccountLoginWrongUserNameOrPassword);
            responseData.ErrorCode = (int)ErrorCodes.AccountLoginWrongUserNameOrPassword;
            return responseData;
        }

        public void DoLogin(string accountName, ref ResponseData loginResult, bool saveCookie)
        {
            if (!IsLogin())
            {
                try
                {
                    bool @bool = AppSettings.Instance.GetBool(Const.DebugMode);
                    Entities.Account userInfo = _accountBo.GetByUserName(accountName);
                    if ((userInfo != null) && (userInfo.Id > 0))
                    {
                        UserLogin login = new UserLogin(userInfo);

                        //var claims = new List<Claim>
                        //{
                        //    new Claim(ClaimTypes.Name, login.UserName),
                        //    new Claim(ClaimTypes.Role, login.UserType.ToString())
                        //};

                        //var userIdentity = new ClaimsIdentity(claims, "login");

                        //ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                        //AuthenticationHttpContextExtensions.SignInAsync(_httpContextAccessor.HttpContext, CookieAuthenticationDefaults.AuthenticationScheme, principal);

                        string token = string.Empty;
                        // lưu checksumKey và token vào cache
                        loginResult.Success = this.JwtLogin(login, out token);
                        //return the token
                        loginResult.Token = token;
                        loginResult.Success = true;
                        loginResult.Message = StringUtils.GetEnumDescription(ErrorCodes.Success);
                        loginResult.ErrorCode = (int)ErrorCodes.Success;

                        // Lưu cache nếu user là Cashier
                        if (login.UserType == UserTypeEnum.Cashier.GetHashCode())
                            SetOnlyCashier(login.UserName);
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                }
            }
        }

        public ResponseData Logout()
        {
            var result = new ResponseData();
            var identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var checksumKey = identity.Claims.Where(x => x.Type == ClaimTypes.Sid).FirstOrDefault();
            if (checksumKey != null && !string.IsNullOrEmpty(checksumKey.Value))
            {
                result.Success = DeleteJWTTokenOnCache(checksumKey.Value);
            }
            return result;
        }

        public void SetOnlyCashier(string userName)
        {
            cacheClient.Set(ConstKeyCached.CashierConstraintKey, userName, weekCacheDuration);
        }


        #region JWT
        public bool SaveJWTTokenOnCache(string checksumKey, string token)
        {
            string keyCached = KeyCacheHelper.GenCacheKeyStatic(ConstKeyCached.JWTToken, checksumKey);
            return cacheClient.Set(keyCached, token, _weekExpiredInMinute);

        }
        public bool SaveObsoleteJWTTokenOnCache(string checksumKey, string token)
        {
            string keyCached = KeyCacheHelper.GenCacheKeyStatic(ConstKeyCached.ObsoleteJWTToken, checksumKey);
            return cacheClient.Set(keyCached, token, 1);
        }
        public bool ChecksumJWTOnCache(string checksumKey)
        {
            string keyCached = KeyCacheHelper.GenCacheKeyStatic(ConstKeyCached.JWTToken, checksumKey);
            return cacheClient.ContainsKey(keyCached);
        }

        public bool CheckExitstJWTTokenOnCache(string checksumKey, string token)
        {
            string keyCached = KeyCacheHelper.GenCacheKeyStatic(ConstKeyCached.JWTToken, checksumKey);
            var val = cacheClient.Get(keyCached);
            if (!string.IsNullOrEmpty(val))
            {
                return token == val;
            }
            return false;
        }

        public bool CheckExitstObsoleteJWTTokenOnCache(string checksumKey, string token)
        {
            string keyCached = KeyCacheHelper.GenCacheKeyStatic(ConstKeyCached.ObsoleteJWTToken, checksumKey);
            var val = cacheClient.Get(keyCached);
            if (!string.IsNullOrEmpty(val))
            {
                return token == val;
            }
            return false;
        }

        public bool DeleteJWTTokenOnCache(string checksumKey)
        {
            string keyCached = KeyCacheHelper.GenCacheKeyStatic(ConstKeyCached.JWTToken, checksumKey);
            return cacheClient.Remove(keyCached);
        }

        //public void DeleteAllChecksumOnCache(string username)
        //{
        //    if (!string.IsNullOrWhiteSpace(username))
        //    {
        //        string prefix = KeyCacheHelper.GenCacheKeyStatic(ConstKeyCached.JWTToken, username + ":");
        //        cacheClient.RemovePrefix(prefix);
        //    }
        //}
        #endregion

        #endregion

        #region private methods

        private bool JwtLogin(UserLogin userInfo, out string token)
        {
            //set the time when it expires
            System.DateTime expires = System.DateTime.UtcNow.AddMinutes(AppSettings.Instance.GetInt32(Const.JWTTimeout));
            // gen checksumKey
            var checksumKey = JWTHelper.Instance.GenerateChecksumKey(userInfo.UserName);
            token = JWTHelper.Instance.CreateToken(userInfo.UserId, userInfo.UserName, userInfo.DisplayName, checksumKey, expires, userInfo.UserType.ToString());

            // lưu checksumKey và token vào cache
            return SaveJWTTokenOnCache(checksumKey, token);

        }

        #endregion
    }
}
