using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationLayer.DTOs.BmiCategory;
<<<<<<< HEAD
using ApplicationLayer.DTOs.Children;
using ApplicationLayer.DTOs.Childrens;
=======
using ApplicationLayer.DTOs.User;
>>>>>>> 947c9ecbd79f84b3bd0e3715238ea359cc79874d
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

<<<<<<< HEAD
            CreateMap<Children, ChildrenCreateDto>().ReverseMap();
            CreateMap<Children, ChildrentResponseDto>();
            CreateMap<Children, ChildrenUpdateDto>().ReverseMap();
=======
            //User
            CreateMap<User, UserDto>().ReverseMap();
>>>>>>> 947c9ecbd79f84b3bd0e3715238ea359cc79874d
        }
    }
}
