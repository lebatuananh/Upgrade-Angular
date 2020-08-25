using DVG.WIS.Core.Enums;
using DVG.WIS.DAL.Product;
using DVG.WIS.Utilities.Logs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.Business.Product
{
    public class ProductBo : IProductBo
    {
        private IProductDal _productDal;
        public ProductBo(IProductDal productDal)
        {
            this._productDal = productDal;
        }

        public IEnumerable<Entities.Product> GetList(string keyword, out int totalRows, int? pageIndex = 1, int? pageSize = 15)
        {
            try
            {
                return _productDal.GetList(keyword, ProductStatusEnum.Approved.GetHashCode(), out totalRows, pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                totalRows = 0;
                return new List<Entities.Product>();
            }
        }
        public Entities.Product GetById(int id)
        {
            try
            {
                return _productDal.GetById(id);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return new Entities.Product();
        }
    }
}
