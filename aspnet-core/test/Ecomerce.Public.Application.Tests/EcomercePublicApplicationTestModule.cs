using ecommerce;
using Volo.Abp.Modularity;

namespace Ecomerce.Public;

[DependsOn(
    typeof(EcomercePublicApplicationModule),
    typeof(ecommerceDomainTestModule)
)]
public class EcomercePublicApplicationTestModule : AbpModule
{

}
