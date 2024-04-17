using ecommerce.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Ecomerce.Public.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class PublicPageModel : AbpPageModel
{
    protected PublicPageModel()
    {
        LocalizationResourceType = typeof(ecommerceResource);
    }
}
