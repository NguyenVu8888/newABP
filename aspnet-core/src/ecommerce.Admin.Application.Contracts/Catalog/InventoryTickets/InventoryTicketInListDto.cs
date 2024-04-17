using ecommerce.InventoryTickets;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ecommerce.Admin.Catalog.InventoryTickets
{
    public class InventoryTicketInListDto :EntityDto<Guid>
    {
        public DateTime ApproverDate { get; set; }
        public string? Code { get; set; }

        public TicketType TicketType { get; set; }
        public Guid? ApproverId { get; set; }
        public bool IsApproved { get; set; }
    }
}
