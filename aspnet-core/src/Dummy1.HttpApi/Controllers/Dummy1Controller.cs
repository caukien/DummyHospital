using Dummy1.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Dummy1.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class Dummy1Controller : AbpControllerBase
{
    protected Dummy1Controller()
    {
        LocalizationResource = typeof(Dummy1Resource);
    }
}
