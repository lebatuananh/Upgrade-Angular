using DVG.WIS.Core;
using DVG.WIS.Core.Constants;
using DVG.WIS.DAL.Account;
using DVG.WIS.Entities;
using DVG.WIS.PublicModel.CMS;
using DVG.WIS.Utilities;
using DVG.WIS.Utilities.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DVG.WIS.Business.Account
{
    public class AccountBo : IAccountBo
    {
        private IAccountDal _accountDal;

        public AccountBo(IAccountDal accountDal)
        {
            this._accountDal = accountDal;
        }

        public Entities.Account GetById(int userId)
        {
            try
            {
                return _accountDal.GetById(userId);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return new Entities.Account();
        }

        public Entities.Account GetByUserName(string userName)
        {
            try
            {
                return _accountDal.GetByUserName(userName);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return new Entities.Account();
        }

        public Entities.Account ValidateLogin(string userName, string password)
        {
            var user = GetUserInfoByAccountName(userName);
            if (user != null)
            {
                if (password == user.Password)
                    return user;
            }
            user = GetUserInfoByEmail(userName);
            if (user != null)
            {
                if (password == user.Password)
                    return user;
            }
            return null;
        }

        public Entities.Account GetUserInfoByEmail(string email)
        {
            return _accountDal.GetByEmail(email);
        }

        public Entities.Account GetUserInfoByAccountName(string accountName)
        {
            return _accountDal.GetByUserName(accountName);
        }

        public ErrorCodes Insert(Entities.Account user)
        {
            ErrorCodes errorCodes = ErrorCodes.Success;
            try
            {
                //if (!UserService.IsLogin())
                //{
                //    return ErrorCodes.NotLogin;
                //}

                if (user == null || string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password))
                {
                    return ErrorCodes.BusinessError;
                }

                _accountDal.Insert(user);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                errorCodes = ErrorCodes.Exception;
            }
            return errorCodes;
        }

        public ErrorCodes Update(Entities.Account user)
        {
            ErrorCodes errorCodes = ErrorCodes.Success;
            try
            {
                //if (!UserService.IsLogin())
                //{
                //    return ErrorCodes.NotLogin;
                //}

                if (user == null || string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password))
                {
                    return ErrorCodes.BusinessError;
                }

                _accountDal.Update(user);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                errorCodes = ErrorCodes.Exception;
            }
            return errorCodes;
        }

        public ErrorCodes ChangePassword(string username, string currentPassword, string passsword, string confirmPassword)
        {
            try
            {
                currentPassword = Crypton.Encrypt(currentPassword);
                passsword = Crypton.Encrypt(passsword);
                confirmPassword = Crypton.Encrypt(confirmPassword);

                var userInfo = _accountDal.GetByUserName(username);

                if (userInfo == null || userInfo.Id <= 0)
                {
                    return ErrorCodes.AccountNotExists;
                }
                if (!passsword.Equals(confirmPassword))
                {
                    return ErrorCodes.AccountPasswordNotMatch;
                }
                if (!userInfo.Password.Equals(currentPassword))
                {
                    return ErrorCodes.AccountLoginInvalidPassword;
                }

                userInfo.Password = passsword;
                userInfo.CreatedDate = DateTime.Now;
                userInfo.Password = passsword;

                _accountDal.Update(userInfo);

                // Create activity foor change pass
                //var userInfoLogin = UserService.GetUserLogin();

                return ErrorCodes.Success;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return ErrorCodes.UnknowError;
        }

        public IEnumerable<Entities.Account> GetList(string keyword,out int totalRows, int? userType = 0, int? pageIndex = 1, int? pageSize = 15)
        {
            try
            {
                return _accountDal.GetList(keyword, out totalRows, userType.Value, pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                totalRows = 0;
                return new List<Entities.Account>();
            }
        }

        public IEnumerable<Entities.Account> GetListByRole(int userType)
        {
            try
            {
                var total = 0;
                return this.GetList(string.Empty, out total, userType: userType, pageSize: 1000);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                return new List<Entities.Account>();
            }
        }

        public string GenerateEmailCreateAcount(string loginLink, string fullName, string userName, string pass, string accountType)
        {
            StringBuilder contentBuilder = new StringBuilder();
            contentBuilder.Append("<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"600\" style=\"margin: 0 auto;\">");
            contentBuilder.Append("<tbody>");
            contentBuilder.Append("<tr><td>");
            contentBuilder.Append("<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"100%\">");
            contentBuilder.Append("<tbody>");
            contentBuilder.Append("<tr><td width=\"60\"></td>");
            contentBuilder.Append("<td>");
            contentBuilder.Append("<h4 style=\"font-family: arial; font-size: 26px; font-weight: bold; color: #243b81; text-transform: uppercase; margin: 0 0 20px 0; padding: 0;\">THÔNG TIN TÀI KHOẢN</h4>");
            contentBuilder.AppendFormat("<font style=\"font-family: arial; font-size: 16px; color: #;323c3f\">Xin chào {0},</font>", fullName);
            contentBuilder.AppendFormat("<br><font style=\"font-family: arial; font-size: 16px; color: #;323c3f\">Quản trị viên vừa tạo cho bạn tài khoản trên hệ thống {0}. Bạn hãy truy cập vào link sau để kiểm tra tài khoản và thay đổi mật khẩu:</font>", AppSettings.Instance.GetString(Const.ProductionDomain));
            contentBuilder.Append("<br><br>");
            contentBuilder.Append("<div style=\"text-align: center;\">");
            contentBuilder.AppendFormat("<a style=\"display: inline-block; font-family: arial; color: #fff; font-size: 20px; font-weight: bold; text-transform: uppercase; text-decoration: none !important; padding: 10px 50px; background-color: #00a9f4; border-radius: 6px; -moz-border-radius: 6px; -webkit-border-radius: 6px;\" href=\"{0}\">Link đăng nhập hệ thống</a>", loginLink);
            contentBuilder.Append("</div><br>");
            contentBuilder.AppendFormat("<font style=\"font-family: arial; font-size: 16px; color: #;323c3f\"> Tên tài khoản : {0}</font>", userName);
            contentBuilder.AppendFormat("<br><font style=\"font-family: arial; font-size: 16px; color: #;323c3f\"> Mật khẩu : {0}</font>", pass);
            contentBuilder.AppendFormat("<br><font style=\"font-family: arial; font-size: 16px; color: #;323c3f\"> Loại tài khoản : {0}</font>", accountType);
            contentBuilder.AppendFormat("<br><br><font style=\"font-family: arial; font-size: 16px; color: #;323c3f\">Chúc bạn có những trải nhiệm thú vị với {0}.</font>", AppSettings.Instance.GetString(Const.ProductionDomain));
            contentBuilder.Append("<br><font style=\"font-family: arial; font-size: 16px; color: #;323c3f\">Trân trọng,</font>");
            contentBuilder.Append("<br><br><font style=\"font-family: arial; font-size: 16px; font-weight: bold; color: #;323c3f\">TSQ.VN</font>");
            contentBuilder.Append("</td>");
            contentBuilder.Append("<td width=\"60\"></td>");
            contentBuilder.Append("</tr>");
            contentBuilder.Append("</tbody></table>");
            contentBuilder.Append("</td></tr>");
            contentBuilder.Append("</tbody></table>");

            return contentBuilder.ToString();
        }
    }
}
