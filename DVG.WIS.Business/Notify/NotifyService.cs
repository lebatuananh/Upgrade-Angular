using DVG.WIS.Business.Account;
using DVG.WIS.Business.Authenticator;
using DVG.WIS.Business.Base;
using DVG.WIS.Caching;
using DVG.WIS.Caching.Interfaces;
using DVG.WIS.Core;
using DVG.WIS.Core.Constants;
using DVG.WIS.Core.Enums;
using DVG.WIS.Utilities.Logs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.Business.Notify
{
    public class NotifyService : BaseService, INotifyService
    {
        protected int longCacheTime = StaticVariable.LongCacheTime;

        private IAccountBo _accountBo;
        public NotifyService(IAccountBo accountBo, ICached cacheClient) : base(cacheClient)
        {
            _accountBo = accountBo;
        }

        public async void PushNotifyToAccounts(string orderCode, RequestTypeEnum type)
        {
            try
            {
                var mess = string.Empty;
                IEnumerable<Entities.Account> lstAcc = new List<Entities.Account>();
                switch (type)
                {
                    case RequestTypeEnum.CSRequestDestroy:
                        {
                            mess = string.Format("Mã đơn {0} - CSKH báo hủy đơn!", orderCode);
                            lstAcc = _accountBo.GetListByRole(UserTypeEnum.KitchenManager.GetHashCode());
                            break;
                        }
                    case RequestTypeEnum.KitchenDestroy:
                        {
                            mess = string.Format("Mã đơn {0} - Quản lý bếp đã hủy đơn, cần báo lại khách!", orderCode);
                            lstAcc = _accountBo.GetListByRole(UserTypeEnum.CustomerService.GetHashCode());
                            break;
                        }
                }

                foreach (var acc in lstAcc)
                {
                    var key = KeyCacheHelper.GenCacheKeyStatic(ConstKeyCached.NotifyAccount, acc.UserName);
                    var score = DateTime.Now.Ticks;
                    await cacheClient.SortedSetAddAsync<string>(key, mess, score);

                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
        public IEnumerable<string> GetNotifyForAccounts(string userName)
        {
            var result = new List<string>();
            try
            {
                var key = KeyCacheHelper.GenCacheKeyStatic(ConstKeyCached.NotifyAccount, userName);
                result = cacheClient.SortedSetRangeByRankAsync<string>(key).Result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return result;
        }
        public bool DeleteNotifyAccount(string userName)
        {
            try
            {
                var key = KeyCacheHelper.GenCacheKeyStatic(ConstKeyCached.NotifyAccount, userName);
                return cacheClient.Remove(key);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return false;
        }
    }
}
