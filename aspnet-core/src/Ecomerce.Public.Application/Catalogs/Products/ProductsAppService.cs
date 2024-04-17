using Ecommerce.Public.Catalogs.Products.Attributes;
using ecommerce.Attributes;
using ecommerce.ProductCategories;
using ecommerce.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Ecomerce.Public;


namespace Ecommerce.Public.Catalogs.Products
{

    public class ProductsAppService : ReadOnlyAppService
        <Product,
        ProductDto,
        Guid,
        PagedResultRequestDto>, IProductsAppService
    {
        private readonly IBlobContainer<ProductThumbnailPictureContainer> _filelContainer;
        private readonly ProductCodeGenerator _productCodeGenerator;
        private readonly IRepository<ProductAttribute> _ProductAttribute;
        private readonly IRepository<ProductAttributeDateTime> _ProductAttributeDateTime;
        private readonly IRepository<ProductAttributeInt> _ProductAttributeInt;
        private readonly IRepository<ProductAttributeDecimal> _ProductAttributeDecimal;
        private readonly IRepository<ProductAttributeText> _ProductAttributeText;
        private readonly IRepository<ProductAttributeVarchar> _ProductAttributeVarchar;

        private readonly IRepository<Product, Guid> _productRepository;

        public ProductsAppService(IRepository<Product, Guid> repository,
            IRepository<ProductCategory> productCategoryRepository,
            ProductManager productManager,
            IBlobContainer<ProductThumbnailPictureContainer> fileContainer,
            ProductCodeGenerator productCodeGenerator,
            IRepository<ProductAttribute> ProductAttribute,
            IRepository<ProductAttributeDateTime> ProductAttributeDateTime,
            IRepository<ProductAttributeInt> ProductAttributeInt,
             IRepository<ProductAttributeDecimal> ProductAttributeDecimal,
             IRepository<ProductAttributeText> ProductAttributeText,
             IRepository<ProductAttributeVarchar> ProductAttributeVarchar,

             IRepository<Product, Guid> productRepository

            ) : base(repository)
        {
            _filelContainer = fileContainer;
            _productCodeGenerator = productCodeGenerator;
            _ProductAttribute = ProductAttribute;
            _ProductAttributeDateTime = ProductAttributeDateTime;
            _ProductAttributeInt = ProductAttributeInt;
            _ProductAttributeDecimal = ProductAttributeDecimal;
            _ProductAttributeText = ProductAttributeText;
            _ProductAttributeVarchar = ProductAttributeVarchar;
            _productRepository = productRepository;
        }

        public async Task<List<ProductInListDto>> GetListAllAsync()
        {
            var query = await Repository.GetQueryableAsync();
            /*  query = query.Where(x => x.IsActive == true);*/

            var data = await AsyncExecuter.ToListAsync(query);

            return ObjectMapper.Map<List<Product>, List<ProductInListDto>>(data);

        }

        public async Task<PagedResult<ProductInListDto>> GetListFilterAsync(ProductListFilterDto input)
        {
            var query = await Repository.GetQueryableAsync();
            query = query.WhereIf(!string.IsNullOrWhiteSpace(input.keyword), p => p.Name.Contains(input.keyword));
            query = query.WhereIf(input.CategoryId.HasValue, p => p.CategoryId == input.CategoryId);

            var totalCount = await AsyncExecuter.LongCountAsync(query);
            var data = await AsyncExecuter
            .ToListAsync(
               query.Skip((input.CurrentPage - 1) * input.PageSize)
            .Take(input.PageSize));

            return new PagedResult<ProductInListDto>(
                ObjectMapper.Map<List<Product>, List<ProductInListDto>>(data),
                totalCount,
                input.CurrentPage,
                input.PageSize
            );
        }

        public async Task<string> GetThumnailImageAsync(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return null;
            }

            var ThumnailContent = await _filelContainer.GetAllBytesOrNullAsync(fileName);

            if (ThumnailContent is null)
            {
                return null;
            }

            var result = Convert.ToBase64String(ThumnailContent);
            return result;



        }

        public async Task<string> GetSuggestNewCodeAsync()
        {
            return await _productCodeGenerator.GenerateAsync();
        }

