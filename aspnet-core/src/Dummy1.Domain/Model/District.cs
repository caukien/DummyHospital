using Dummy1.Area;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Dummy1.Model
{
    public class District : FullAuditedAggregateRoot<Guid>
    {
        public int Code { get; set; }
        public string DistrictName { get; set; }
        public string DistrictEnName { get; set; }
        public DistrictLevel Districtlevel { get; set; }
        public int ProvinceCode { get; set; }
    }
}
