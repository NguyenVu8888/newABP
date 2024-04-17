using ecommerce.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace ecommerce.Admin.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class ecommerceController : AbpControllerBase
{
    protected ecommerceController()
    {
        LocalizationResource = typeof(ecommerceResource);
    }
}
