using Ecomerce.Public;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Ecommerce.Public
{
    public class BaseListFilterDto:PagedResultRequestBase
    {
        public string? keyword { get; set; }
    }
}
