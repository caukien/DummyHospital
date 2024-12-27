using Dummy1.Area;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Dummy1.ProvinceDtos
{
    public class ProvinceDto : FullAuditedEntityDto<Guid>
    {
        public int Code { get; set; }
        public string ProvinceName { get; set; }
        public string ProvinceEnName { get; set; }
        public ProvinceLevel ProvinceLevel { get; set; }
    }
}
