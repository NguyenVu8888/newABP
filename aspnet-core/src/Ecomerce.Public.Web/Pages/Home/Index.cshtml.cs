using Ecomerce.Public.Web.Models;
using Ecommerce.Public.Catalogs.ProductCategories;
using Ecommerce.Public.Catalogs.Products;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Caching;

namespace Ecomerce.Public.Web.Pages.Home;

public class IndexModel : PublicPageModel
{
    private readonly IDistributedCache<HomeCacheItem> _distributedCache;
    private readonly IProductsAppService _productsAppService;
    private readonly IProductCategoriesAppService _productCategoriesAppService;
    public List<ProductCategoryInListDto> Categories {  set; get; }
    public List<ProductInListDto> Products {  set; get; }

    public IndexModel(
        IProductCategoriesAppService productCategoriesAppService,
        IProductsAppService productsAppService,
        IDistributedCache<HomeCacheItem> distributedCache
        )
    {
        _productCategoriesAppService = productCategoriesAppService;
        _productsAppService = productsAppService;
        _distributedCache = distributedCache;
    }
    public async Task OnGet()
    {

        var cacheItem = await _distributedCache.GetOrAddAsync(EcomercePublicApplicationConstant.CacheKeys.HomeData, async () =>
        {
           var categories = await _productCategoriesAppService.GetListAllAsync();

           var products = await _productsAppService.GetListAllAsync();

            return new HomeCacheItem
            {
                Products = products,
                Categories = categories
            };
        },
        () => new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = System.DateTimeOffset.Now.AddHours(12)
        }
        );

        Categories = cacheItem.Categories;
        Products = cacheItem.Products;


    }
}
