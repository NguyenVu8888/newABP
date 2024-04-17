using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ecommerce.Admin.Catalog.Manufactures
{
    public class CreateUpdateManufactureDtoValidator : AbstractValidator<CreateUpdateManufactureDto>
    {
        public CreateUpdateManufactureDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Code).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Slug).NotEmpty().MaximumLength(50);
            RuleFor(x => x.CoverPicture).MaximumLength(250);
        }
    }
}
