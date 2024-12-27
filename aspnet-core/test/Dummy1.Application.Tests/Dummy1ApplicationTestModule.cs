using Volo.Abp.Modularity;

namespace Dummy1;

[DependsOn(
    typeof(Dummy1ApplicationModule),
    typeof(Dummy1DomainTestModule)
)]
public class Dummy1ApplicationTestModule : AbpModule
{

}
