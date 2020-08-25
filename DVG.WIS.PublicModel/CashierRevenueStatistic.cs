using DVG.WIS.Core;
using DVG.WIS.Core.Enums;
using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DVG.WIS.PublicModel
{
    [Serializable]
    public class CashierRevenueIndexModel
    {
        public CashierRevenueIndexModel()
        {
            //this.FromDateIndex = Convert.ToDateTime(DateTime.Now.Month + "/" + "01" + "/" + DateTime.Now.Year).ToString(Const.DateTimeFormatStringClient);
            this.FromDateIndex = DateTime.Now.AddDays(-7).ToString(Const.DateTimeFormatStringClient);
            this.ToDateIndex = DateTime.Now.ToString(Const.DateTimeFormatStringClient);
            this.ListRevenueType = EnumHelper.Instance.ConvertEnumToList<RevenueTypeEnum>().ToList();
        }
        public IEnumerable<EnumHelper.Enums> ListRevenueType { get; set; }
        public string FromDateIndex { get; set; }
        public string ToDateIndex { get; set; }
    }

    [Serializable]
    public class CashierRevenueSearchModel
    {
        public IEnumerable<DateTime> LstDate { get; set; }
        public IEnumerable<CashierRevenueStatisticModel> LstData { get; set; }
        public IEnumerable<CashierRevenueStatisticModel> LstDataSouceType { get; set; }
        public IEnumerable<CashierRevenueStatisticModel> LstDataPaymentType { get; set; }
        public IEnumerable<CashierRevenueStatisticModel> LstDataProduct { get; set; }
        public IEnumerable<CashierRevenueStatisticModel> LstCountProduct { get; set; }
        public IEnumerable<CashierRevenueStatisticModel> LstCountBill { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string FromDateStr { get; set; }
        public string ToDateStr { get; set; }
        public int Type { get; set; }
    }

    [Serializable]
    public class CashierRevenueStatisticModel
    {
        public CashierRevenueStatisticModel()
        {
            lstDay = new List<CashierRevenueStatisticByDayModel>();
        }
        public string CashierName { get; set; }
        public string TotalStr { get; set; }
        public List<CashierRevenueStatisticByDayModel> lstDay { get; set; }
    }

    [Serializable]
    public class CashierRevenueStatisticByDayModel
    {
        public DateTime Day { get; set; }
        public int Revenue { get; set; }
        public string RevenueStr
        {
            get
            {
                return StringUtils.ConvertNumberToCurrency(Revenue);
            }
        }
    }
}
