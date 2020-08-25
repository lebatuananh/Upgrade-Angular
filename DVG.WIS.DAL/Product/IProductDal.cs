using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.DAL.Product
{
    public interface IProductDal
    {
        IEnumerable<Entities.Product> GetList(string keyword, int status, out int totalRows, int? pageIndex = 1, int? pageSize = 15);
        Entities.Product GetById(int id);
    }
}
