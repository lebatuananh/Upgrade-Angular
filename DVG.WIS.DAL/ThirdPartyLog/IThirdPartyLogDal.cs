using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.DAL.ThirdPartyLog
{
    public interface IThirdPartyLogDal
    {
        int Insert(Entities.ThirdPartyLog obj);
        int Update(Entities.ThirdPartyLog obj);
    }
}
