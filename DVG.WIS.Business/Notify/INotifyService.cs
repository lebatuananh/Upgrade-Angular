using DVG.WIS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.Business.Notify
{
    public interface INotifyService
    {
        void PushNotifyToAccounts(string orderCode, RequestTypeEnum type);
        IEnumerable<string> GetNotifyForAccounts(string userName);
        bool DeleteNotifyAccount(string userName);
    }
}
