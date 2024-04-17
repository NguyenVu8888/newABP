using ecommerce.InventoryTickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecommerce.InventoryTickets
{
    public class InventoryTicketConfiguration :IEntityTypeConfiguration<InvenoryTicket>
    {
        public void Configure(EntityTypeBuilder<InvenoryTicket> builder)
        {
            builder.ToTable(ecommerceConsts.DbTablePrefix + "InventoryTickets");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Code)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();
        }
    }
}
