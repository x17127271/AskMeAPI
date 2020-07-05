using AskMe.Data.Entities;
using AskMe.Domain.Models;
using AutoMapper;

namespace AskMe.Data.AutoMapperProfiles
{
    public class SubjectEntityProfile : Profile
    {
        public SubjectEntityProfile()
        {
            CreateMap<SubjectEntity, Subject>();              
            CreateMap<Subject, SubjectEntity>();
             
        }
    }
}
