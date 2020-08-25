using DVG.WIS.Utilities.Databases;
using DVG.WIS.Utilities.Databases.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.DAL.Ward
{
    public class WardDal : NpgsqlHelpers, IWardDal
    {
        public IEnumerable<Entities.Ward> GetAll()
        {
            List<Entities.Ward> listWard = new List<Entities.Ward>();
            string storeName = "admin_ward_getall";
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
                                    var obj = new Entities.Ward();
                                    EntityBase.SetObjectValue(reader, ref obj);
                                    listWard.Add(obj);
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
            return listWard;
        }
    }
}
