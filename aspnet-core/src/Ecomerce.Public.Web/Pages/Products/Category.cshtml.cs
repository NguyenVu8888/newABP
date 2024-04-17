using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using Ecommerce.Public.Catalogs.ProductCategories;
using Ecommerce.Public.Catalogs.Products;
using System.Threading.Tasks;
using Ecommerce.Public;



namespace Ecomerce.Public.Web.Pages.Products
{
    public class CategoryModel : PageModel
    {
        private readonly IProductsAppService _productsAppService;
        private readonly IProductCategoriesAppService _productCategoriesAppService;
        


        public List<ProductCategoryInListDto> Categories { set; get; }
        public PagedResult<ProductInListDto> ProductsData { set; get; }

        public ProductCategoryDto Category { set; get; }


        public CategoryModel(IProductCategoriesAppService productCategoriesAppService,
        IProductsAppService productsAppService)
        {
            _productsAppService = productsAppService;
            _productCategoriesAppService = productCategoriesAppService;
        }
        public async Task OnGet(string code, int page = 1)
        {
            Categories = await _productCategoriesAppService.GetListAllAsync();

           Category = await _productCategoriesAppService.GetByCodeAsync(code);
            ProductsData = await _productsAppService.GetListFilterAsync(new ProductListFilterDto() 
            { 
                CurrentPage = page
            });


        }
    }
}
