using DVG.WIS.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.PublicModel
{
    public class CustomerAddressModel
    {
        public int AddressId { get; set; }
        public string Name { get; set; }
        public string CityCode { get; set; }
        public int DistrictId { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public byte IsDefault { get; set; }

        public CustomerAddressModel(CustomerAddress address)
        {
            this.AddressId = address.address_id;
            this.Name = address.name;
            this.CityCode = address.city_code;
            this.DistrictId = address.district_id;
            this.Address = address.address;
            this.Phone = address.phone;
            this.IsDefault = address.is_default;
        }
    }
}
