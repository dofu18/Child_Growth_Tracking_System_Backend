using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Enum.GeneralEnum;

namespace DomainLayer.Entities
{
    public class Role : BaseEntity
    {
        public string RoleName { get; set; }
        public string Description { get; set; }
        public RoleStatusEnum Status { get; set; }
    }
}
