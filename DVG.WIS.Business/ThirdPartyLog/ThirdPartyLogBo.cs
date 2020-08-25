using DVG.WIS.Entities;
using DVG.WIS.Utilities.Logs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.DAL.ThirdPartyLog
{
    public class ThirdPartyLogBo : IThirdPartyLogBo
    {
        private IThirdPartyLogDal _thirdPartyLogDal;

        public ThirdPartyLogBo(IThirdPartyLogDal thirdPartyLogDal)
        {
            this._thirdPartyLogDal = _thirdPartyLogDal;
        }

        public ErrorCodes Insert(Entities.ThirdPartyLog obj)
        {
            ErrorCodes errorCodes = ErrorCodes.Success;
            try
            {
                obj.created_date = DateTime.Now;
                var result = _thirdPartyLogDal.Insert(obj);
                if (result <= 0) errorCodes = ErrorCodes.BusinessError;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                errorCodes = ErrorCodes.Exception;
            }
            return errorCodes;
        }
        public ErrorCodes Update(Entities.ThirdPartyLog obj)
        {
            ErrorCodes errorCodes = ErrorCodes.Success;
            try
            {
                obj.updated_date = DateTime.Now;
                var result = _thirdPartyLogDal.Update(obj);
                if (result <= 0) errorCodes = ErrorCodes.BusinessError;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                errorCodes = ErrorCodes.Exception;
            }
            return errorCodes;
        }
    }
}
