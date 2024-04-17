using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ecommerce.Admin.Catalog.Products
{
    public class CreateUpdateProductDtoValidator : AbstractValidator<CreateUpdateProductDto>
    {
        public CreateUpdateProductDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Code).NotEmpty().MaximumLength(50);
            RuleFor(x => x.SKU).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Name).MaximumLength(250);
            RuleFor(x => x.Name).MaximumLength(250);
        }

    }
}
