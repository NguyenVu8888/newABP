using Ecomerce.Public;
using Ecommerce.Public.Catalogs.Products.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Ecommerce.Public.Catalogs.Products
{
    public interface IProductsAppService : IReadOnlyAppService
        <ProductDto,
        Guid,
        PagedResultRequestDto
        
        >
    {
        Task<PagedResult<ProductInListDto>> GetListFilterAsync(ProductListFilterDto input);
        Task<List<ProductInListDto>> GetListAllAsync();

        Task<string> GetThumnailImageAsync(string fileName);

        Task<string> GetSuggestNewCodeAsync();


        Task<List<ProductAttributeValueDto>> GetListProductAttributeAllAsync(Guid productId);

        Task<PagedResult<ProductAttributeValueDto>> GetListFilterAttributeAsync(ProductAttributeListFilterDto input);

        Task<ProductDto> GetBySlugAsync(string slug);


    }
}
