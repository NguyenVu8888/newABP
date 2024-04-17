using ecommerce.Admin.Catalog.ProductAttributes;
using ecommerce.Admin.Permissions;
using ecommerce.Attributes;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ecommerce.Admin.Catalogs.ProductAttributes
{

    [Authorize(ecommercePermissions.ProductAttribute.Default, Policy = "AdminOnly")]

    public class ProductAttributeAppService : CrudAppService
        <ProductAttribute,
        ProductAttributeDto,
        Guid,
        PagedResultRequestDto,
        CreateUpdateProductAttributeDto,
        CreateUpdateProductAttributeDto>, IProductAttributeAppService
    {

        public ProductAttributeAppService(IRepository<ProductAttribute, Guid> repository
            ) : base(repository)
        {


            GetPolicyName = ecommercePermissions.ProductAttribute.Default;
            GetListPolicyName = ecommercePermissions.ProductAttribute.Default;
            CreatePolicyName = ecommercePermissions.ProductAttribute.Create;
            UpdatePolicyName = ecommercePermissions.ProductAttribute.Update;
            DeletePolicyName = ecommercePermissions.ProductAttribute.Delete;
        }

        [Authorize(ecommercePermissions.ProductAttribute.Delete)]

        public async Task DeleteMultipleAsync(IEnumerable<Guid> ids)
        {
            await Repository.DeleteManyAsync(ids);
            await UnitOfWorkManager.Current.SaveChangesAsync();

        }


        [Authorize(ecommercePermissions.ProductAttribute.Default)]

        public async Task<List<ProductAttributeInListDto>> GetListAllAsync()
        {
            var query = await Repository.GetQueryableAsync();
            query = query.Where(x => x.IsActive == true);
            var data = await AsyncExecuter.ToListAsync(query);

            return ObjectMapper.Map<List<ProductAttribute>, List<ProductAttributeInListDto>>(data);

        }


        [Authorize(ecommercePermissions.ProductAttribute.Default)]

        public async Task<PagedResultDto<ProductAttributeInListDto>> GetListFilterAsync(BaseListFilterDto input)
        {
            var query = await Repository.GetQueryableAsync();
            query = query.WhereIf(!string.IsNullOrWhiteSpace(input.keyword), x => x.Label.Contains(input.keyword));

            var count = await AsyncExecuter.LongCountAsync(query);
            var data = await AsyncExecuter.ToListAsync(query.Skip(input.SkipCount).Take(input.MaxResultCount));

            return new PagedResultDto<ProductAttributeInListDto>(count, ObjectMapper.Map<List<ProductAttribute>, List<ProductAttributeInListDto>>(data));
        }
    }
}
