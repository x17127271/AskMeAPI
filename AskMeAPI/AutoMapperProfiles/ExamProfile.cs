using AskMe.Domain.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskMe.API.AutoMapperProfiles
{
    public class ExamProfile : Profile
    {
        public ExamProfile()
        {
            CreateMap<Exam, Models.ExamDto>();
        }
    }
}
