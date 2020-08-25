using DVG.WIS.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.DAL.OrderHistory
{
    public interface IOrderHistoryBo
    {
        ErrorCodes Insert(Entities.OrderHistory obj);
    }
}
