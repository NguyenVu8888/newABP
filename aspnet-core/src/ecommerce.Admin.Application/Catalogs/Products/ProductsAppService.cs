using ecommerce.Admin.Catalog.Products;
using ecommerce.Admin.Catalog.Products.Attributes;
using ecommerce.Admin.Catalog.ProductAttributes;
using ecommerce.Attributes;
using ecommerce.Attributies;
using ecommerce.ProductCategories;
using ecommerce.Products;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Repositories;
using ecommerce.Admin.Permissions;

namespace ecommerce.Admin.Catalogs.Products
{
    [Authorize(ecommercePermissions.Product.Default,Policy = "AdminOnly")]
    public class ProductsAppService : CrudAppService
        <Product,
        ProductDto,
        Guid,
        PagedResultRequestDto,
        CreateUpdateProductDto,
        CreateUpdateProductDto>, IProductsAppService
    {

        private readonly ProductManager _productManager;
        private readonly IRepository<ProductCategory> _productCategoryRepository;
        private readonly IBlobContainer<ProductThumbnailPictureContainer> _filelContainer;
        private readonly ProductCodeGenerator _productCodeGenerator;
        private readonly IRepository<ProductAttribute> _ProductAttribute;
        private readonly IRepository<ProductAttributeDateTime> _ProductAttributeDateTime;
        private readonly IRepository<ProductAttributeInt> _ProductAttributeInt;
        private readonly IRepository<ProductAttributeDecimal> _ProductAttributeDecimal;
        private readonly IRepository<ProductAttributeText> _ProductAttributeText;
        private readonly IRepository<ProductAttributeVarchar> _ProductAttributeVarchar;
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
             IRepository<ProductAttributeVarchar> ProductAttributeVarchar

            ) : base(repository)
        {
            _productManager = productManager;
            _productCategoryRepository = productCategoryRepository;
            _filelContainer = fileContainer;
            _productCodeGenerator = productCodeGenerator;
            _ProductAttribute = ProductAttribute;
            _ProductAttributeDateTime = ProductAttributeDateTime;
            _ProductAttributeInt = ProductAttributeInt;
            _ProductAttributeDecimal = ProductAttributeDecimal;
            _ProductAttributeText = ProductAttributeText;
            _ProductAttributeVarchar = ProductAttributeVarchar;




            GetPolicyName = ecommercePermissions.Product.Default;
            GetListPolicyName = ecommercePermissions.Product.Default;
            CreatePolicyName = ecommercePermissions.Product.Create;
            UpdatePolicyName = ecommercePermissions.Product.Update;
            DeletePolicyName = ecommercePermissions.Product.Delete;
        }



        [Authorize(ecommercePermissions.Product.Create)]

        public override async Task<ProductDto> CreateAsync(CreateUpdateProductDto input)
        {
            var product = await _productManager.CreateAsync(input.ManufictureId, input.Name, input.Code, input.ProductType, input.SKU,
                input.SortOrder, input.Visibility, input.IsActive, input.CategoryId, input.SeoMetaDescription, input.Descreption);


            if (input.ThumbnailPictureContent != null && input.ThumbnailPictureContent.Length > 0)
            {

                await SaveThumnailImageAsync(input.ThumbnailPictureName, input.ThumbnailPictureContent);
                product.ThumbnailPicture = input.ThumbnailPictureName;
            }

            var result = await Repository.InsertAsync(product);

            return ObjectMapper.Map<Product, ProductDto>(result);
        }



        [Authorize(ecommercePermissions.Product.Update)]

        public override async Task<ProductDto> UpdateAsync(Guid id, CreateUpdateProductDto input)
        {
            var product = await Repository.GetAsync(id);
            if (product == null)
                throw new BusinessException(ecommerceDomainErrorCodes.ProductIsNotExists);
            product.ManufictureId = input.ManufictureId;
            product.Name = input.Name;
            product.Code = input.Code;
            product.ProductType = input.ProductType;
            product.SKU = input.SKU;
            product.SortOrder = input.SortOrder;
            product.Visibility = input.Visibility;
            product.IsActive = input.IsActive;

            product.CategoryId = input.CategoryId;

            /*if (product.CategoryId != input.CategoryId)
            {
                product.CategoryId = input.CategoryId;
                var category = await _productCategoryRepository.GetAsync(x => x.Id == input.CategoryId);
                product.CategoryName = category.Name;
                product.CategorySlug = category.Slug;
            }*/
            product.SeoMetaDescription = input.SeoMetaDescription;
            product.Descreption = input.Descreption;
            if (input.ThumbnailPictureContent != null && input.ThumbnailPictureContent.Length > 0)
            {

                await SaveThumnailImageAsync(input.ThumbnailPictureName, input.ThumbnailPictureContent);
                product.ThumbnailPicture = input.ThumbnailPictureName;

            }
            /* product.SellPrice = input.SellPrice;*/
            await Repository.UpdateAsync(product);

            return ObjectMapper.Map<Product, ProductDto>(product);
        }

        
        [Authorize(ecommercePermissions.Product.Delete)]

