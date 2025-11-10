using Dapper;
using MySqlConnector;
using NRLObstacleReporting.Database;
using NRLObstacleReporting.Models;

namespace NRLObstacleReporting.Repositories
{
    public class RegistrarRepository : RepositoryBase, IRegistrarRepository
    {
        public async Task<IEnumerable<ObstacleDto>> GetAllReportsAsync()
        {
            using var connection = CreateConnection();
            var sql = @$"
                SELECT * FROM Obstacle
                WHERE Status <> {(int)ObstacleCompleteModel.ObstacleStatus.Draft}
                  AND Status <> {(int)ObstacleCompleteModel.ObstacleStatus.Deleted}";
            
            return await connection.QueryAsync<ObstacleDto>(sql);
        }

        public async Task<IEnumerable<ObstacleDto>> GetReportsByStatusAsync(ObstacleCompleteModel.ObstacleStatus status)
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM Obstacle WHERE Status = @Status";
            return await connection.QueryAsync<ObstacleDto>(sql, new { Status = (int)status });
        }
    }
}
