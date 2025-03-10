using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Enum.GeneralEnum;

namespace ApplicationLayer.DTOs.Transaction
{
    public class TransactionQuery : PaginationReq
    {
        public DateOnly? FromDate { get; set; }
        public DateOnly? ToDate { get; set; }
    }
}
