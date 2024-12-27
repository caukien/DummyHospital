using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Dummy1.Helper
{
    public class QueryObject : PagedResultRequestDto
    {
        public string Sorting { get; set; } = string.Empty;
        public string SearchString { get; set; } = string.Empty;
        public bool IsAscending { get; set; } = true;
    }
}
