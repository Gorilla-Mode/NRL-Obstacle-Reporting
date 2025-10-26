using NRLObstacleReporting.Database;
using NRLObstacleReporting.Models;

namespace NRLObstacleReporting.Profile;

public class ObstacleProfile : AutoMapper.Profile
{
    public ObstacleProfile()
    {
        CreateMap<ObstacleDto, ObstacleCompleteModel>().ReverseMap();
        
        CreateMap<ObstacleStep1Model, ObstacleDto>();
        
        CreateMap<ObstacleStep2Model, ObstacleDto>();
        
        CreateMap<ObstacleStep3Model, ObstacleDto>();
    }
    
}