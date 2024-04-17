using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ecommerce.Admin.Catalog.InventoryTickets.IventoryTicketItems
{
    public class InventoryTicketItemInListDto 
    {
        public Guid InventoryTicketId { get; set; }
        public Guid ProductId { get; set; }
        public string? SKU { get; set; }
        public int Quantity { get; set; }
        public string? BatchNumber { get; set; }
        public DateTime ExpiredDate { get; set; }
    }
}
