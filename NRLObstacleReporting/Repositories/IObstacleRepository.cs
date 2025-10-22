using System.Data;
using NRLObstacleReporting.Database;

namespace NRLObstacleReporting.Repositories;

public interface IObstacleRepository
{
    Task Insert(ObstacleDto data);
    Task<IEnumerable<ObstacleDto>> GetAllObstacleData(); 
}