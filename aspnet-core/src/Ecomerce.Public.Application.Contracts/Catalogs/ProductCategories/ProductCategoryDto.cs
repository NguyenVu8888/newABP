﻿using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Ecommerce.Public.Catalogs.ProductCategories
{
    public class ProductCategoryDto : IEntityDto<Guid>
    {
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Slug { get; set; }
        public int SortOrder { get; set; }
        public string? CoverPucture { get; set; }
        public bool Visibility { get; set; }
        public bool IsActive { get; set; }
        public Guid? ParentId { get; set; }
        public string? SeoMetaDescreption { get; set; }
        public Guid Id { get; set; }
    }
}