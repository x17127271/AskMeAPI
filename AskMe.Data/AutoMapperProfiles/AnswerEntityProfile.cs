using AskMe.Data.Entities;
using AskMe.Domain.Models;
using AutoMapper;

namespace AskMe.Data.AutoMapperProfiles
{
    public class AnswerEntityProfile : Profile
    {
        public AnswerEntityProfile()
        {
            CreateMap<AnswerEntity, Answer>();
            CreateMap<Answer, AnswerEntity>();
        }
    }
}
