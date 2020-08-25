using DVG.WIS.Business.Base;
using DVG.WIS.Caching.Interfaces;
using DVG.WIS.Utilities.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.DAL.Ward
{
    public class WardBoCached : BaseService, IWardBoCached
    {
        private IWardBo _wardBo;
        public WardBoCached(IWardBo wardBo, ICached cacheClient) : base(cacheClient)
        {
            this._wardBo = wardBo;
        }


        public IEnumerable<Entities.Ward> GetByDistrictId(int district_id)
        {
            return Execute(() => _wardBo.GetByDistrictId(district_id), shortCacheDuration, district_id);
        }
        public IEnumerable<Entities.Ward> GetAll()
        {
            return Execute(() => _wardBo.GetAll(), shortCacheDuration);
        }
        public Entities.Ward GetById(int id)
        {
            return Execute(() => _wardBo.GetById(id), shortCacheDuration, id);
        }

    }
}
