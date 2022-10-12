using AutoMapper;

namespace NZWalks.API.Profiles
{
    public class RegionsProfile : Profile
    {
        public RegionsProfile()
        {
            CreateMap<Models.Domain.Region, Models.DTO.Region>()
        // .ForMember(dest => dest.Id, options => options.MapFrom(source => source.RegionId)); // If source and destination different columns names mapping 
        .ReverseMap();
        }
    }
}
