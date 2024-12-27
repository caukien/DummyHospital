using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace Dummy1.Model
{
    public class UserHospital : Entity<Guid>
    {
        public Guid UserId { get; set; }
        public Guid HospitalId { get; set; }
    }
}
