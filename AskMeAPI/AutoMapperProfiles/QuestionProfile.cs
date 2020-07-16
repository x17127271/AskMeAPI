using AskMe.API.Models;
using AskMe.Domain.Models;
using AutoMapper;

namespace AskMe.API.AutoMapperProfiles
{
    public class QuestionProfile : Profile
    {
        public QuestionProfile()
        {
            CreateMap<Question, QuestionDto>();
            CreateMap<Models.QuestionForCreationDto, Question>();
        }
    }
}
