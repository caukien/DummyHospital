using System.Threading.Tasks;

namespace Dummy1.Data;

public interface IDummy1DbSchemaMigrator
{
    Task MigrateAsync();
}
