using Dummy1.Area;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Dummy1.Model
{
    public class Commune : FullAuditedAggregateRoot<Guid>
    {
        public int CommuneCode { get; set; }
        public string CommuneName { get; set; }
        public string CommuneEnName { get; set; }
        public CommuneLevel CommuneLevel { get; set; }
        public int DistrictCode { get; set; }
    }
}
