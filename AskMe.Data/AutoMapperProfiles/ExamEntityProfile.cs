using AskMe.Data.Entities;
using AskMe.Domain.Models;
using AutoMapper;

namespace AskMe.Data.AutoMapperProfiles
{
    public class ExamEntityProfile : Profile
    {
        public ExamEntityProfile()
        {
            CreateMap<ExamEntity, Exam>();
            CreateMap<Exam, ExamEntity>();
        }
    }
}
