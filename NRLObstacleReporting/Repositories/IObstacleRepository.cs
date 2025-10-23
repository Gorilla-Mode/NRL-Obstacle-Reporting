using System.Data;
using NRLObstacleReporting.Database;

namespace NRLObstacleReporting.Repositories;

public interface IObstacleRepository
{
    Task InsertStep1(ObstacleDto data);
    Task InsertStep2(ObstacleDto data);
    Task InsertStep3(ObstacleDto data);
    Task<IEnumerable<ObstacleDto>> GetAllObstacleData(); 
}