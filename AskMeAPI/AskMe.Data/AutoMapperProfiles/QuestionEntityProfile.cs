using AskMe.Data.Entities;
using AskMe.Domain.Models;
using AutoMapper;

namespace AskMe.Data.AutoMapperProfiles
{
    public class QuestionEntityProfile : Profile
    {
        public QuestionEntityProfile()
        {
            CreateMap<QuestionEntity, Question>();
            CreateMap<Question, QuestionEntity>();
        }
    }
}
