using ecommerce.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace ecommerce.Admin.Catalog.Products
{
    public class CreateUpdateProductDto
    {
        public Guid ManufictureId { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public ProductType ProductType { get; set; }
        public string? SKU { get; set; }
        /* public string? Slug { get; set; }*/
        public int SortOrder { get; set; }
        public bool Visibility { get; set; }
        public bool IsActive { get; set; }
        public Guid CategoryId { get; set; }
        public string? SeoMetaDescription { get; set; }
        public string? Descreption { get; set; }


        public string? ThumbnailPictureName { get; set; }
        public string? ThumbnailPictureContent { get; set; }
    }
}
