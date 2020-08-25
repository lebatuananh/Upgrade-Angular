using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.DAL.District
{
    public interface IDistrictDal
    {
        IEnumerable<Entities.District> GetAll();

    }
}
