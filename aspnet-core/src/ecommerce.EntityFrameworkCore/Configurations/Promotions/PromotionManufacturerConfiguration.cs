using ecommerce.Promotions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecommerce.Promotions
{
    public class PromotionManufacturerConfiguration:IEntityTypeConfiguration<PromotionManufacture>
    {
        public void Configure(EntityTypeBuilder<PromotionManufacture> builder)
        {
            builder.ToTable(ecommerceConsts.DbTablePrefix + "PromotionManufacturers");
            builder.HasKey(x => x.Id);
        }
    }
}
