using AskMe.Domain.Models;
using AutoMapper;

namespace AskMe.API.AutoMapperProfiles
{
    public class QuestionProfile : Profile
    {
        public QuestionProfile()
        {
            CreateMap<Question, Models.QuestionDto>();
            CreateMap<Models.QuestionForCreationDto, Question>();
        }
    }
}
