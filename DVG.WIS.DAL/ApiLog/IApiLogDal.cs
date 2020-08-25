using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.DAL.ApiLog
{
    public interface IApiLogDal
    {
        int Insert(Entities.ApiLog obj);
    }
}
