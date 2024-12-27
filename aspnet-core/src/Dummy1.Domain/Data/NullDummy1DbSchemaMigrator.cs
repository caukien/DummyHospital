using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Dummy1.Data;

/* This is used if database provider does't define
 * IDummy1DbSchemaMigrator implementation.
 */
public class NullDummy1DbSchemaMigrator : IDummy1DbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
