using Volo.Abp.Modularity;

namespace Dummy1;

public abstract class Dummy1ApplicationTestBase<TStartupModule> : Dummy1TestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
