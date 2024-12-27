using Xunit;

namespace Dummy1.EntityFrameworkCore;

[CollectionDefinition(Dummy1TestConsts.CollectionDefinitionName)]
public class Dummy1EntityFrameworkCoreCollection : ICollectionFixture<Dummy1EntityFrameworkCoreFixture>
{

}
