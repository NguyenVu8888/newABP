using ecommerce.Attributies;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ecommerce.Admin.Catalog.Products.Attributes
{
    public class addUpdateProductAttributeDto
    {
        public Guid ProductId { get; set; }
        public Guid AttributeId { get; set; }


        public DateTime? DateTimeValue { get; set; }
        public decimal? DecimalValue { get; set; }
        public int? IntValue { get; set; }
        public string? TextValue { get; set; }
        public string? VarcharValue { get; set; }
    }
}
