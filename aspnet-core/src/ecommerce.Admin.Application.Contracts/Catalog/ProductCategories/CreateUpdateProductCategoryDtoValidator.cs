using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ecommerce.Admin.Catalog.ProductCategories
{
    public class CreateUpdateProductCategoryDtoValidator : AbstractValidator<CreateUpdateProductCategoryDto>
    {
        public CreateUpdateProductCategoryDtoValidator()
        {

            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Code).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Slug).NotEmpty().MaximumLength(50);
            RuleFor(x => x.CoverPucture).MaximumLength(250);
            RuleFor(x => x.SeoMetaDescreption).MaximumLength(250);
        }
    }
}
