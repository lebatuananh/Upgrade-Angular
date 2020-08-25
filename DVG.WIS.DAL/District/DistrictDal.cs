using DVG.WIS.Utilities.Databases;
using DVG.WIS.Utilities.Databases.Base;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.DAL.District
{
    public class DistrictDal : NpgsqlHelpers, IDistrictDal
    {
        public IEnumerable<Entities.District> GetAll()
        {
            List<Entities.District> listDistrict = new List<Entities.District>();
            string storeName = "admin_district_getall";
            try
            {
                using (var db = new PostgresSQL(ConnectionEntity.DBPosition.Master))
                {
                    using (var command = db.CreateCommand(storeName, true))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    var obj = new Entities.District();
                                    EntityBase.SetObjectValue(reader, ref obj);
                                    listDistrict.Add(obj);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
            return listDistrict;
        }
    }
}
