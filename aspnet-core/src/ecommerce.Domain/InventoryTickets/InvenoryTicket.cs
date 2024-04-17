using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace ecommerce.InventoryTickets
{
    public class InvenoryTicket:AuditedAggregateRoot<Guid>
    {

        public DateTime ApproverDate { get; set; }
        public string? Code { get; set; }

        public TicketType TicketType { get; set; }
        public Guid? ApproverId { get; set; }
        public bool IsApproved { get; set; }
    }
}
