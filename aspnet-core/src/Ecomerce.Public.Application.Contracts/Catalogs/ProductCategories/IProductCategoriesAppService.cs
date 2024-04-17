using Ecomerce.Public;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Ecommerce.Public.Catalogs.ProductCategories
{
    public interface IProductCategoriesAppService : IReadOnlyAppService
        <ProductCategoryDto,
        Guid,
        PagedResultRequestDto
        >
    {

        Task<PagedResult<ProductCategoryInListDto>> GetListFilterAsync(BaseListFilterDto input);
       /* Task<IEnumerable<ProductCategoryInListDto>> GetListAllAsync();*/

        Task<List<ProductCategoryInListDto>> GetListAllAsync();
        Task<ProductCategoryDto> GetByCodeAsync(string code);

        Task<ProductCategoryDto> GetBySlugAsync(string slug);
    }
}

