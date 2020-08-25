using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.DAL.District
{
    public interface IDistrictBoCached
    {
        IEnumerable<Entities.District> GetByCityCode(string code);
        IEnumerable<Entities.District> GetAll();
        Entities.District GetById(int id);
    }
}
