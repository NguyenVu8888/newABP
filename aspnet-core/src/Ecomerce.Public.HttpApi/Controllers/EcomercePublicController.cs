using ecommerce.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Ecomerce.Public.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class EcomercePublicController : AbpControllerBase
{
    protected EcomercePublicController()
    {
        LocalizationResource = typeof(ecommerceResource);
    }
}
