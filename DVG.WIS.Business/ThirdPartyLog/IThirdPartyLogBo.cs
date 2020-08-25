using DVG.WIS.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.DAL.ThirdPartyLog
{
    public interface IThirdPartyLogBo
    {
        ErrorCodes Insert(Entities.ThirdPartyLog obj);
        ErrorCodes Update(Entities.ThirdPartyLog obj);
    }
}
