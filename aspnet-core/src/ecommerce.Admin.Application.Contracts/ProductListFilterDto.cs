using System;
using System.Collections.Generic;
using System.Text;

namespace ecommerce.Admin
{
    public class ProductListFilterDto:BaseListFilterDto
    {
        public Guid? CategoryId { get; set; }

    }
}
