using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.DAL.Account
{
    public interface IAccountDal
    {
        int Insert(Entities.Account obj);
        int Update(Entities.Account obj);
        IEnumerable<Entities.Account> GetList(string keyword, out int totalRows, int UserType = 0, int? pageIndex = 1, int? pageSize = 15);
        IEnumerable<Entities.Account> GetAll();
        Entities.Account GetById(int userId);
        Entities.Account GetByUserName(string userName);
        Entities.Account GetByEmail(string email);
        
    }
}
