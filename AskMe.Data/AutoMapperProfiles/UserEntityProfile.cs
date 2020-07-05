using AskMe.Data.Entities;
using AskMe.Domain.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace AskMe.Data.AutoMapperProfiles
{
    public class UserEntityProfile : Profile
    {
        public UserEntityProfile()
        {
            CreateMap<UserEntity, User>();
            CreateMap<User, UserEntity>();
        }
    }
}
