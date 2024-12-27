using Dummy1.CommuneDtos;
using Dummy1.Helper;
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
    public interface ICommuneService : ICrudAppService<
        CommuneDto,
        Guid,
        QueryObject,
        CreateUpdateCommuneDto
        >
    {
        Task ImportFile(IFormFile file);
        Task<List<CommuneDto>> GetCommuneByDistrictCode(int districtCode);
    }
}
