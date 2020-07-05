using AskMe.Data.Entities;
using AskMe.Domain.Models;
using AutoMapper;

namespace AskMe.Data.AutoMapperProfiles
{
    public class LessonEntityProfile : Profile
    {
        public LessonEntityProfile()
        {
            CreateMap<LessonEntity, Lesson>();
            CreateMap<Lesson, LessonEntity>();
        }
    }
}
