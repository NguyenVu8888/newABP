using ecommerce.Admin.Catalog.Products.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ecommerce.Admin.Catalog.Products
{
    public interface IProductsAppService : ICrudAppService
        <ProductDto,
        Guid,
        PagedResultRequestDto,
        CreateUpdateProductDto,
        CreateUpdateProductDto>
    {
        Task<PagedResultDto<ProductInListDto>> GetListFilterAsync(ProductListFilterDto input);
        Task<List<ProductInListDto>> GetListAllAsync();
        Task DeleteMultipleAsync(IEnumerable<Guid> ids);

        Task<string> GetThumnailImageAsync(string fileName);

        Task<string> GetSuggestNewCodeAsync();

        Task<ProductAttributeValueDto> addAttibuteAsync(addUpdateProductAttributeDto input);

        Task<ProductAttributeValueDto> updateAttibuteAsync(Guid id, addUpdateProductAttributeDto input);


        Task removeAttributeAsync(Guid attributeId, Guid id);

        Task<List<ProductAttributeValueDto>> GetListProductAttributeAllAsync(Guid productId);

        Task<PagedResultDto<ProductAttributeValueDto>> GetListFilterAttributeAsync(ProductAttributeListFilterDto input);


    }
}
