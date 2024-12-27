using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dummy1.PantientDtos
{
    public class CreateUpdatePantientDto
    {
        [Required]
        public string Code { get; set; } = string.Empty;
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public int ProvinceCode { get; set; }
        [Required]
        public int DistrictCode { get; set; }
        [Required]
        public int CommuneCode { get; set; }
        [Required]
        public string Address { get; set; } = string.Empty;
        public Guid HospitalId { get; set; }
    }
}
