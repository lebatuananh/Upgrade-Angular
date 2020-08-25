using DVG.WIS.Entities;
using DVG.WIS.PublicModel;
using DVG.WIS.Utilities.Databases;
using DVG.WIS.Utilities.Databases.Base;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.DAL.Order
{
    public class OrderDal : NpgsqlHelpers, IOrderDal
    {
        public int CreateOrder(OrderFullViewModel order)
        {
            string storeName = "admin_order_insert_v1";
            string storeNameOrderDetail = "admin_orderdetail_insert";
            int order_id = 0;
            try
            {
                using (var db = new PostgresSQL(ConnectionEntity.DBPosition.Master))
                {
                    db.BeginTransaction();

                    try
                    {
                        using (var command = db.CreateCommand(storeName, true))
                        {
                            command.Parameters.Add(NpgsqlParameter("@_code", order.Code));
                            command.Parameters.Add(NpgsqlParameter("@_order_code", order.OrderCode));
                            command.Parameters.Add(NpgsqlParameter("@_customer_name", order.CustomerName));
                            command.Parameters.Add(NpgsqlParameter("@_city_code", order.CityCode));
                            command.Parameters.Add(NpgsqlParameter("@_district_id", order.DistrictId));
                            command.Parameters.Add(NpgsqlParameter("@_ward_id", order.WardId));
                            command.Parameters.Add(NpgsqlParameter("@_delivery_address", order.DeliveryAddress));
                            command.Parameters.Add(NpgsqlParameter("@_customer_phone", order.CustomerPhone));
                            command.Parameters.Add(NpgsqlParameter("@_customer_note", order.CustomerNote));
                            command.Parameters.Add(NpgsqlParameter("@_created_date", DateTime.Now));
                            command.Parameters.Add(NpgsqlParameter("@_modified_date", DateTime.Now));
                            command.Parameters.Add(NpgsqlParameter("@_created_by", order.CreatedBy));
                            command.Parameters.Add(NpgsqlParameter("@_modified_by", order.ModifiedBy));
                            command.Parameters.Add(NpgsqlParameter("@_reason_note", string.Empty));
                            command.Parameters.Add(NpgsqlParameter("@_origin_price", order.OrderOriginPrice));
                            command.Parameters.Add(NpgsqlParameter("@_price", order.OrderPrice));
                            command.Parameters.Add(NpgsqlParameter("@_status", order.Status));
                            command.Parameters.Add(NpgsqlParameter("@_ship_fee", order.ShipFee));
                            command.Parameters.Add(NpgsqlParameter("@_request_type", order.RequestType));
                            command.Parameters.Add(NpgsqlParameter("@_order_type", order.OrderType));
                            if (order.DeliverDate <= DateTime.Now)

                                command.Parameters.Add(NpgsqlParameter("@_delivery_date", DBNull.Value));
                            else
                                command.Parameters.Add(NpgsqlParameter("@_delivery_date", order.DeliverDate));
                            command.Parameters.Add(NpgsqlParameter("@_source_type", order.SourceType));

                            order_id = (int)command.ExecuteScalar();
                        }

                        if (order_id > 0)
                        {
                            foreach (OrderDetailViewModel item in order.ListProductViewModel)
                            {
                                using (var command = db.CreateCommand(storeNameOrderDetail, true))
                                {

                                    command.Parameters.Add(NpgsqlParameter("@_order_id", order_id));
                                    command.Parameters.Add(NpgsqlParameter("@_product_id", item.ProductId));
                                    command.Parameters.Add(NpgsqlParameter("@_quantity", item.Quantity));
                                    command.Parameters.Add(NpgsqlParameter("@_origin_price", item.OriginPrice));
                                    command.Parameters.Add(NpgsqlParameter("@_price", item.Price));
                                    command.Parameters.Add(NpgsqlParameter("@_created_date", DateTime.Now));
                                    command.Parameters.Add(NpgsqlParameter("@_created_by", order.CreatedBy));
                                    command.Parameters.Add(NpgsqlParameter("@_note", item.Note));
                                    command.ExecuteScalar();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        db.RollbackTransaction();
                        throw ex;
                    }

                    if (order_id > 0)
                    {
                        db.CommitTransaction();
                    }
                    else
                    {
                        db.RollbackTransaction();
                    }

                    return order_id;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }
        public int UpdateOrder(OrderFullViewModel order)
        {
            string storeName = "admin_order_update";
            string storeNameOrderDetail = "admin_orderdetail_insert";
            string storeNameOrderDetailDelete = "admin_orderdetail_deleteby_order_id";
            int result = 0;
            try
            {
                using (var db = new PostgresSQL(ConnectionEntity.DBPosition.Master))
                {
                    db.BeginTransaction();

                    try
                    {
                        using (var command = db.CreateCommand(storeName, true))
                        {
                            command.Parameters.Add(NpgsqlParameter("@_order_id", order.OrderId));
                            command.Parameters.Add(NpgsqlParameter("@_customer_name", order.CustomerName));
                            command.Parameters.Add(NpgsqlParameter("@_city_code", order.CityCode));
                            command.Parameters.Add(NpgsqlParameter("@_district_id", order.DistrictId));
                            command.Parameters.Add(NpgsqlParameter("@_ward_id", order.WardId));
                            command.Parameters.Add(NpgsqlParameter("@_delivery_address", order.DeliveryAddress));
                            command.Parameters.Add(NpgsqlParameter("@_customer_phone", order.CustomerPhone));
                            command.Parameters.Add(NpgsqlParameter("@_customer_note", order.CustomerNote));
                            //command.Parameters.Add(NpgsqlParameter("@_created_date", DateTime.Now));
                            //command.Parameters.Add(NpgsqlParameter("@_modified_date", DateTime.Now));
                            //command.Parameters.Add(NpgsqlParameter("@_created_by", order.CreatedBy));
                            command.Parameters.Add(NpgsqlParameter("@_modified_by", order.ModifiedBy));
                            command.Parameters.Add(NpgsqlParameter("@_reason_note", string.Empty));
                            command.Parameters.Add(NpgsqlParameter("@_origin_price", order.OrderOriginPrice));
                            command.Parameters.Add(NpgsqlParameter("@_price", order.OrderPrice));
                            command.Parameters.Add(NpgsqlParameter("@_status", order.Status));
                            command.Parameters.Add(NpgsqlParameter("@_ship_fee", order.ShipFee));
                            command.Parameters.Add(NpgsqlParameter("@_request_type", order.RequestType));
                            command.Parameters.Add(NpgsqlParameter("@_order_type", order.OrderType));
                            if (order.DeliverDate <= DateTime.Now)

                                command.Parameters.Add(NpgsqlParameter("@_delivery_date", DBNull.Value));
                            else
                                command.Parameters.Add(NpgsqlParameter("@_delivery_date", order.DeliverDate));
                            command.Parameters.Add(NpgsqlParameter("@_source_type", order.SourceType));

                            result = (int)command.ExecuteScalar();
                        }

                        if (result > 0)
                        {
                            using (var command = db.CreateCommand(storeNameOrderDetailDelete, true))
                            {
                                command.Parameters.Add(NpgsqlParameter("@_order_id", order.OrderId));

                                result = (int)command.ExecuteScalar();
                            }
                            foreach (OrderDetailViewModel item in order.ListProductViewModel)
                            {
                                using (var command = db.CreateCommand(storeNameOrderDetail, true))
                                {

                                    command.Parameters.Add(NpgsqlParameter("@_order_id", order.OrderId));
                                    command.Parameters.Add(NpgsqlParameter("@_product_id", item.ProductId));
                                    command.Parameters.Add(NpgsqlParameter("@_quantity", item.Quantity));
                                    command.Parameters.Add(NpgsqlParameter("@_origin_price", item.OriginPrice));
                                    command.Parameters.Add(NpgsqlParameter("@_price", item.Price));
                                    command.Parameters.Add(NpgsqlParameter("@_created_date", DateTime.Now));
                                    command.Parameters.Add(NpgsqlParameter("@_created_by", order.CreatedBy));
                                    command.Parameters.Add(NpgsqlParameter("@_note", item.Note));
                                    result = (int)command.ExecuteScalar();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        db.RollbackTransaction();
                        throw ex;
                    }

                    if (result > 0)
                    {
                        db.CommitTransaction();
                    }
                    else
                    {
                        db.RollbackTransaction();
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public int UpdateStatus(int order_id, short status, short requestType, string updatedBy, string reasonNote, string cashierReceive = "", string cashierRefund = "")
        {
            string storeName = "admin_order_updatestatus";
            try
            {
                using (var db = new PostgresSQL(ConnectionEntity.DBPosition.Master))
                {
                    using (var command = db.CreateCommand(storeName, true))
                    {
                        command.Parameters.Add(NpgsqlParameter("@_order_id", order_id));
                        command.Parameters.Add(NpgsqlParameter("@_status", status));
                        command.Parameters.Add(NpgsqlParameter("@_request_type", requestType));
                        command.Parameters.Add(NpgsqlParameter("@_reason_note", reasonNote));
                        command.Parameters.Add(NpgsqlParameter("@_cashier_receive", cashierReceive));
                        command.Parameters.Add(NpgsqlParameter("@_cashier_refund", cashierRefund));
                        command.Parameters.Add(NpgsqlParameter("@_modified_by", updatedBy));
                        return (int)command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        /// <summary>
        /// The update complete status
        /// Author: ThanhDT
        /// Created date: 8/11/2020 2:49 PM
        /// </summary>
        /// <param name="order_id">The order identifier.</param>
        /// <param name="status">The status.</param>
        /// <param name="deliveryStatus">The delivery status.</param>
        /// <param name="updatedBy">The updated by.</param>
        /// <param name="reasonNote">The reason note.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public int UpdateCompleteStatus(int order_id, short status, short deliveryStatus, string updatedBy, string reasonNote)
        {
            string storeName = "admin_order_updatecompletestatus";
            try
            {
                using (var db = new PostgresSQL(ConnectionEntity.DBPosition.Master))
                {
                    using (var command = db.CreateCommand(storeName, true))
                    {
                        command.Parameters.Add(NpgsqlParameter("@_order_id", order_id));
                        command.Parameters.Add(NpgsqlParameter("@_status", status));
                        command.Parameters.Add(NpgsqlParameter("@_delivery_status", deliveryStatus));
                        command.Parameters.Add(NpgsqlParameter("@_reason_note", reasonNote));
                        command.Parameters.Add(NpgsqlParameter("@_modified_by", updatedBy));
                        return (int)command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public IEnumerable<Entities.Order> GetList(string keyword, short status, short sourceType, short deliveryStatus, short orderType, DateTime? fromCreatedDate, DateTime? toCreatedDate, short requestType, out int totalRows, int? pageIndex = 1, int? pageSize = 15)
        {
            totalRows = 0;
            string storeName = "admin_order_getlist_20200824";
            string storeNameCount = "admin_order_getlist_count_v1";
            var lisOrders = new List<Entities.Order>();
            try
            {
                using (var db = new PostgresSQL(ConnectionEntity.DBPosition.Master))
                {
                    using (var command = db.CreateCommand(storeNameCount, true))
                    {
                        command.Parameters.Add(NpgsqlParameter("@_keyword", keyword + ""));
                        command.Parameters.Add(NpgsqlParameter("@_status", status));
                        command.Parameters.Add(NpgsqlParameter("@_sourcetype", sourceType));
                        command.Parameters.Add(NpgsqlParameter("@_deliverystatus", deliveryStatus));
                        command.Parameters.Add(NpgsqlParameter("@_ordertype", orderType));
                        command.Parameters.Add(NpgsqlParameter("@_requesttype", requestType));
                        if (fromCreatedDate == null)
                            command.Parameters.Add(new NpgsqlParameter("@_fromcreateddate", DBNull.Value));
                        else
                            command.Parameters.Add(new NpgsqlParameter("@_fromcreateddate", fromCreatedDate.Value));
                        if (toCreatedDate == null)
                            command.Parameters.Add(new NpgsqlParameter("@_tocreateddate", DBNull.Value));
                        else
                            command.Parameters.Add(new NpgsqlParameter("@_tocreateddate", toCreatedDate.Value));
                        totalRows = (int)command.ExecuteScalar();
                    }

                    if (totalRows > 0)
                    {
                        using (var command = db.CreateCommand(storeName, true))
                        {
                            command.Parameters.Add(NpgsqlParameter("@_keyword", keyword + ""));
                            command.Parameters.Add(NpgsqlParameter("@_status", status));
                            command.Parameters.Add(NpgsqlParameter("@_sourcetype", sourceType));
                            command.Parameters.Add(NpgsqlParameter("@_deliverystatus", deliveryStatus));
                            command.Parameters.Add(NpgsqlParameter("@_ordertype", orderType));
                            command.Parameters.Add(NpgsqlParameter("@_requesttype", requestType));
                            if (fromCreatedDate == null)
                                command.Parameters.Add(new NpgsqlParameter("@_fromcreateddate", DBNull.Value));
                            else
                                command.Parameters.Add(new NpgsqlParameter("@_fromcreateddate", fromCreatedDate.Value));
                            if (toCreatedDate == null)
                                command.Parameters.Add(new NpgsqlParameter("@_tocreateddate", DBNull.Value));
                            else
                                command.Parameters.Add(new NpgsqlParameter("@_tocreateddate", toCreatedDate.Value));
                            command.Parameters.Add(NpgsqlParameter("@_pageindex", pageIndex));
                            command.Parameters.Add(NpgsqlParameter("@_pagesize", pageSize));
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        var obj = new Entities.Order();
                                        EntityBase.SetObjectValue(reader, ref obj);
                                        lisOrders.Add(obj);
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
            return lisOrders;
        }

        /// <summary>
        /// The get list pending to kitchen
        /// Author: ThanhDT
        /// Created date: 8/9/2020 2:29 AM
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="limit">The limit.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public IEnumerable<Entities.Order> GetListPendingToKitchen(short status, int limit)
        {
            string storeName = "service_order_get_list_pending_to_kitchen";
            var lisOrders = new List<Entities.Order>();
            try
            {
                using (var db = new PostgresSQL(ConnectionEntity.DBPosition.Master))
                {
                    using (var command = db.CreateCommand(storeName, true))
                    {
                        command.Parameters.Add(NpgsqlParameter("@_status", status));
                        command.Parameters.Add(NpgsqlParameter("@_limit", limit));
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    var obj = new Entities.Order();
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

        public IEnumerable<Entities.Order> GetListToDelivery(short status, int limit)
        {
            string storeName = "service_order_get_list_to_delivery";
            var lisOrders = new List<Entities.Order>();
            try
            {
                using (var db = new PostgresSQL(ConnectionEntity.DBPosition.Master))
                {
                    using (var command = db.CreateCommand(storeName, true))
                    {
                        command.Parameters.Add(NpgsqlParameter("@_status", status));
                        command.Parameters.Add(NpgsqlParameter("@_limit", limit));
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    var obj = new Entities.Order();
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

        /// <summary>
        /// The update delivery status
        /// Author: ThanhDT
        /// Created date: 8/11/2020 9:08 AM
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="deliveryStatus">The delivery status.</param>
        /// <param name="cancelStatus">The cancel status.</param>
        /// <param name="updatedBy">The updated by.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public int UpdateDeliveryStatus(int orderId, short deliveryStatus, short cancelStatus, string updatedBy)
        {
            string storeName = "admin_order_update_delivery_status";
            try
            {
                using (var db = new PostgresSQL(ConnectionEntity.DBPosition.Master))
                {
                    using (var command = db.CreateCommand(storeName, true))
                    {
                        command.Parameters.Add(NpgsqlParameter("@_order_id", orderId));
                        command.Parameters.Add(NpgsqlParameter("@_delivery_status", deliveryStatus));
                        command.Parameters.Add(NpgsqlParameter("@_cancel_status", cancelStatus));
                        command.Parameters.Add(NpgsqlParameter("@_modified_by", updatedBy));
                        return (int)command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public int UpdateRequestType(int orderId, short requestType, string reasonNote, string updatedBy)
        {
            string storeName = "admin_order_updaterequesttype";
            try
            {
                using (var db = new PostgresSQL(ConnectionEntity.DBPosition.Master))
                {
                    using (var command = db.CreateCommand(storeName, true))
                    {
                        command.Parameters.Add(NpgsqlParameter("@_order_id", orderId));
                        command.Parameters.Add(NpgsqlParameter("@_request_type", requestType));
                        command.Parameters.Add(NpgsqlParameter("@_reason_note", reasonNote));
                        command.Parameters.Add(NpgsqlParameter("@_modified_by", updatedBy));
                        return (int)command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        /// <summary>
        /// The move to kitchen
        /// Author: ThanhDT
        /// Created date: 8/12/2020 9:06 AM
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public int MoveToKitchen(Entities.Order order)
        {
            string storeName = "admin_order_move_to_kitchen";
            try
            {
                using (var db = new PostgresSQL(ConnectionEntity.DBPosition.Master))
                {
                    using (var command = db.CreateCommand(storeName, true))
                    {
                        command.Parameters.Add(NpgsqlParameter("@_order_id", order.order_id));
                        command.Parameters.Add(NpgsqlParameter("@_status", order.status));
                        command.Parameters.Add(NpgsqlParameter("@_delivery_price", order.delivery_price));
                        command.Parameters.Add(NpgsqlParameter("@_delivery_estimate_time", order.delivery_estimate_time));
                        command.Parameters.Add(NpgsqlParameter("@_reason_note", order.reason_note, NpgsqlDbType.Varchar));
                        return (int)command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        /// <summary>
        /// The change to delivery
        /// Author: ThanhDT
        /// Created date: 8/11/2020 5:06 PM
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public int ChangeToDelivery(Entities.Order order)
        {
            string storeName = "admin_order_change_to_delivery";
            try
            {
                using (var db = new PostgresSQL(ConnectionEntity.DBPosition.Master))
                {
                    using (var command = db.CreateCommand(storeName, true))
                    {
                        command.Parameters.Add(NpgsqlParameter("@_order_id", order.order_id));
                        //command.Parameters.Add(NpgsqlParameter("@_status", order.status));
                        command.Parameters.Add(NpgsqlParameter("@_delivery_status", order.delivery_status));
                        command.Parameters.Add(NpgsqlParameter("@_delivery_order_id", order.delivery_order_id, NpgsqlDbType.Varchar));
                        command.Parameters.Add(NpgsqlParameter("@_delivery_price", order.delivery_price));
                        command.Parameters.Add(NpgsqlParameter("@_delivery_estimate_time", order.delivery_estimate_time));
                        command.Parameters.Add(NpgsqlParameter("@_reason_note", order.reason_note, NpgsqlDbType.Varchar));
                        return (int)command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        

        public Entities.Order GetById(int id)
        {
            string storeName = "admin_order_getbyid_v1";
            Entities.Order result = new Entities.Order();
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
                                reader.Read();
                                EntityBase.SetObjectValue(reader, ref result);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
            return result;
        }

        /// <summary>
        /// The get by delivery order identifier
        /// Author: ThanhDT
        /// Created date: 8/11/2020 12:21 AM
        /// </summary>
        /// <param name="deliveryOrderId">The delivery order identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public Entities.Order GetByDeliveryOrderId(string deliveryOrderId)
        {
            string storeName = "admin_order_getbydeliveryorderid";
            Entities.Order result;
            try
            {
                using (var db = new PostgresSQL(ConnectionEntity.DBPosition.Master))
                {
                    using (var command = db.CreateCommand(storeName, true))
                    {
                        command.Parameters.Add(NpgsqlParameter("@_delivery_order_id", deliveryOrderId, NpgsqlDbType.Varchar));
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                result = new Entities.Order();
                                reader.Read();
                                EntityBase.SetObjectValue(reader, ref result);
                            }
                            else
                            {
                                result = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
            return result;
        }

        public IEnumerable<Entities.Order> GetList(short status, DateTime fromCreatedDate, DateTime toCreatedDate)
        {
            string storeName = "admin_order_getlist_statistic_V1";
            var lisOrders = new List<Entities.Order>();
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
                                    var obj = new Entities.Order();
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

        public IEnumerable<Entities.OrderCount> CountOrder()
        {
            string storeName = "admin_order_count";
            var lisOrders = new List<Entities.OrderCount>();
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
                                    var obj = new Entities.OrderCount();
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

        public int UpdateDeliveryPrice(int orderId, string user, int deliveryPrice, short deliveryStatus)
        {
            string storeName = "admin_order_update_delivery_price";
            try
            {
                using (var db = new PostgresSQL(ConnectionEntity.DBPosition.Master))
                {
                    using (var command = db.CreateCommand(storeName, true))
                    {
                        command.Parameters.Add(NpgsqlParameter("@_order_id", orderId));
                        command.Parameters.Add(NpgsqlParameter("@_modified_by", user));
                        command.Parameters.Add(NpgsqlParameter("@_delivery_price", deliveryPrice));
                        command.Parameters.Add(NpgsqlParameter("@_delivery_status", deliveryStatus));
                        return (int)command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public IEnumerable<CustomerAddress> SearchCustomerByPhone(string keyword)
        {
            string storeName = "admin_customer_addres_getbykeyword";
            var lisCustomers = new List<Entities.CustomerAddress>();
            try
            {
                using (var db = new PostgresSQL(ConnectionEntity.DBPosition.Master))
                {
                    using (var command = db.CreateCommand(storeName, true))
                    {
                        command.Parameters.Add(NpgsqlParameter("@_keyword", keyword));
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    var obj = new Entities.CustomerAddress();
                                    EntityBase.SetObjectValue(reader, ref obj);
                                    lisCustomers.Add(obj);
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
            return lisCustomers;
        }
    }
}
