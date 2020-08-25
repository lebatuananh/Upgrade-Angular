using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.DAL.OrderHistory
{
    public interface IOrderHistoryDal
    {
        int Insert(Entities.OrderHistory obj);
    }
}
