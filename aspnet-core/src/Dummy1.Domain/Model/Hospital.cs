using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Dummy1.Model
{
    public class Hospital : FullAuditedAggregateRoot<Guid>
    {
        public int HospitalCode { get; set; }
        public string HospitalName { get; set; }
        public int ProvinceCode { get; set; }
        public int DistrictCode { get; set; }
        public int CommuneCode { get; set; }
        public string Address { get; set; }
        public Guid TenantId { get; set; }
    }
}
