using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.DAL.Ward
{
    public interface IWardDal
    {
        IEnumerable<Entities.Ward> GetAll();
    }
}
