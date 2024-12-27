using Volo.Abp.Settings;

namespace Dummy1.Settings;

public class Dummy1SettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(Dummy1Settings.MySetting1));
    }
}
