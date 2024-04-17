using Volo.Abp.Settings;

namespace ecommerce.Settings;

public class ecommerceSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(ecommerceSettings.MySetting1));
    }
}
