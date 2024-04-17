using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace Ecomerce.Public.Web;

[Dependency(ReplaceServices = true)]
public class EcomercePublicBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "Public";
}
