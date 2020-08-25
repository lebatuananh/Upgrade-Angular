using DVG.WIS.Business.Base;
using DVG.WIS.Caching.Interfaces;
using DVG.WIS.Utilities.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.DAL.District
{
    public class DistrictBoCached : BaseService, IDistrictBoCached
    {
        private IDistrictBo _districtBo;
        public DistrictBoCached( IDistrictBo districtBo, ICached cacheClient) : base(cacheClient)
        {
            this._districtBo = districtBo;
        }


        public IEnumerable<Entities.District> GetByCityCode(string code)
        {
            return Execute(() => _districtBo.GetByCityCode(code), shortCacheDuration, code);
        }

        public IEnumerable<Entities.District> GetAll()
        {
            return Execute(() => _districtBo.GetAll(), shortCacheDuration);
        }

        public Entities.District GetById(int id)
        {
            return Execute(() => _districtBo.GetById(id), shortCacheDuration, id);
        }

    }
}
