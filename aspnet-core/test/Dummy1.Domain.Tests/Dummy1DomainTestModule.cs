using Volo.Abp.Modularity;

namespace Dummy1;

[DependsOn(
    typeof(Dummy1DomainModule),
    typeof(Dummy1TestBaseModule)
)]
public class Dummy1DomainTestModule : AbpModule
{

}