        public async Task DeleteMultipleAsync(IEnumerable<Guid> ids)
        {
            await Repository.DeleteManyAsync(ids);
            await UnitOfWorkManager.Current.SaveChangesAsync();
        }


        [Authorize(ecommercePermissions.Product.Default)]

        public async Task<List<ProductInListDto>> GetListAllAsync()
        {
            var query = await Repository.GetQueryableAsync();
            /*  query = query.Where(x => x.IsActive == true);*/

            var data = await AsyncExecuter.ToListAsync(query);

            return ObjectMapper.Map<List<Product>, List<ProductInListDto>>(data);

        }


        [Authorize(ecommercePermissions.Product.Default)]

        public async Task<PagedResultDto<ProductInListDto>> GetListFilterAsync(ProductListFilterDto input)
        {
            var query = await Repository.GetQueryableAsync();
            query = query.WhereIf(!string.IsNullOrWhiteSpace(input.keyword), p => p.Name.Contains(input.keyword));
            query = query.WhereIf(input.CategoryId.HasValue, p => p.CategoryId == input.CategoryId);

            var totalCount = await AsyncExecuter.LongCountAsync(query);
            var data = await AsyncExecuter.ToListAsync(query.OrderByDescending(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount));

            return new PagedResultDto<ProductInListDto>(totalCount, ObjectMapper.Map<List<Product>, List<ProductInListDto>>(data));
        }

