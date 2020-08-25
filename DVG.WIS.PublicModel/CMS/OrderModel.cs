using DVG.WIS.Core.Constants;
using DVG.WIS.Core.Enums;
using DVG.WIS.Entities;
using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DVG.WIS.PublicModel.CMS
{
    public class OrderSearchModel
    {
        public string KeyWord { get; set; }
        public short RequestType { get; set; }
        public short DeliveryStatus { get; set; }
        public short OrderType { get; set; }
        public short SourceType { get; set; }
        public short Status { get; set; }
        public DateTime? FromCreatedDate { get; set; }
        public DateTime? ToCreatedDate { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public OrderSearchModel()
        {
            this.SourceType = -1;
            this.RequestType = -1;
            this.DeliveryStatus = -1;
            this.OrderType = -1;
            this.Status = -1;
            this.PageIndex = 1;
            this.PageSize = 15;
        }
    }
    public class OrderIndexModel
    {
        public IEnumerable<DVG.WIS.Utilities.EnumHelper.Enums> ListDeliveryStatus { get; set; }
        public IEnumerable<DVG.WIS.Utilities.EnumHelper.Enums> ListSourceType { get; set; }
        public IEnumerable<DVG.WIS.Utilities.EnumHelper.Enums> ListOrderType { get; set; }
        public short Status { get; set; }
        public short Role { get; set; }
        public string PrintKitchenApi { get { return StaticVariable.PrintKitchenApi; } }
        public string PrintCashierApi { get { return StaticVariable.PrintCashierApi; } }
        public IEnumerable<OrderCountModel> ListOrderCountModel { get; set; }
        public OrderIndexModel()
        {
            this.ListDeliveryStatus = EnumHelper.Instance.ConvertEnumToList<DeliveryStatus>().ToList();
            this.ListSourceType = EnumHelper.Instance.ConvertEnumToList<SourceTypeEnum>().ToList();
            this.ListOrderType = EnumHelper.Instance.ConvertEnumToList<OrderTypeEnum>().ToList();
        }
    }
    public class OrderViewModel : Order
    {
        public OrderViewModel()
        {

        }

        public OrderViewModel(Order entity, IEnumerable<District> listDistrict, IEnumerable<Ward> listWard, IEnumerable<EnumInfo> listOrderType, IEnumerable<Product> listProduct, IEnumerable<OrderDetail> listOrderDetail)
        {
            try
            {
                if (entity != null)
                {
                    var entityType = entity.GetType();
                    foreach (var pModel in this.GetType().GetProperties())
                    {
                        var pEntityName = entityType.GetProperty(pModel.Name);
                        if (pEntityName != null)
                        {
                            var value = pEntityName.GetValue(entity, null);
                            pModel.SetValue(this, value);
                        }
                    }
                }
                list_district = listDistrict;
                list_ward = listWard;
                list_order_type = listOrderType;
                list_product = listProduct;
                list_order_detail = listOrderDetail;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (listDistrict != null)
            {
                var disObj = listDistrict.Where(x => x.district_id == entity.district_id).FirstOrDefault();
                if (disObj != null)
                {
                    district_id = disObj.district_id;
                    district_name = disObj.district_name;
                }
            }
            if (listWard != null)
            {
                var wardObj = listWard.Where(x => x.ward_id == entity.ward_id).FirstOrDefault();
                if (wardObj != null)
                {
                    ward_id = wardObj.ward_id;
                    ward_name = wardObj.ward_name;
                }
            }

            if (listOrderType != null)
            {
                var orderObj = listOrderType.Where(x => x.value == entity.order_type).FirstOrDefault();
                if (orderObj != null)
                {
                    order_type = orderObj.value;
                    order_type_name = orderObj.description;
                }
            }
        }

        public IEnumerable<District> list_district { get; set; }
        public IEnumerable<Ward> list_ward { get; set; }
        public IEnumerable<EnumInfo> list_order_type { get; set; }
        public IEnumerable<OrderDetail> list_order_detail { get; set; }
        public IEnumerable<Product> list_product { get; set; }
        public string district_name { get; set; }
        public string ward_name { get; set; }
        public string order_type_name { get; set; }
        public string delivery_date_string { get; set; }
        public OrderDetail order_detail_item { get; set; }
    }

}
      
 