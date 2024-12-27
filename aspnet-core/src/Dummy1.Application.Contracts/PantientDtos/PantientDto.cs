using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Dummy1.PantientDtos
{
    public class PantientDto : FullAuditedAggregateRoot<Guid>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int ProvinceCode { get; set; }
        public int DistrictCode { get; set; }
        public int CommuneCode { get; set; }
        public string Address { get; set; }
        public Guid HospitalId { get; set; }
    }
}
