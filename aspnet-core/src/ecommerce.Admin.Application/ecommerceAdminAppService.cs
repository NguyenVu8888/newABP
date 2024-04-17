using System;
using System.Collections.Generic;
using System.Text;
using ecommerce.Localization;
using Volo.Abp.Application.Services;

namespace ecommerce.Admin;

/* Inherit your application services from this class.
 */
public abstract class ecommerceAdminAppService : ApplicationService
{
    protected ecommerceAdminAppService()
    {
        LocalizationResource = typeof(ecommerceResource);
    }
}
