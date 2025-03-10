using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Enum.GeneralEnum;

namespace DomainLayer.Entities
{
    public class Children : BaseEntity
    {
        public Guid ParentId { get; set; }
        public string Name { get; set; }
        public DateOnly DoB {  get; set; }
        public GenderEnum? Gender { get; set; }
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public decimal Bmi { get; set; }
        public Guid BmiCategoryId { get; set; }
        public decimal BmiPercentile { get; set; }
        public string Notes { get; set; }
        public GroupAgeEnum? GroupAge { get; set; }
        public ChildrentStatusEnum? Status { get; set; }

        public User Parent { get; set; }
        public BmiCategory BmiCategory { get; set; }
    }
}
