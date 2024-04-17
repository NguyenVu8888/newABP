using System;
using System.Collections.Generic;
using System.Text;

namespace ecommerce.Admin.Catalog.Inventories
{
    public class CreateUpdateInventoryDto
    {
        public Guid ProductId { get; set; }
        public string? SKU { get; set; }
        public int SockQuantity { get; set; }
    }
}
