using AskMe.Domain.Models;
using AutoMapper;

namespace AskMeAPI.AutoMapperProfiles
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<User, Models.UserDto>();
            CreateMap<Models.UserForCreationDto, User>();
            CreateMap<Models.UserForUpdateDto, User>();
        }
    }
}
