using Dummy1.Provinces;
using Dummy1.Samples;
using Xunit;

namespace Dummy1.EntityFrameworkCore.Applications;

[Collection(Dummy1TestConsts.CollectionDefinitionName)]
public class ProvinceTestAbp : ProvinceTestAbp<Dummy1EntityFrameworkCoreTestModule>
{

}
