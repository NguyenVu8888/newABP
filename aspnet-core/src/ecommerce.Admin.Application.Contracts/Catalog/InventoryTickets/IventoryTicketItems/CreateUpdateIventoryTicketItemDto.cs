using System;
using System.Collections.Generic;
using System.Text;

namespace ecommerce.Admin.Catalog.InventoryTickets.IventoryTicketItems
{
    public class CreateUpdateIventoryTicketItemDto
    {
        public Guid InventoryTicketId { get; set; }
        public Guid ProductId { get; set; }
        public string? SKU { get; set; }
        public int Quantity { get; set; }
        public string? BatchNumber { get; set; }
        public DateTime ExpiredDate { get; set; }
    }
}
