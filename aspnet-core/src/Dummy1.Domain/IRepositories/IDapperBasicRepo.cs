using Dummy1.Helper;
using Dummy1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;

namespace Dummy1.IRepositories
{
    public interface IDapperBasicRepo<T> where T : class
    {
        Task<List<T>> GetAll(QueryObject input);
        Task<T> GetOne(Guid id);
        Task<int> GetTotalCount();
    }
}
