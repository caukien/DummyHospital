using Dummy1.Area;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dummy1.CommuneDtos
{
    public class CreateUpdateCommuneDto
    {
        [Required(ErrorMessage = "Commune code is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Commune code must be a positive integer.")]
        public int CommuneCode { get; set; }

        [StringLength(100, MinimumLength = 1, ErrorMessage = "Commune name must be between 1 and 100 characters.")]
        public string CommuneName { get; set; } = string.Empty;

        public string CommuneEnName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Commune level is required.")]
        public CommuneLevel CommuneLevel { get; set; } = CommuneLevel.Undifined;

        [Required(ErrorMessage = "District code is required.")]
        public int DistrictCode { get; set; }
    }
}
