using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.Business.Product
{
    public interface IProductBo
    {
        IEnumerable<Entities.Product> GetList(string keyword, out int totalRows, int? pageIndex = 1, int? pageSize = 15);
        Entities.Product GetById(int id);
    }
}
