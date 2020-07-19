using AskMe.API.Models;
using AskMe.Domain.Models;
using AutoMapper;

namespace AskMe.API.AutoMapperProfiles
{
    public class SubjectsProfile : Profile
    { 
        public SubjectsProfile()
        {
            CreateMap<Subject, SubjectDto>()
                .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.Id))
                .ForMember(
                dest => dest.Title,
                opt => opt.MapFrom(src => src.Title))
                .ForMember(
                dest => dest.Description,
                opt => opt.MapFrom(src => src.Description))
                .ForMember(
                dest => dest.UserId,
                opt => opt.MapFrom(src => src.UserId));
            CreateMap<SubjectForCreationDto, Subject>();
        }
    }
}
