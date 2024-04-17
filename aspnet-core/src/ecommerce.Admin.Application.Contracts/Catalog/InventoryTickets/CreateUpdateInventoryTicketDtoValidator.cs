using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ecommerce.Admin.Catalog.InventoryTickets
{
    public class CreateUpdateInventoryTicketDtoValidator :AbstractValidator<CreateUpdateInventoryTicketDto>
    {
        public CreateUpdateInventoryTicketDtoValidator()
        {
            RuleFor(x=>x.ApproverDate).NotNull().GreaterThanOrEqualTo(DateTime.Now);
            RuleFor(x => x.TicketType).NotNull();
            RuleFor(x => x.ApproverId).NotNull();
        }
    }
}
