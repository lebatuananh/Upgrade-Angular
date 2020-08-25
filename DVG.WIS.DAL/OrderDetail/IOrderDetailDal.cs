using DVG.WIS.Entities;
using DVG.WIS.PublicModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.DAL.OrderDetail
{
    public interface IOrderDetailDal
    {
        IEnumerable<Entities.OrderDetail> GetByOrderId(int id);
        IEnumerable<OrderDetailViewModel> GetAllByOrderId(int id);

        IEnumerable<Entities.OrderDetail> GetList(short status, DateTime fromCreatedDate, DateTime toCreatedDate);
    }
}
