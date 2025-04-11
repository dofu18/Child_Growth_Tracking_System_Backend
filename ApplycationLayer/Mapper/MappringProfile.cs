using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationLayer.DTOs.BmiCategory;
using ApplicationLayer.DTOs.Children;
using ApplicationLayer.DTOs.Childrens;
using ApplicationLayer.DTOs.Package;
using ApplicationLayer.DTOs.Doctor;
using ApplicationLayer.DTOs.Payments;
using ApplicationLayer.DTOs.RatingFeedback;
using ApplicationLayer.DTOs.Transaction;
using ApplicationLayer.DTOs.Users;
using AutoMapper;
using DomainLayer.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ApplicationLayer.DTOs.Payment;
using ApplicationLayer.DTOs.Feature;
using ApplicationLayer.DTOs.Consultation.ConsultationRequests;
using ApplicationLayer.DTOs.Consultation.ConsultationResponses;
using ApplicationLayer.DTOs.Alert;
using ApplicationLayer.DTOs;
using ApplicationLayer.DTOs.WHOData;

namespace ApplicationLayer.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Alert Mapping
            CreateMap<Alert, AlertDto>().ReverseMap(); 
            CreateMap<Alert, CheckHealthAlertRequestDto>().ReverseMap();


            //BmiCategory
            CreateMap<BmiCategory, BmiCategoryCreateDto>().ReverseMap();
            CreateMap<BmiCategory, BmiCategoryDto>();

            //Children
            CreateMap<Children, ChildrenCreateDto>().ReverseMap();
            CreateMap<Children, ChildrentResponseDto>();
            CreateMap<Children, ChildrenUpdateDto>().ReverseMap();
            CreateMap<Children, ChildrenDto>();

            //User
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserUpdateDto>().ReverseMap();

            //Doctor
            CreateMap<DoctorLicense, DoctorDto>().ReverseMap();
            CreateMap<DoctorLicense, DoctorUpdateDto>().ReverseMap();
            CreateMap<DoctorCreateDto, DoctorLicense>().ReverseMap();

            //RatingFeedback
            CreateMap<RatingFeedback,RatingFeedbackCreateDto>().ReverseMap();
            CreateMap<RatingFeedback, RatingFeedbackUpdateDto>().ReverseMap();
            CreateMap<RatingFeedback, RatingFeedbackDto>().ReverseMap();

            //Package
            CreateMap<Package, PackageCreateDto>().ReverseMap();
            CreateMap<Package, RenewPackageDto>();
            CreateMap<Package, PackageUpdateDto>().ReverseMap();
            CreateMap<Package, PackageDto>();

            //UserPackage
            CreateMap<UserPackage, UserPackageDto>()
                           .ForMember(dest => dest.PackageName, opt => opt.MapFrom(src => src.Package.PackageName));

            //Transaction
            CreateMap<Transaction, PaymentResponseDto>();
            CreateMap<Transaction, TransactionDto>().ReverseMap();
            CreateMap<Transaction, PaymentListDto>()
                        .ForMember(dest => dest.PackageName, opt => opt.MapFrom(src => src.Package.PackageName)) // Lấy từ Transaction.Package.PackageName
                        .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.PaymentStatus.ToString())); // Enum phải chuyển về string

            //Feature
            //CreateMap<Feature, FeatureDto>().ReverseMap();
            //CreateMap<Feature, FeatureCreateDto>().ReverseMap();
            //CreateMap<Feature, FeatureUpdateDto>().ReverseMap();

            //Consultation Request
            CreateMap<ConsultationRequest, ConsultationRequestDto>().ReverseMap();
            CreateMap<ConsultationRequest, ConsultationRequestCreateDto>().ReverseMap();

            //Response
            CreateMap<ConsultationResponse, ConsultationResponseDto>().ReverseMap();
            CreateMap<ConsultationResponse, ConsultationResponseCreateDto>().ReverseMap();

            //WhoData
            CreateMap<WhoDataDto, WhoData>().ReverseMap();
            CreateMap<WhoDataUpdateDto, WhoData>();
            CreateMap<WhoDataCreateDto, WhoData>();

        }
    }
}
