using Ecomerce.Public;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Ecommerce.Public.Catalogs.ProductAttributes
{
    public interface IProductAttributeAppService : IReadOnlyAppService
       <ProductAttributeDto,
        Guid,
        PagedResultRequestDto>
    {
        Task<PagedResult<ProductAttributeInListDto>> GetListFilterAsync(BaseListFilterDto input);
        Task<List<ProductAttributeInListDto>> GetListAllAsync();

    }
}
