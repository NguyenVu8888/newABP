using ecommerce.InventoryTickets;
using System;
using System.Collections.Generic;
using System.Text;

namespace ecommerce.Admin.Catalog.InventoryTickets
{
    public class CreateUpdateInventoryTicketDto
    {
        public DateTime ApproverDate { get; set; }
        public string? Code { get; set; }

        public TicketType TicketType { get; set; }
        public Guid? ApproverId { get; set; }
        public bool IsApproved { get; set; }
    }
}
