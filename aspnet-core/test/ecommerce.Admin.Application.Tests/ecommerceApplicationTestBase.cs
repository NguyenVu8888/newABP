using Volo.Abp.Modularity;

namespace ecommerce.Admin;

public abstract class ecommerceApplicationTestBase<TStartupModule> : ecommerceTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
