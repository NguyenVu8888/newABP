using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ecommerce.Admin.Catalog.Inventories
{
    public class CreateUpdateInventoryDtoValidator : AbstractValidator<CreateUpdateInventoryDto>
    {
        public CreateUpdateInventoryDtoValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.SKU).NotEmpty().MaximumLength(50);         
            RuleFor(x => x.SockQuantity).GreaterThanOrEqualTo(1);
        }
    }
}
