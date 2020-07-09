using AskMe.Data.Entities;
using AskMe.Domain.Models;
using AutoMapper;

namespace AskMe.Data.AutoMapperProfiles
{
    public class ExamsQuestionsProfile : Profile
    {
        public ExamsQuestionsProfile()
        {
            CreateMap<ExamsQuestions, ExamQuestion>();
            CreateMap<ExamQuestion, ExamsQuestions>();
        }
    }
}
