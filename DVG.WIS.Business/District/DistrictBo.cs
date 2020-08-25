using DVG.WIS.Utilities.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DVG.WIS.DAL.District
{
    public class DistrictBo : IDistrictBo
    {
        private IDistrictDal _districtDal;
        public DistrictBo(IDistrictDal districtDal)
        {
            this._districtDal = districtDal;
        }

        public IEnumerable<Entities.District> GetByCityCode(string code)
        {
            List<Entities.District> list = new List<Entities.District>();
            try
            {
                list = (List<Entities.District>)_districtDal.GetAll();
                list = list.Where(m => m.city_code == code).ToList();
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                return new List<Entities.District>();
            }
            return list;
        }

        public IEnumerable<Entities.District> GetAll()
        {
            try
            {
                return _districtDal.GetAll();
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                return new List<Entities.District>();
            }
        }

        public Entities.District GetById(int id)
        {
            Entities.District district = new Entities.District();
            List<Entities.District> list = new List<Entities.District>();
            try
            {
                list = (List<Entities.District>)_districtDal.GetAll();
                district = (Entities.District)list.Select(m => m.district_id == id);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return district;
        }
    }
}
