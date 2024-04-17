﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ecommerce.Admin.Catalog.ProductAttributes
{
    public class CreateUpdateProductAttributeDtoValidator : AbstractValidator<CreateUpdateProductAttributeDto>
    {
        public CreateUpdateProductAttributeDtoValidator()
        {
            RuleFor(x => x.Label).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Code).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Type).NotEmpty();
        }

    }
}
