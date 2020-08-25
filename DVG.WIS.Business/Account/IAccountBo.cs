using DVG.WIS.Entities;
using DVG.WIS.PublicModel.CMS;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.Business.Account
{
    public interface IAccountBo
    {
        Entities.Account GetById(int userId);
        Entities.Account GetByUserName(string userName);
        Entities.Account ValidateLogin(string userName, string password);
        Entities.Account GetUserInfoByEmail(string email);
        Entities.Account GetUserInfoByAccountName(string accountName);
        ErrorCodes Insert(Entities.Account user);
        ErrorCodes Update(Entities.Account user);
        ErrorCodes ChangePassword(string username, string currentPassword, string passsword, string confirmPassword);
        IEnumerable<Entities.Account> GetList(string keyword, out int totalRows, int? userType = 0, int? pageIndex = 1, int? pageSize = 15);
        IEnumerable<Entities.Account> GetListByRole(int userType);
        string GenerateEmailCreateAcount(string loginLink, string fullName, string userName, string pass,
            string accountType);
    }
}
