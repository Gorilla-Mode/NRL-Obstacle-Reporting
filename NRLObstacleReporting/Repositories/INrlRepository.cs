using System.Data;
using NRLObstacleReporting.Database;

namespace NRLObstacleReporting.Repositories;

public interface INrlRepository
{
    Task InsertObstacleData(ObstacleDto data);
    Task<IEnumerable<ObstacleDto>> GetAllObstacleData(); 
}