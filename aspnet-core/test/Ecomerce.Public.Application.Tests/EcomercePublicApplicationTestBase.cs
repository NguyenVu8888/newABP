using ecommerce;
using Volo.Abp.Modularity;

namespace Ecomerce.Public;

public abstract class EcomercePublicApplicationTestBase<TStartupModule> : ecommerceTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
