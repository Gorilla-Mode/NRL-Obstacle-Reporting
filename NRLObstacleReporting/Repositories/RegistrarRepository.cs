using Dapper;
using MySqlConnector;
using System.Data;
using NRLObstacleReporting.Database;
using NRLObstacleReporting.Models;

namespace NRLObstacleReporting.Repositories
{
    public class RegistrarRepository : RepositoryBase, IRegistrarRepository
    {
        /// <inheritdoc/>
        public async Task<IEnumerable<ObstacleDto>> GetAllSubmittedObstacles()
        {
            
            using var connection = CreateConnection();
            var sql = @$"SELECT * 
                         FROM Obstacle 
                         WHERE Status <> {(int)ObstacleCompleteModel.ObstacleStatus.Draft}";
            
            return await connection.QueryAsync<ObstacleDto>(sql);
        }
    }
}