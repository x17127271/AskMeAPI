using AskMe.Data.Entities;
using AskMe.Domain.Models;
using AutoMapper;

namespace AskMe.Data.AutoMapperProfiles
{
    public class ResultEntityProfile : Profile
    {
        public ResultEntityProfile()
        {
            CreateMap<ResultEntity, Result>();
            CreateMap<Result, ResultEntity>();
        }
    }
}
