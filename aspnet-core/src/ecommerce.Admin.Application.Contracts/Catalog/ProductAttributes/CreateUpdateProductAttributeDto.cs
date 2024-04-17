using ecommerce.Attributies;
using System;
using System.Collections.Generic;
using System.Text;

namespace ecommerce.Admin.Catalog.ProductAttributes
{
    public class CreateUpdateProductAttributeDto
    {
        public string? Code { get; set; }
        public AttributeType Type { get; set; }
        public string? Label { get; set; }
        public int SortOrder { get; set; }
        public bool Vissibility { get; set; }
        public bool IsActive { get; set; }
        public bool IsRequired { get; set; }
        public bool IsUnique { get; set; }
        public string? Note { get; set; }
    }
}
