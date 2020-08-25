using DVG.WIS.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.PublicModel
{

    public class SuggestAddressModel
    {
        public SuggestAddressModel(SuggestAddress address)
        {
            this.FormattedAddress = address.formatted_address;
            this.Address = address.address;
            this.Lng = address.lng;
            this.City = address.city;
            this.District = address.district;
            this.Ward = address.ward;
            this.Lat = address.lat;
            this.DistrictId = address.district_id;
        }
        public string FormattedAddress { get; set; }
        public string Address { get; set; }
        public double Lng { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public double Lat { get; set; }
        public int DistrictId { get; set; }
    }

    public class SearchSuggestAddressModel
    {
        public string text { get; set; }
        public int rows { get; set; }

        public SearchSuggestAddressModel()
        {
            this.rows = 10;
        }

    }

    public class ResponseAddressModel
    {
        public string repair { get; set; }
        public List<SuggestAddress> suggestion { get; set; }
        public string text { get; set; }
    }
}