        public async Task<List<ProductAttributeValueDto>> GetListProductAttributeAllAsync(Guid productId)
        {
            var attributeQuery = await _ProductAttribute.GetQueryableAsync();

            var attributeDateTimeQuery = await _ProductAttributeDateTime.GetQueryableAsync();
            var attributeDecimalQuery = await _ProductAttributeDecimal.GetQueryableAsync();
            var attributeIntQuery = await _ProductAttributeInt.GetQueryableAsync();
            var attributeVarcharQuery = await _ProductAttributeVarchar.GetQueryableAsync();
            var attributeTextQuery = await _ProductAttributeText.GetQueryableAsync();

            var query = from a in attributeQuery
                        join aDate in attributeDateTimeQuery on a.Id equals aDate.AttributeId into joinDate
                        from aDate in joinDate.DefaultIfEmpty()
                        join aDecimal in attributeDecimalQuery on a.Id equals aDecimal.AttributeId into joinDecimal
                        from aDecimal in joinDecimal.DefaultIfEmpty()
                        join aInt in attributeIntQuery on a.Id equals aInt.AttributeId into joinInt
                        from aInt in joinInt.DefaultIfEmpty()
                        join aVarchar in attributeVarcharQuery on a.Id equals aVarchar.AttributeId into joinVarchar
                        from aVarchar in joinVarchar.DefaultIfEmpty()
                        join aText in attributeTextQuery on a.Id equals aText.AttributeId into joinText
                        from aText in joinText.DefaultIfEmpty()
                            /* where (aDate != null || aDate.ProductId == productId)
                             && (aDecimal != null || aDecimal.ProductId == productId)
                             && (aInt != null || aInt.ProductId == productId)
                             && (aVarchar != null || aVarchar.ProductId == productId)
                             && (aText != null || aText.ProductId == productId)*/
                        where aDate.ProductId == productId
                        || aDecimal.ProductId == productId
                        || aInt.ProductId == productId
                        || aVarchar.ProductId == productId
                        || aText.ProductId == productId

                        select new ProductAttributeValueDto
                        {
                            Label = a.Label,
                            AttributeId = a.Id,
                            type = a.Type,
                            Code = a.Code,
                            ProductId = productId,
                            DateTimeValue = aDate != null ? aDate.Value : null,
                            DecimalValue = aDecimal != null ? aDecimal.Value : null,
                            IntValue = aInt != null ? aInt.Value : 0,
                            TextValue = aText != null ? aText.Value : null,
                            VarcharValue = aVarchar != null ? aVarchar.Value : null,
                            DateTimeId = aDate != null ? aDate.Id : null,
                            DecimalId = aDecimal != null ? aDecimal.Id : null,
                            IntId = aInt != null ? aInt.Id : null,
                            TextId = aText != null ? aText.Id : null,
                            VarcharId = aVarchar != null ? aVarchar.Id : null,
                        };
            query = query.Where(x => x.DateTimeId != null
                           || x.DecimalId != null
                           || x.IntValue != null
                           || x.TextId != null
                           || x.VarcharId != null);
            return await AsyncExecuter.ToListAsync(query);

        }

        public async Task<PagedResult<ProductAttributeValueDto>> GetListFilterAttributeAsync(ProductAttributeListFilterDto input)
        {
            var attributeQuery = await _ProductAttribute.GetQueryableAsync();

            var attributeDateTimeQuery = await _ProductAttributeDateTime.GetQueryableAsync();
            var attributeDecimalQuery = await _ProductAttributeDecimal.GetQueryableAsync();
            var attributeIntQuery = await _ProductAttributeInt.GetQueryableAsync();
            var attributeVarcharQuery = await _ProductAttributeVarchar.GetQueryableAsync();
            var attributeTextQuery = await _ProductAttributeText.GetQueryableAsync();

            var query = from a in attributeQuery
                        join aDate in attributeDateTimeQuery on a.Id equals aDate.AttributeId into joinDate
                        from aDate in joinDate.DefaultIfEmpty()
                        join aDecimal in attributeDecimalQuery on a.Id equals aDecimal.AttributeId into joinDecimal
                        from aDecimal in joinDecimal.DefaultIfEmpty()
                        join aInt in attributeIntQuery on a.Id equals aInt.AttributeId into joinInt
                        from aInt in joinInt.DefaultIfEmpty()
                        join aVarchar in attributeVarcharQuery on a.Id equals aVarchar.AttributeId into joinVarchar
                        from aVarchar in joinVarchar.DefaultIfEmpty()
                        join aText in attributeTextQuery on a.Id equals aText.AttributeId into joinText
                        from aText in joinText.DefaultIfEmpty()
                            /* where (aDate != null || aDate.ProductId == input.ProductId)
                             && (aDecimal != null || aDecimal.ProductId == input.ProductId)
                             && (aInt != null || aInt.ProductId == input.ProductId)
                             && (aVarchar != null || aVarchar.ProductId == input.ProductId)
                             && (aText != null || aText.ProductId == input.ProductId)*/
                        where aDate.ProductId == input.ProductId
                        || aDecimal.ProductId == input.ProductId
                        || aInt.ProductId == input.ProductId
                        || aVarchar.ProductId == input.ProductId
                        || aText.ProductId == input.ProductId
                        select new ProductAttributeValueDto
                        {
                            Label = a.Label,
                            AttributeId = a.Id,
                            type = a.Type,
                            Code = a.Code,
                            ProductId = input.ProductId,
                            DateTimeValue = aDate != null ? aDate.Value : null,
                            DecimalValue = aDecimal != null ? aDecimal.Value : null,
                            IntValue = aInt != null ? aInt.Value : 0,
                            TextValue = aText != null ? aText.Value : null,
                            VarcharValue = aVarchar != null ? aVarchar.Value : null,
                            DateTimeId = aDate != null ? aDate.Id : null,
                            DecimalId = aDecimal != null ? aDecimal.Id : null,
                            IntId = aInt != null ? aInt.Id : null,
                            TextId = aText != null ? aText.Id : null,
                            VarcharId = aVarchar != null ? aVarchar.Id : null,
                        };
            query = query.Where(x => x.DateTimeId != null
            || x.DecimalId != null
            || x.IntValue != null
            || x.TextId != null
            || x.VarcharId != null);
            var totalCount = await AsyncExecuter.LongCountAsync(query);
            var data = await AsyncExecuter
             .ToListAsync(
                query.Skip((input.CurrentPage - 1) * input.PageSize)
             .Take(input.PageSize));

            return new PagedResult<ProductAttributeValueDto>(data,
                totalCount,
                input.CurrentPage,
                input.PageSize
            );

           
        }


        public async Task<ProductDto> GetBySlugAsync(string slug)
        {
            var product = await _productRepository.GetAsync(x => x.Code == slug);
            return ObjectMapper.Map<Product, ProductDto>(product);
        }


    }
}
