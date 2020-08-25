using DVG.WIS.PublicModel;
using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;

namespace DVG.WIS.Business.Authenticator
{
    public interface IUserService
    {
        Tuple<int, string, List<int>> GetCurrentUser();

        bool IsLogin();

        UserLogin GetUserLogin();

        ResponseData LoginForCms(string userName, string password, bool isSavedPassword = false, string secureCode = "");

        void DoLogin(string accountName, ref ResponseData loginResult, bool saveCookie);

        ResponseData Logout();

        void SetOnlyCashier(string userName);

        bool SaveJWTTokenOnCache(string checksumKey, string token);

        bool SaveObsoleteJWTTokenOnCache(string checksumKey, string token);

        bool ChecksumJWTOnCache(string checksumKey);

        bool CheckExitstJWTTokenOnCache(string checksumKey, string token);

        bool CheckExitstObsoleteJWTTokenOnCache(string checksumKey, string token);

        bool DeleteJWTTokenOnCache(string checksumKey);

        string GetUserName();

        short GetRole();

        string GetCurrentCashier();

        bool AllowProcessCashier();

        bool IsAdmin();
    }
}
