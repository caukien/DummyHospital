using Microsoft.Extensions.Localization;
using Dummy1.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Dummy1;

[Dependency(ReplaceServices = true)]
public class Dummy1BrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<Dummy1Resource> _localizer;

    public Dummy1BrandingProvider(IStringLocalizer<Dummy1Resource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
