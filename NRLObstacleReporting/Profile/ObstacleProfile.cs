using NRLObstacleReporting.Database;
using NRLObstacleReporting.Models;

namespace NRLObstacleReporting.Profile;

public class ObstacleProfile : AutoMapper.Profile
{
    public ObstacleProfile()
    {
        CreateMap<ObstacleDto, ObstacleCompleteModel>();
    }
}