using System;
using System.Collections.Generic;
using System.Text;
using ecommerce.Localization;
using Volo.Abp.Application.Services;

namespace Ecomerce.Public;

/* Inherit your application services from this class.
 */
public abstract class EcomercePublicAppService : ApplicationService
{
    protected EcomercePublicAppService()
    {
        LocalizationResource = typeof(ecommerceResource);
    }
}
