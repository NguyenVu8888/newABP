using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Public.Catalogs.Products.Attributes
{
    public class ProductAttributeListFilterDto : BaseListFilterDto
    {
        public Guid ProductId { get; set; }
    }
}
