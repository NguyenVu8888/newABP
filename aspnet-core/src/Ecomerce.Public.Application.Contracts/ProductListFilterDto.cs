using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Public
{
    public class ProductListFilterDto:BaseListFilterDto
    {
        public Guid? CategoryId { get; set; }

    }
}
