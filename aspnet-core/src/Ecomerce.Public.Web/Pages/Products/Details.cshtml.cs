using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Ecommerce.Public.Catalogs.ProductCategories;
using Ecommerce.Public.Catalogs.Products;
using System;

namespace Ecomerce.Public.Web.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private readonly IProductsAppService _productsAppService;
        private readonly IProductCategoriesAppService _productCategoriesAppService;
        public DetailsModel(IProductsAppService productsAppService,
            IProductCategoriesAppService productCategoriesAppService)
        {
            _productsAppService = productsAppService;
            _productCategoriesAppService = productCategoriesAppService;
        }
        public ProductCategoryDto Category { get; set; }
        public ProductDto Product { get; set; }
        public async Task OnGet(Guid categorySlug, string slug)
        {
            Category = await _productCategoriesAppService.GetAsync(categorySlug);
            Product = await _productsAppService.GetBySlugAsync(slug);
        }
    }
}
