using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ecommerce.Admin.Catalog.InventoryTickets.IventoryTicketItems
{
    public class CreateUpdateIventoryTicketItemDtoValidator : AbstractValidator<CreateUpdateIventoryTicketItemDto>
    {
        public CreateUpdateIventoryTicketItemDtoValidator()
        {
            RuleFor(x => x.InventoryTicketId).NotNull();
            RuleFor(x => x.ProductId).NotNull();
            RuleFor(x => x.SKU).NotEmpty();
            RuleFor(x => x.Quantity).GreaterThanOrEqualTo(1);
            RuleFor(x => x.BatchNumber).NotEmpty();
            RuleFor(x => x.ExpiredDate).NotNull();
        }
    }
}
