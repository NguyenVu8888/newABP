using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Ecommerce.Public.Catalogs.Manufactures
{
    public class ManufactureDto : IEntityDto<Guid>
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Slug { get; set; }
        public string? CoverPicture { get; set; }
        public bool Visibility { get; set; }
        public bool IsActive { get; set; }
        public string? Country { get; set; }
    }
}
