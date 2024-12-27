using Dummy1.Helper;
using Dummy1.HospitalDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Dummy1.Interfaces
{
    public interface IHospitalService : ICrudAppService<
        HospitalDto,
        Guid,
        QueryObject, 
        CreateUpdateHospitalDto
        >
    {

    }
}
