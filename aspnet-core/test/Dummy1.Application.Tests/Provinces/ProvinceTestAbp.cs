using Dummy1.Interfaces;
using Dummy1.ProvinceDtos;
using Dummy1.Service;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Modularity;
using Xunit;

namespace Dummy1.Provinces
{
    public abstract class ProvinceTestAbp<TStartupModule> : Dummy1ApplicationTestBase<TStartupModule>
         where TStartupModule : IAbpModule
    {
        private readonly IProvinceService _provinceService;
        protected ProvinceTestAbp()
        {
            _provinceService = GetRequiredService<IProvinceService>();
        }

        [Fact]
        public async Task Should_Create_Province()
        {
            var province = new CreateUpdateProvinceDto
            {
                Code = 123,
                ProvinceName = "Test Province",
                ProvinceLevel = Area.ProvinceLevel.Tinh,
            };
            var result = await _provinceService.CreateAsync(province);
            result.ShouldNotBeNull();
        }
    }
}
