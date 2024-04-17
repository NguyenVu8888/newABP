using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace ecommerce;

[Dependency(ReplaceServices = true)]
public class ecommerceBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "ecommerce";
}
