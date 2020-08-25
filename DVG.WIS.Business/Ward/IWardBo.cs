using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.DAL.Ward
{
    public interface IWardBo
    {
        IEnumerable<Entities.Ward> GetByDistrictId(int district_id);
        IEnumerable<Entities.Ward> GetAll();
        Entities.Ward GetById(int id);
    }
}
