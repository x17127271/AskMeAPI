using AskMe.API.Models;
using AskMe.Domain.Models;
using AutoMapper;

namespace AskMe.API.AutoMapperProfiles
{
    public class ExamProfile : Profile
    {
        public ExamProfile()
        {
            CreateMap<Exam, ExamDto>();
            CreateMap<ExamQuestionsForCreationDto, Exam>();
        }
    }
}
