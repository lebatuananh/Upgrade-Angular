using DVG.WIS.Entities;
using DVG.WIS.PublicModel;
using DVG.WIS.Utilities.Databases;
using DVG.WIS.Utilities.Databases.Base;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.DAL.OrderDetail
{
    public class OrderDetailDal : NpgsqlHelpers, IOrderDetailDal
    {
        public IEnumerable<Entities.OrderDetail> GetByOrderId(int id)
        {
            string storeName = "admin_orderdetail_getbyorderid";
            var lisOrderDetail = new List<Entities.OrderDetail>();
            try
            {
                using (var db = new PostgresSQL(ConnectionEntity.DBPosition.Master))
                {
                    using (var command = db.CreateCommand(storeName, true))
                    {
                        command.Parameters.Add(NpgsqlParameter("@_id", id));

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    var obj = new Entities.OrderDetail();
                                    EntityBase.SetObjectValue(reader, ref obj);
                                    lisOrderDetail.Add(obj);
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
            return lisOrderDetail;
        }

        public IEnumerable<OrderDetailViewModel> GetAllByOrderId(int id)
        {
            string storeName = "admin_orderdetail_getallbyorderid";
            var lisOrderDetail = new List<OrderDetailViewModel>();
            try
            {
                using (var db = new PostgresSQL(ConnectionEntity.DBPosition.Master))
                {
                    using (var command = db.CreateCommand(storeName, true))
                    {
                        command.Parameters.Add(NpgsqlParameter("@_id", id));

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    var obj = new OrderDetailViewModel();
                                    EntityBase.SetObjectValue(reader, ref obj);
                                    lisOrderDetail.Add(obj);
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
            return lisOrderDetail;
        }

        public IEnumerable<Entities.OrderDetail> GetList(short status, DateTime fromCreatedDate, DateTime toCreatedDate)
        {
            string storeName = "admin_orderdetail_getlist_statistic";
            var lisOrders = new List<Entities.OrderDetail>();
            try
            {
                using (var db = new PostgresSQL(ConnectionEntity.DBPosition.Master))
                {
                    using (var command = db.CreateCommand(storeName, true))
                    {
                        command.Parameters.Add(NpgsqlParameter("@_status", status));
                        command.Parameters.Add(NpgsqlParameter("@_fromcreateddate", fromCreatedDate));
                        command.Parameters.Add(NpgsqlParameter("@_tocreateddate", toCreatedDate));
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    var obj = new Entities.OrderDetail();
                                    EntityBase.SetObjectValue(reader, ref obj);
                                    lisOrders.Add(obj);
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
            return lisOrders;
        }
    }
}
