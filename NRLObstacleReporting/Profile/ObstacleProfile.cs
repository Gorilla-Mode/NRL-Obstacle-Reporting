using NRLObstacleReporting.Database;
using NRLObstacleReporting.Models;

namespace NRLObstacleReporting.Profile;

public class ObstacleProfile : AutoMapper.Profile
{
    public ObstacleProfile()
    {
        CreateMap<ObstacleDto, ObstacleCompleteModel>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.GeometryGeoJson, opt => opt.MapFrom(src => src.GeometryGeoJson))
            .ForMember(dest => dest.Illuminated, opt => opt.MapFrom(src => src.Illuminated));
        
        CreateMap<ObstacleCompleteModel, ObstacleDto>();
        
        CreateMap<ObstacleStep1Model, ObstacleDto>();
        
        CreateMap<ObstacleStep2Model, ObstacleDto>();
        
        CreateMap<ObstacleStep3Model, ObstacleDto>();
    }
    
}