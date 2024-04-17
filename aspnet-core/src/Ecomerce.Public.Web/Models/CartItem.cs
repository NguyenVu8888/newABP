using System;
using Ecommerce.Public.Catalogs.Products;


namespace Ecomerce.Public.Web.Models
{
    public class CartItem
    {
        public ProductDto Product { get; set; }
        public int Quantity { get; set; }
    }
}
