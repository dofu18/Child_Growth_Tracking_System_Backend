using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            NotVerified,
        }
        public enum GenderEnum
        {
            Male,
            Female,
        }
        public enum GroupAgeEnum
        {
            From2to19,
            Under2,
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
        public enum AuthTypeEnum
        {
            Email,
            Google,
        }
        public enum IdType
        {
            [Description("Id")]
            Id = 1,
            [Description("User Id")]
            UserId = 2,
            [Description("Children Id")]
            ChildrenId = 3,
        }
        public enum ChildrentStatusEnum
        {
            Active,
            Disable,
            Archived,
        }
        public enum RatingFeedbackStatusEnum
        {
            Publish,
            Archived,
            Disable
        }
        public enum PackageStatusEnum
        {
            Pending,
            Published,
            Archived,
            Deleted,
        }
        public enum RatingTypeEnum
        {
            System,
            Doctor
        }
    }
}
