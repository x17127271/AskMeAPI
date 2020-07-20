using AskMe.API.Models;
using AskMe.Domain.Models;
using AutoMapper;

namespace AskMe.API.AutoMapperProfiles
{
    public class LessonsProfile : Profile
    {
        public LessonsProfile()
        {
            CreateMap<Lesson, LessonDto>();
            CreateMap<LessonForCreationDto, Lesson>();
            CreateMap<LessonDto, Lesson>();
        }
    }
}
