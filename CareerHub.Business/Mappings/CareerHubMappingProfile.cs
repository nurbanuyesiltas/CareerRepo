using AutoMapper;
using CareerHub.Core.Parameters;
using CareerHub.Entities.Entities;

namespace CareerHub.Business.Mappings
{
    public class CareerHubMappingProfile:Profile
    {
        internal CareerHubMappingProfile()
        {
            CreateMap<Job, JobRequestModel>()             
              .ForMember(d => d.Benefits, o => o.MapFrom(s => s.Benefits))
              .ForMember(d => d.Description, o => o.MapFrom(s => s.Description))
              .ForMember(d => d.Position, o => o.MapFrom(s => s.Position))
              .ForMember(d => d.Salary, o => o.MapFrom(s => s.Salary))
              .ForMember(d => d.WorkingType, o => o.MapFrom(s => s.WorkingType))
              .ForMember(d => d.CompanyId, o => o.MapFrom(s => s.CompanyId))
              .ReverseMap()
              ;

            CreateMap<Company, CompanyRequestModel>()
            .ForMember(d => d.PhoneNumber, o => o.MapFrom(s => s.PhoneNumber))
            .ForMember(d => d.CompanyName, o => o.MapFrom(s => s.CompanyName))
            .ForMember(d => d.Address, o => o.MapFrom(s => s.Address))
            .ReverseMap()
            ;
        }
    }
}
