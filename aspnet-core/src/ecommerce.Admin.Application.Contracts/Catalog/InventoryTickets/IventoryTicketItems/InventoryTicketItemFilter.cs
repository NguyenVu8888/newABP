using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ecommerce.Admin.Catalog.InventoryTickets.IventoryTicketItems
{
    public class InventoryTicketItemFilter : PagedResultRequestDto
    {
        public Guid? InventoryTicketId { get; set; }
    }
}
