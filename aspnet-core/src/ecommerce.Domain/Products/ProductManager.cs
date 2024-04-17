using ecommerce.ProductCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace ecommerce.Products
{
    public class ProductManager :DomainService
    {
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly IRepository<ProductCategory, Guid> _ProductCategoryRepository;
        public ProductManager(IRepository<Product,Guid> productRepository,
            IRepository<ProductCategory, Guid> productCategoryRepository) {
        
            _productRepository = productRepository;
            _ProductCategoryRepository = productCategoryRepository;
        
        }

        public async Task<Product> CreateAsync(
            Guid manufictureId,
            string? name,
            string? code,
            ProductType productType,
            string? sKU,
            int sortOrder,
            bool visibility,
            bool isActive,
            Guid categoryId,
            string? seoMetaDescription,
            string? descreption
            )
        {

            if(await _productRepository.AnyAsync(x => x.Name == name))
            {
                throw new UserFriendlyException("ten san pham da ton tai",ecommerceDomainErrorCodes.ProductNameAlreadyExists);
            }
            if (await _productRepository.AnyAsync(x => x.Code == code))
            {
                throw new UserFriendlyException("code pham da ton tai",ecommerceDomainErrorCodes.ProductCodeAlreadyExists);
            }
            if (await _productRepository.AnyAsync(x => x.SKU == sKU))
            {
                throw new UserFriendlyException("sku san pham da ton tai",ecommerceDomainErrorCodes.ProductSkuAlreadyExists);
            }


            return new Product(Guid.NewGuid(), manufictureId, name, code, productType, sKU, sortOrder, visibility,
                isActive, categoryId, seoMetaDescription, descreption, null);

        }
    }
}
