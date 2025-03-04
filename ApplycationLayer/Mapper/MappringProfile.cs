using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationLayer.DTOs.BmiCategory;
using ApplicationLayer.DTOs.Children;
using ApplicationLayer.DTOs.Childrens;
using ApplicationLayer.DTOs.Package;
using ApplicationLayer.DTOs.Payment;
using ApplicationLayer.DTOs.RatingFeedback;
using ApplicationLayer.DTOs.User;
using AutoMapper;
using DomainLayer.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApplicationLayer.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //BmiCategory
            CreateMap<BmiCategory, BmiCategoryCreateDto>().ReverseMap();

            //Children
            CreateMap<Children, ChildrenCreateDto>().ReverseMap();
            CreateMap<Children, ChildrentResponseDto>();
            CreateMap<Children, ChildrenUpdateDto>().ReverseMap();

            //User
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserUpdateDto>().ReverseMap();
            //RatingFeedback
            CreateMap<RatingFeedback,RatingFeedbackCreateDto>().ReverseMap();
            CreateMap<RatingFeedback, RatingFeedbackUpdateDto>().ReverseMap();
            CreateMap<RatingFeedback, RatingFeedbackDto>().ReverseMap();
            //Package
            CreateMap<Package, PackageCreateDto>().ReverseMap();
            CreateMap<Package, RenewPackageDto>().ReverseMap();
            CreateMap<Package, CancelPackageDto>().ReverseMap();
            CreateMap<Package, PackageUpdateDto>().ReverseMap();

            //Transaction
            CreateMap<Transaction, PaymentResponseDto>();
        }
    }
}
