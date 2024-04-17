using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace ecommerce.InventoryTickets
{
    public class InventoryTicketItem:Entity<Guid>
    {
        public InventoryTicketItem(Guid id, Guid inventoryTicketId, Guid productId, string? sKU, int quantity, string? batchNumber, DateTime expiredDate)
        {
            Id = id;
            InventoryTicketId = inventoryTicketId;
            ProductId = productId;
            SKU = sKU;
            Quantity = quantity;
            BatchNumber = batchNumber;
            ExpiredDate = expiredDate;
        }

        public Guid InventoryTicketId { get; set; }
        public Guid ProductId { get; set; }
        public string? SKU { get; set; }
        public int Quantity { get; set; }
        public string? BatchNumber { get; set; }
        public DateTime ExpiredDate { get; set; }

    }
}
