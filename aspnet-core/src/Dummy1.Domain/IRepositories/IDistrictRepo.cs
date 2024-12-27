using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dummy1.IRepositories
{
    public interface IDistrictRepo<T> : IDapperBasicRepo<T> where T : class
    {
        Task<List<T>> GetDistrictByProvinceCode(int provinceCode);
    }
}
