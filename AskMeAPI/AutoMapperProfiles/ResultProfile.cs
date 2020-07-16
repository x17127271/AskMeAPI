using AskMe.API.Models;
using AskMe.Domain.Models;
using AutoMapper;

namespace AskMe.API.AutoMapperProfiles
{
    public class ResultProfile : Profile
    {
        public ResultProfile()
        {
            CreateMap<ExamResult, ExamResultDto>();
            CreateMap<ExamResultDto, ExamResult>();
        }
    }
}
