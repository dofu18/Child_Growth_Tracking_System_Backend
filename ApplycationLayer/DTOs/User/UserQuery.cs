using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DTOs.Users
{
    public class UserQuery : PaginationReq
    {
        public string? SearchKeyword { get; set; }
        public List<Guid>? RoleIds { get; set; }

    }
}
