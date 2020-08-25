using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.PublicModel
{
    public class ProductSearchModel
    {
        public string KeyWord { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public ProductSearchModel()
        {
            this.PageIndex = 1;
            this.PageSize = 15;
        }
    }

    public class ProductViewModel
    {
        public ProductViewModel(Entities.Product product)
        {
            ProductId = product.product_id;
            Code = product.code;
            ProductName = product.name;
            Price = product.price;
            PriceStr = StringUtils.ConvertNumberToCurrency(product.price);
            OriginPrice = product.origin_price;
            OriginPriceStr = StringUtils.ConvertNumberToCurrency(product.origin_price);
            CookingTime = product.cooking_time;
        }

        public int ProductId { get; set; }
        public int Code { get; set; }
        public string ProductName { get; set; }
        public int Price { get; set; }
        public string PriceStr { get; set; }
        public int OriginPrice { get; set; }
        public string OriginPriceStr { get; set; }
        public short CookingTime { get; set; }
    }
}
