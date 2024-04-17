﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace ecommerce.Promotions
{
    public class PromotionManufacture:Entity<Guid>
    {
        public Guid ManufactureId { get; set; }
        public Guid PromotionId { get; set; }
    }
}