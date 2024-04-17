using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace ecommerce.Products
{
    public class ProductTag:Entity<Guid>
    {
        public Guid ProductId { get; set; }
        public string? TagId { get; set; }
    }
}
