using ecommerce.Attributies;
using System;
using Volo.Abp.Application.Dtos;

namespace Ecommerce.Public.Catalogs.ProductAttributes
{
    public class ProductAttributeInListDto : EntityDto<Guid>
    {
        public string? Code { get; set; }
        public AttributeType Type { get; set; }
        public string? Label { get; set; }
        public int SortOrder { get; set; }
        public bool Vissibility { get; set; }
        public bool IsActive { get; set; }
        public bool IsRequired { get; set; }
        public bool IsUnique { get; set; }
    }
}
