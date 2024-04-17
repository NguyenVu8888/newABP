using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ecommerce.Admin.Catalog.Inventories
{
    public class InventoryInListDto :EntityDto<Guid>
    {
        public Guid ProductId { get; set; }
        public string? SKU { get; set; }
        public int SockQuantity { get; set; }
    }
}
