using DVG.WIS.Utilities.Databases;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.DAL.OrderHistory
{
    public class OrderHistoryDal : NpgsqlHelpers, IOrderHistoryDal
    {
        public int Insert(Entities.OrderHistory obj)
        {
            string storeName = "admin_order_histories_insert";
            try
            {
                using (var db = new PostgresSQL(ConnectionEntity.DBPosition.Master))
                {
                    using (var command = db.CreateCommand(storeName, true))
                    {
                        command.Parameters.Add(NpgsqlParameter("@_order_id", obj.order_id));
                        command.Parameters.Add(NpgsqlParameter("@_created_by", obj.created_by));
                        command.Parameters.Add(NpgsqlParameter("@_change_log", obj.change_log));
                        command.Parameters.Add(NpgsqlParameter("@_status", obj.status));
                        command.Parameters.Add(NpgsqlParameter("@_delivery_status", obj.delivery_status));
                        command.Parameters.Add(NpgsqlParameter("@_request_type", obj.request_type));
                        return (int)command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }
    }
}
