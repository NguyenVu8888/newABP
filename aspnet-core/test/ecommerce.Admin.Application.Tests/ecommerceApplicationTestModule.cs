using Volo.Abp.Modularity;

namespace ecommerce.Admin;

[DependsOn(
    typeof(ecommerceAdminApplicationModule),
    typeof(ecommerceDomainTestModule)
)]
public class ecommerceApplicationTestModule : AbpModule
{

}
