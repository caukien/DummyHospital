using Dummy1.Area;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Dummy1.DistrictDtos
{
    public class CreateUpdateDistrictDto
    {
        [Required(ErrorMessage = "District code is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "District code must be a positive integer.")]
        public int Code { get; set; }

        [Required(ErrorMessage = "District name is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "District name must be between 1 and 100 characters.")]
        public string DistrictName { get; set; } = string.Empty;
        public string DistrictEnName { get; set; } = string.Empty ;

        [Required(ErrorMessage = "District level is required.")]
        public DistrictLevel DistrictLevel { get; set; } = DistrictLevel.Undefined;

        [Required(ErrorMessage = "Province code is required.")]
        public int ProvinceCode { get; set; }
    }
}