        private async Task SaveThumnailImageAsync(string fileName, string base64)
        {
            Regex regex = new Regex(@"^[\w/\:.-]+;base64,");
            base64 = regex.Replace(base64, string.Empty);
            byte[] bytes = Convert.FromBase64String(base64);
            await _filelContainer.SaveAsync(fileName, bytes, overrideExisting: true);
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


        [Authorize(ecommercePermissions.Product.Create)]

        public async Task<ProductAttributeValueDto> addAttibuteAsync(addUpdateProductAttributeDto input)
        {
            var product = await Repository.GetAsync(x => x.Id == input.ProductId);
            if (product is null)
            {
                throw new BusinessException(ecommerceDomainErrorCodes.ProductIsNotExists);
            }

            var attribute = await _ProductAttribute.GetAsync(x => x.Id == input.AttributeId);
            if (attribute is null)
            {
                throw new BusinessException(ecommerceDomainErrorCodes.AttributeIsNotExists);
            }


            var newAttibuteId = Guid.NewGuid();
            switch (attribute.Type)
            {
                case AttributeType.Int:
                    if (input.IntValue == null)
                    {
                        throw new BusinessException(ecommerceDomainErrorCodes.AttributeIsNotValid);
                    }
                    var productAttributeInt = new ProductAttributeInt(newAttibuteId, input.AttributeId, input.ProductId, input.IntValue.Value);
                    await _ProductAttributeInt.InsertAsync(productAttributeInt);

                    break;
                case AttributeType.Decimal:
                    if (input.DecimalValue == null)
                    {
                        throw new BusinessException(ecommerceDomainErrorCodes.AttributeIsNotValid);
                    }
                    var productAttributeDecimal = new ProductAttributeDecimal(newAttibuteId, input.AttributeId, input.ProductId, input.DecimalValue.Value);
                    await _ProductAttributeDecimal.InsertAsync(productAttributeDecimal);
                    break;
                case AttributeType.Date:
                    if (input.DateTimeValue == null)
                    {
                        throw new BusinessException(ecommerceDomainErrorCodes.AttributeIsNotValid);
                    }
                    var productAttributeDatime = new ProductAttributeDateTime(newAttibuteId, input.AttributeId, input.ProductId, input.DateTimeValue);
                    await _ProductAttributeDateTime.InsertAsync(productAttributeDatime);
                    break;
                case AttributeType.Text:
                    if (input.TextValue == null)
                    {
                        throw new BusinessException(ecommerceDomainErrorCodes.AttributeIsNotValid);
                    }
                    var productAttributeText = new ProductAttributeText(newAttibuteId, input.AttributeId, input.ProductId, input.TextValue);
                    await _ProductAttributeText.InsertAsync(productAttributeText);
                    break;
                case AttributeType.Varchar:
                    if (input.VarcharValue == null)
                    {
                        throw new BusinessException(ecommerceDomainErrorCodes.AttributeIsNotValid);
                    }
                    var productAttributeVarchar = new ProductAttributeVarchar(newAttibuteId, input.AttributeId, input.ProductId, input.VarcharValue);
                    await _ProductAttributeVarchar.InsertAsync(productAttributeVarchar);
                    break;
            }

            try
            {
                await UnitOfWorkManager.Current.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return new ProductAttributeValueDto()
            {
                AttributeId = input.AttributeId,
                ProductId = input.ProductId,
                type = attribute.Type,
                Id = newAttibuteId,
                Label = attribute.Label,
                IntValue = input.IntValue,
                DecimalValue = input.DecimalValue,
                DateTimeValue = input.DateTimeValue,
                TextValue = input.TextValue,
                VarcharValue = input.VarcharValue

            };
        }



        [Authorize(ecommercePermissions.Product.Delete)]

        public async Task removeAttributeAsync(Guid attributeId, Guid id)
        {
            var attribute = await _ProductAttribute.GetAsync(x => x.Id == attributeId);
            if (attribute is null)
            {
                throw new BusinessException(ecommerceDomainErrorCodes.AttributeIsNotExists);
            }


            var newAttibuteId = Guid.NewGuid();
            switch (attribute.Type)
            {
                case AttributeType.Int:

                    var productAttributeInt = await _ProductAttributeInt.GetAsync(x => x.Id == id);
                    if (productAttributeInt is null)
                    {
                        throw new BusinessException(ecommerceDomainErrorCodes.AttributeIsNotExists);
                    }

                    await _ProductAttributeInt.DeleteAsync(productAttributeInt);

                    break;
                case AttributeType.Decimal:


                    var productAttributeDecimal = await _ProductAttributeDecimal.GetAsync(x => x.Id == id);
                    if (productAttributeDecimal is null)
                    {
                        throw new BusinessException(ecommerceDomainErrorCodes.AttributeIsNotExists);
                    }

                    await _ProductAttributeDecimal.DeleteAsync(productAttributeDecimal);
                    break;
                case AttributeType.Date:

                    var productAttributeDatime = await _ProductAttributeDateTime.GetAsync(x => x.Id == id);
                    if (productAttributeDatime is null)
                    {
                        throw new BusinessException(ecommerceDomainErrorCodes.AttributeIsNotExists);
                    }



                    await _ProductAttributeDateTime.DeleteAsync(productAttributeDatime);
                    break;
                case AttributeType.Text:

                    var productAttributeText = await _ProductAttributeText.GetAsync(x => x.Id == id);
                    if (productAttributeText is null)
                    {
                        throw new BusinessException(ecommerceDomainErrorCodes.AttributeIsNotExists);
                    }

                    await _ProductAttributeText.DeleteAsync(productAttributeText);
                    break;
                case AttributeType.Varchar:

                    var productAttributeVarchar = await _ProductAttributeVarchar.GetAsync(x => x.Id == id);
                    if (productAttributeVarchar is null)
                    {
                        throw new BusinessException(ecommerceDomainErrorCodes.AttributeIsNotExists);
                    }

                    await _ProductAttributeVarchar.DeleteAsync(productAttributeVarchar);
                    break;
            }

            await UnitOfWorkManager.Current.SaveChangesAsync();
        }



        [Authorize(ecommercePermissions.Product.Default)]

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



        [Authorize(ecommercePermissions.Product.Default)]

        public async Task<PagedResultDto<ProductAttributeValueDto>> GetListFilterAttributeAsync(ProductAttributeListFilterDto input)
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
            var data = await AsyncExecuter.ToListAsync(
                query.OrderByDescending(x => x.Label)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount));

            return new PagedResultDto<ProductAttributeValueDto>(totalCount, data);
        }



