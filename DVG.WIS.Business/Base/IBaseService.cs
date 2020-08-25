using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Business.Base
{
    public interface IBaseService
    {
        string GetFriendlyTypeName(Type type);
    }
}
