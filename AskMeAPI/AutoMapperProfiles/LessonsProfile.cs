using AskMe.Domain.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskMe.API.AutoMapperProfiles
{
    public class LessonsProfile : Profile
    {
        public LessonsProfile()
        {
            CreateMap<Lesson, Models.LessonDto>();
            CreateMap<Models.LessonForCreationDto, Lesson>();
        }
    }
}