        [Authorize(ecommercePermissions.Product.Update)]

        public async Task<ProductAttributeValueDto> updateAttibuteAsync(Guid id, addUpdateProductAttributeDto input)
        {
            var product = await Repository.GetAsync(x => x.Id == input.ProductId);
            if (product is null)
            {
                throw new BusinessException(ecommerceDomainErrorCodes.ProductIsNotExists);
            }

            var attribute = await _ProductAttribute.GetAsync(x => x.Id == input.AttributeId);
            if (attribute is null)
            {
                throw new BusinessException(ecommerceDomainErrorCodes.AttributeIsNotExists);
            }


            var newAttibuteId = Guid.NewGuid();
            switch (attribute.Type)
            {
                case AttributeType.Int:
                    if (input.IntValue == null)
                    {
                        throw new BusinessException(ecommerceDomainErrorCodes.AttributeIsNotValid);
                    }
                    var productAttributeInt = await _ProductAttributeInt.GetAsync(x => x.Id == id);
                    if (productAttributeInt is null)
                    {
                        throw new BusinessException(ecommerceDomainErrorCodes.AttributeIsNotExists);
                    }
                    productAttributeInt.Value = input.IntValue.Value;
                    await _ProductAttributeInt.UpdateAsync(productAttributeInt);

                    break;
                case AttributeType.Decimal:
                    if (input.DecimalValue == null)
                    {
                        throw new BusinessException(ecommerceDomainErrorCodes.AttributeIsNotValid);
                    }

                    var productAttributeDecimal = await _ProductAttributeDecimal.GetAsync(x => x.Id == id);
                    if (productAttributeDecimal is null)
                    {
                        throw new BusinessException(ecommerceDomainErrorCodes.AttributeIsNotExists);
                    }
                    productAttributeDecimal.Value = input.DecimalValue.Value;

                    await _ProductAttributeDecimal.UpdateAsync(productAttributeDecimal);
                    break;
                case AttributeType.Date:
                    if (input.DateTimeValue == null)
                    {
                        throw new BusinessException(ecommerceDomainErrorCodes.AttributeIsNotValid);
                    }
                    var productAttributeDatime = await _ProductAttributeDateTime.GetAsync(x => x.Id == id);
                    if (productAttributeDatime is null)
                    {
                        throw new BusinessException(ecommerceDomainErrorCodes.AttributeIsNotExists);
                    }
                    productAttributeDatime.Value = input.DateTimeValue;


                    await _ProductAttributeDateTime.UpdateAsync(productAttributeDatime);
                    break;
                case AttributeType.Text:
                    if (input.TextValue == null)
                    {
                        throw new BusinessException(ecommerceDomainErrorCodes.AttributeIsNotValid);
                    }
                    var productAttributeText = await _ProductAttributeText.GetAsync(x => x.Id == id);
                    if (productAttributeText is null)
                    {
                        throw new BusinessException(ecommerceDomainErrorCodes.AttributeIsNotExists);
                    }
                    productAttributeText.Value = input.TextValue;
                    await _ProductAttributeText.UpdateAsync(productAttributeText);
                    break;
                case AttributeType.Varchar:
                    if (input.VarcharValue == null)
                    {
                        throw new BusinessException(ecommerceDomainErrorCodes.AttributeIsNotValid);
                    }
                    var productAttributeVarchar = await _ProductAttributeVarchar.GetAsync(x => x.Id == id);
                    if (productAttributeVarchar is null)
                    {
                        throw new BusinessException(ecommerceDomainErrorCodes.AttributeIsNotExists);
                    }
                    productAttributeVarchar.Value = input.VarcharValue;
                    await _ProductAttributeVarchar.UpdateAsync(productAttributeVarchar);
                    break;
            }

            await UnitOfWorkManager.Current.SaveChangesAsync();
            return new ProductAttributeValueDto()
            {
                AttributeId = input.AttributeId,
                ProductId = input.ProductId,
                type = attribute.Type,
                Id = newAttibuteId,
                Label = attribute.Label,
                IntValue = input.IntValue.Value,
                DecimalValue = input.DecimalValue,
                DateTimeValue = input.DateTimeValue,
                TextValue = input.TextValue,
                VarcharValue = input.VarcharValue

            };
        }
    }
}
