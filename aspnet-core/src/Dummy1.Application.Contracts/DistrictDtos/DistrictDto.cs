using Dummy1.Area;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Dummy1.DistrictDtos
{
    public class DistrictDto : FullAuditedEntityDto<Guid>
    {
        public int Code { get; set; }
        public string DistrictName { get; set; }
        public string DistrictEnName { get; set; }
        public DistrictLevel DistrictLevel { get; set; }
        public int ProvinceCode { get; set; }
    }
}
