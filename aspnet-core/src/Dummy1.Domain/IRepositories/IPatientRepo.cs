using Dummy1.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Dummy1.IRepositories
{
    public interface IPatientRepo<T> : IDapperBasicRepo<T> where T : class
    {
        Task<List<T>> GetPatientByHospitalCode(QueryObject input, Guid hospitalCode);
    }
}
