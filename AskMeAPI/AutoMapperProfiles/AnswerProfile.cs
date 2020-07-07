using AskMe.Domain.Models;
using AutoMapper;

namespace AskMe.API.AutoMapperProfiles
{
    public class AnswerProfile : Profile
    {
        public AnswerProfile()
        {
            CreateMap<Answer, Models.AnswerDto>();
            CreateMap<Models.AnswerForCreationDto, Answer>();
        }
    }
}
