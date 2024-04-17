﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ecommerce.Admin.Catalog.Manufactures
{
    public class CreateUpdateManufactureDto
    {

        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Slug { get; set; }
        public string? CoverPicture { get; set; }
        public bool Visibility { get; set; }
        public bool IsActive { get; set; }
        public string? Country { get; set; }
    }
}
