using Dummy1.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Dummy1.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(Dummy1EntityFrameworkCoreModule),
    typeof(Dummy1ApplicationContractsModule)
    )]
public class Dummy1DbMigratorModule : AbpModule
{
}
