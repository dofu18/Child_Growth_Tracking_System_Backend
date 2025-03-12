using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DTOs.Feature
{
    public class FeatureQuery : PaginationReq
    {
        public string? SearchKeyword { get; set; }
    }
}
