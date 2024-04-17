﻿using ecommerce.Attributes;
using ecommerce.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecommerce.Products
{
    public class ProductAttributeVarcharConfiguration:IEntityTypeConfiguration<ProductAttributeVarchar>
    {
        public void Configure(EntityTypeBuilder<ProductAttributeVarchar> builder)
        {
            builder.ToTable(ecommerceConsts.DbTablePrefix + "ProductAttributeVarchars");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Value).HasMaxLength(500);
        }
    }
}
