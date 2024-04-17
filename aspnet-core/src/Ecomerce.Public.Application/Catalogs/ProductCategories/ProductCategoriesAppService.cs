using Ecomerce.Public;
using ecommerce.EntityFrameworkCore;
using ecommerce.ProductCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.AuditLogging;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace Ecommerce.Public.Catalogs.ProductCategories
{
    public class ProductCategoriesAppService : CrudAppService
        <ProductCategory,
        ProductCategoryDto,
        Guid,
        PagedResultRequestDto>, IProductCategoriesAppService
    {

        private readonly ecommerceDbContext _dbContext;
        private readonly IRepository<ProductCategory, Guid> _repository;
        public ProductCategoriesAppService(IRepository<ProductCategory, Guid> repository, ecommerceDbContext context) : base(repository)
        {
            _dbContext = context;
            _repository = repository;
        }

        public async Task<ProductCategoryDto> GetByCodeAsync(string code)
        {
            var query = await _repository.GetQueryableAsync();
            query = query.Where(x => x.Code == code);
            var data = await AsyncExecuter.FirstAsync(query);
            return ObjectMapper.Map<ProductCategory, ProductCategoryDto>(data);
        }

        public async Task<List<ProductCategoryInListDto>> GetListAllAsync()
        {
            var query = await Repository.GetQueryableAsync();
            query = query.Where(x => x.IsActive == true);
            var data = await AsyncExecuter.ToListAsync(query);

            return ObjectMapper.Map<List<ProductCategory>, List<ProductCategoryInListDto>>(data);
        }

        public async Task<PagedResult<ProductCategoryInListDto>> GetListFilterAsync(BaseListFilterDto input)
        {
            var query = await Repository.GetQueryableAsync();
            query = query.WhereIf(!string.IsNullOrWhiteSpace(input.keyword), x => x.Name.Contains(input.keyword));

            var count = await AsyncExecuter.LongCountAsync(query);
            var data = await AsyncExecuter
            .ToListAsync(
               query.Skip((input.CurrentPage - 1) * input.PageSize)
            .Take(input.PageSize));

            return new PagedResult<ProductCategoryInListDto>(
                ObjectMapper.Map<List<ProductCategory>, List<ProductCategoryInListDto>>(data),
                count,
                input.CurrentPage,
                input.PageSize
            );
        }

        /* async Task<IEnumerable<ProductCategoryInListDto>> IProductCategoriesAppService.GetListAllAsync()
         {
             return _dbContext.ProductCategorys.Select(x => new ProductCategoryInListDto 
             { 
                 Name = x.Name,
                 Code = x.Code,
                 SortOrder = x.SortOrder, 
                 CoverPucture = x.CoverPucture,
                 Visibility = x.Visibility, 
                 IsActive = x.IsActive 
             });
         }*/



        public async Task<ProductCategoryDto> GetBySlugAsync(string slug)
        {
            var productCategory = await _repository.GetAsync(x => x.Slug == slug);
            return ObjectMapper.Map<ProductCategory, ProductCategoryDto>(productCategory);
        }

    }
}
