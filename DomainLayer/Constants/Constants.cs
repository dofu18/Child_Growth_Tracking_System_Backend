using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Constants
{
    public class Constants
    {
        public static class Errors
        {
            public const string NOT_EXIST_ERROR = "not exist";
            public const string ALREADY_EXIST_ERROR = "already exist";
            public static string ENUM_NOT_EXIST_ERROR(string Enum, int? EnumNum)
            {
                return $"Please choose the correct {Enum} [1-{EnumNum}]";
            }
        }
        public static class Http
        {
            public const string API_VERSION = "v1";
            public const string CORS = "CORS";
            public const string JSON_CONTENT_TYPE = "application/json";
            public const string USER_POLICY = "User";
        }
        public static class Entities
        {
            public const string ALERT = "Alert ";
            public const string BMI_CATEGORY = "BMI Category ";
            public const string CHILDREN= "Children ";
            public const string CONSULTATION_REQUEST = "Consultation Request ";
            public const string CONSULTATION_RESPONSE = "Consultation Response ";
            public const string DOCTOR_LICENSE = "Doctor License ";
            public const string DOCTOR_SPECIALIZATION = "Doctor Specialization ";
            public const string FEATURE = "Feature ";
            public const string GROWTH_RECORD = "Growth Record ";
            public const string PACKAGE = "Package ";
            public const string PACKAGE_FEATURE = "Package Feature ";
            public const string RATING_FEEDBACK = "Rating Feedback ";
            public const string ROLE = "Role ";
            public const string SHARING_PROFILE = "Sharing Profile ";
            public const string SPECIALIZATION = "Specialization ";
            public const string TRANSACTION = "Transaction ";
            public const string USER = "User ";
            public const string USER_PACKAGE = "User Package ";
        }
    }
}
