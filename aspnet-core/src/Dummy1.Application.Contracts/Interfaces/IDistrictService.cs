using Dummy1.DistrictDtos;
using Dummy1.Helper;
using Dummy1.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Dummy1.Interfaces
{
    public interface IDistrictService : ICrudAppService<DistrictDto, Guid, QueryObject, CreateUpdateDistrictDto>
    {
        Task ImportFile(IFormFile file);
        Task<List<DistrictDto>> GetDistrictByProvinceCode(int provinceCode);
    }
}
