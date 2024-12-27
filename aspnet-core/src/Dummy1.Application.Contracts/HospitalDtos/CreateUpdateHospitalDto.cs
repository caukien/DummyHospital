using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dummy1.HospitalDtos
{
    public class CreateUpdateHospitalDto
    {
        public int HospitalCode { get; set; }
        public string HospitalName { get; set; } = string.Empty;
        public int ProvinceCode { get; set; }
        public int DistrictCode { get; set; }
        public int CommuneCode { get; set; }
        public string Address { get; set; } = string.Empty;
    }
}
