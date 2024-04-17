using Ecomerce.Public;
using ecommerce.Admin.Catalog.ProductAttributes;
using ecommerce.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace Ecommerce.Public.Catalogs.ProductAttributes
{
    public class ProductAttributeAppService : ReadOnlyAppService
        <ProductAttribute,
        ProductAttributeDto,
        Guid,
        PagedResultRequestDto>, IProductAttributeAppService
    {
        public ProductAttributeAppService(IRepository<ProductAttribute, Guid> repository) : base(repository)
        {
        }

        public async Task<List<ProductAttributeInListDto>> GetListAllAsync()
        {
            var query = await Repository.GetQueryableAsync();
            query = query.Where(x => x.IsActive == true);
            var data = await AsyncExecuter.ToListAsync(query);

            return ObjectMapper.Map<List<ProductAttribute>, List<ProductAttributeInListDto>>(data);

        }

        public async Task<PagedResult<ProductAttributeInListDto>> GetListFilterAsync(BaseListFilterDto input)
        {
            var query = await Repository.GetQueryableAsync();
            query = query.WhereIf(!string.IsNullOrWhiteSpace(input.keyword), x => x.Label.Contains(input.keyword));

            var count = await AsyncExecuter.LongCountAsync(query);
            var data = await AsyncExecuter
            .ToListAsync(
               query.Skip((input.CurrentPage - 1) * input.PageSize)
            .Take(input.PageSize));

            return new PagedResult<ProductAttributeInListDto>(
                ObjectMapper.Map<List<ProductAttribute>, List<ProductAttributeInListDto>>(data),
                count,
                input.CurrentPage,
                input.PageSize
            );
        }
    }
}
