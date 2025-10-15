using System.Data;
using NRLObstacleReporting.Database;

namespace NRLObstacleReporting.Repositories;

public interface INrlRepository
{
    Task InsertObstacleData(ObstacleCompleteModel data);
    Task<IEnumerable<ObstacleCompleteModel>> GetAllObstacleData();
}