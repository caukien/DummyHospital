using Dummy1.Area;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dummy1.ProvinceDtos
{
    public class CreateUpdateProvinceDto
    {
        [Required(ErrorMessage = "Code is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Code must be a positive integer.")]
        public int Code { get; set; }
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Province name must be between 1 and 100 characters.")]
        public string ProvinceName { get; set; } = string.Empty;

        public string ProvinceEnName { get; set; } = string.Empty;

        [Required(ErrorMessage = " Code is required.")]
        public ProvinceLevel ProvinceLevel { get; set; } = ProvinceLevel.Undefined;
    }
}
