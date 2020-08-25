using DVG.WIS.Entities;
using DVG.WIS.Utilities.Logs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.DAL.OrderHistory
{
    public class OrderHistoryBo : IOrderHistoryBo
    {
        private IOrderHistoryDal _orderHistoryDal;

        public OrderHistoryBo(IOrderHistoryDal orderHistoryDal)
        {
            this._orderHistoryDal = orderHistoryDal;
        }

        public ErrorCodes Insert(Entities.OrderHistory obj)
        {
            ErrorCodes errorCodes = ErrorCodes.Success;
            try
            {
                obj.created_date = DateTime.Now;
                var result = _orderHistoryDal.Insert(obj);
                if (result <= 0) errorCodes = ErrorCodes.BusinessError;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                errorCodes = ErrorCodes.Exception;
            }
            return errorCodes;
        }
    }
}
