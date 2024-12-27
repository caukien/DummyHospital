using Dummy1.Samples;
using Xunit;

namespace Dummy1.EntityFrameworkCore.Domains;

[Collection(Dummy1TestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<Dummy1EntityFrameworkCoreTestModule>
{

}
