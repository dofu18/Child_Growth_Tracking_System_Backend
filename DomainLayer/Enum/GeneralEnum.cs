using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Enum
{
    public class GeneralEnum
    {
        public enum RoleStatusEnum
        {
            Active,
            Disable,
            Pending,
        }
        public enum UserStatusEnum
        {
            Active,
            Disable,
            Archived,
        }
        public enum GenderEnum
        {
            Male,
            Female,
        }
        public enum GroupAgeEnum
        {
            Under2,
            From2to19,
        }
        public enum DoctorLicenseStatusEnum
        {
            Published,
            Archived,
            Disable,
            Pending,
        }
        public enum ConsultationRequestStatusEnum
        {
            Approve,
            Reject,
            Pending,
            Archived,
        }
        public enum PaymentStatusEnum
        {
            Pending,
            Failed,
            Successfully,
        }
        public enum UserPackageStatusEnum
        {
            OnGoing,
            Cancel,
            Expired,
        }
    }
}
