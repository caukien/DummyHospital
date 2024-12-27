using Dummy1.Area;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Dummy1.Model
{
    public class Province : FullAuditedAggregateRoot<Guid>
    {
        public int Code { get; set; }
        public string ProvinceName { get; set; }
        public string ProvinceEnName { get; set; }
        public ProvinceLevel ProvinceLevel { get; set; }

    }
}
