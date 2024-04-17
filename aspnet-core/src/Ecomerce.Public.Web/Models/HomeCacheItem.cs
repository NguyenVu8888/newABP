using System.Collections.Generic;
using Ecommerce.Public.Catalogs.ProductCategories;
using Ecommerce.Public.Catalogs.Products;

namespace Ecomerce.Public.Web.Models
{
    public class HomeCacheItem
    {
        public List<ProductCategoryInListDto> Categories { set; get; }
        public List<ProductInListDto> Products { set; get; }
    }
}
