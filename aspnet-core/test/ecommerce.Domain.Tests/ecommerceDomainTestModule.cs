using Volo.Abp.Modularity;

namespace ecommerce;

[DependsOn(
    typeof(ecommerceDomainModule),
    typeof(ecommerceTestBaseModule)
)]
public class ecommerceDomainTestModule : AbpModule
{

}
