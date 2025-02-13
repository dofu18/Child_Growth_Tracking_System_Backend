using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Enum.GeneralEnum;

namespace DomainLayer.Entities
{
    public class User : BaseEntity
    {
        public Guid RoleId {  get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone {  get; set; }
        public DateTime LastLogin {  get; set; }
        public string Address { get; set; }
        public Guid PackageId { get; set; }
        public UserStatusEnum Status { get; set; }

        //Navigation Properties
        public Role Role { get; set; }
        public Package Package { get; set; }
    }
}
