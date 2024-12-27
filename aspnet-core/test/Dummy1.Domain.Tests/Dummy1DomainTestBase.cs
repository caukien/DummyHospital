using Volo.Abp.Modularity;

namespace Dummy1;

/* Inherit from this class for your domain layer tests. */
public abstract class Dummy1DomainTestBase<TStartupModule> : Dummy1TestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
