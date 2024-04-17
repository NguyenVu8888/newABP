using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ecommerce.Admin.System.Roles
{
    public class CreateUpdateRoleDtoValidator : AbstractValidator<CreateUpdateRoleDto>
    {
        public CreateUpdateRoleDtoValidator()
        {

            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Desciption).NotEmpty();

        }
    }
}
