using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ecommerce.Admin.Catalog.Products.Attributes
{
    public class addUpdateProductAttributeDtoValidator : AbstractValidator<addUpdateProductAttributeDto>
    {
        public addUpdateProductAttributeDtoValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.AttributeId).NotEmpty();
        }
    }
}
