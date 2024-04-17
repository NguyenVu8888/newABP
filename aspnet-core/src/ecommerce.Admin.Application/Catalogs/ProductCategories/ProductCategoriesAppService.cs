using ecommerce.Admin.Catalog.ProductCategories;
using ecommerce.Admin.Permissions;
using ecommerce.Data;
using ecommerce.EntityFrameworkCore;
using ecommerce.ProductCategories;
using Microsoft.AspNetCore.Authorization;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ecommerce.Admin.Catalogs.ProductCategories
{

    [Authorize(ecommercePermissions.ProductCategory.Default, Policy = "AdminOnly")]


    public class ProductCategoriesAppService : CrudAppService
        <ProductCategory,
        ProductCategoryDto,
        Guid,
        PagedResultRequestDto,
        CreateUpdateProductCategoryDto,
        CreateUpdateProductCategoryDto>, IProductCategoryAppService
    {
        private readonly ecommerceDbContext _dbContext;
        public ProductCategoriesAppService(IRepository<ProductCategory, Guid> repository, ecommerceDbContext context
            ) : base(repository)
        {
            _dbContext = context;


            GetPolicyName = ecommercePermissions.ProductCategory.Default;
            GetListPolicyName = ecommercePermissions.ProductCategory.Default;
            CreatePolicyName = ecommercePermissions.ProductCategory.Create;
            UpdatePolicyName = ecommercePermissions.ProductCategory.Update;
            DeletePolicyName = ecommercePermissions.ProductCategory.Delete;
        }


        [Authorize(ecommercePermissions.ProductCategory.Delete)]

        public async Task DeleteMultipleAsync(IEnumerable<Guid> ids)
        {
            await Repository.DeleteManyAsync(ids);
            await UnitOfWorkManager.Current.SaveChangesAsync();

        }

        [Authorize(ecommercePermissions.ProductCategory.Default)]

        public async Task<List<ProductCategoryInListDto>> GetListAllAsync()
        {
            /*var query = await Repository.GetQueryableAsync();
            query = query.Where(x => x.IsActive == true);
            var data = await AsyncExecuter.ToListAsync(query);

            return ObjectMapper.Map<List<ProductCategory>, List<ProductCategoryInListDto>>(data);*/



            var query = from cate in _dbContext.ProductCategorys
                        select cate;

            List<ProductCategoryInListDto> listResult = new List<ProductCategoryInListDto>();

            foreach (var productCategory in query)
            {
                ProductCategoryInListDto category = new ProductCategoryInListDto();
                category.Id = productCategory.Id;
                category.Name = productCategory.Name;
                category.CoverPucture = productCategory.CoverPucture;
                category.Code = productCategory.Code;
                category.IsActive = productCategory.IsActive;
                category.SortOrder = productCategory.SortOrder;
                category.Visibility = productCategory.Visibility;
                listResult.Add(category);
            }
            return listResult;
        }


        [Authorize(ecommercePermissions.ProductCategory.Default)]

        public async Task<PagedResultDto<ProductCategoryInListDto>> GetListFilterAsync(BaseListFilterDto input)
        {
            var query = await Repository.GetQueryableAsync();
            query = query.WhereIf(!string.IsNullOrWhiteSpace(input.keyword), x => x.Name.Contains(input.keyword));

            var count = await AsyncExecuter.LongCountAsync(query);
            var data = await AsyncExecuter.ToListAsync(query.Skip(input.SkipCount).Take(input.MaxResultCount));

            return new PagedResultDto<ProductCategoryInListDto>(count, ObjectMapper.Map<List<ProductCategory>, List<ProductCategoryInListDto>>(data));
        }
    }
}
