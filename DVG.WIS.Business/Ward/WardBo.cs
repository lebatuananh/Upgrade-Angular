using DVG.WIS.Utilities.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DVG.WIS.DAL.Ward
{
    public class WardBo :IWardBo
    {
        private IWardDal _wardDal;
        public WardBo(IWardDal wardal)
        {
            this._wardDal = wardal;
        }
        public IEnumerable<Entities.Ward> GetByDistrictId(int district_id)
        {
            List<Entities.Ward> list = new List<Entities.Ward>();
            try
            {
                list = (List<Entities.Ward>)_wardDal.GetAll();
                list = list.Where(m => m.district_id == district_id).ToList();
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                return new List<Entities.Ward>();
            }
            return list;
        }
        public IEnumerable<Entities.Ward> GetAll()
        {
            try
            {
                return _wardDal.GetAll();
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                return new List<Entities.Ward>();
            }
        }

        public Entities.Ward GetById(int id)
        {
            Entities.Ward ward = new Entities.Ward();
            List<Entities.Ward> list = new List<Entities.Ward>();
            try
            {
                list = (List<Entities.Ward>)_wardDal.GetAll();
                ward = (Entities.Ward)list.Select(m => m.ward_id == id);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return ward;
        }
    }
}
