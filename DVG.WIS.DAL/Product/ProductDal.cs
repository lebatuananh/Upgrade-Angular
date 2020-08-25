using DVG.WIS.Utilities.Databases;
using DVG.WIS.Utilities.Databases.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.DAL.Product
{
    public class ProductDal : IProductDal
    {
        public IEnumerable<Entities.Product> GetList(string keyword, int status, out int totalRows, int? pageIndex = 1, int? pageSize = 15)
        {
            List<Entities.Product> list = new List<Entities.Product>();
            totalRows = 0;
            string storeName = "admin_product_getlist";
            string storeNameCount = "admin_product_getlist_count";
            try
            {
                using (var db = new PostgresSQL(ConnectionEntity.DBPosition.Master))
                {
                    using (var command = db.CreateCommand(storeNameCount, true))
                    {
                        command.Parameters.Add(new Npgsql.NpgsqlParameter("_keyword", keyword + ""));
                        command.Parameters.Add(new Npgsql.NpgsqlParameter("_status", status));
                        totalRows = (int)command.ExecuteScalar();
                    }

                    if (totalRows > 0)
                    {
                        using (var command = db.CreateCommand(storeName, true))
                        {
                            command.Parameters.Add(new Npgsql.NpgsqlParameter("_keyword", keyword + ""));
                            command.Parameters.Add(new Npgsql.NpgsqlParameter("_status", status));
                            command.Parameters.Add(new Npgsql.NpgsqlParameter("_pageindex", pageIndex));
                            command.Parameters.Add(new Npgsql.NpgsqlParameter("_pagesize", pageSize));
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var obj = new Entities.Product();
                                        EntityBase.SetObjectValue(reader, ref obj);
                                        list.Add(obj);
                                    }
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
            return list;
        }
        public Entities.Product GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
