using Dapper;
using NRLObstacleReporting.Database;
using NRLObstacleReporting.Models;

namespace NRLObstacleReporting.Repositories;

public class DraftRepository : RepositoryBase, IDraftRepository
{
    
    //TODO: Implements these things
    public async Task EditDraft(int id, ObstacleDto data)
    {
        throw new NotImplementedException();
    }

    public async Task SubmitDraft(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ObstacleDto>> GetAllDrafts()
    {
        using var connection = CreateConnection();
        const int statusId = (int)ObstacleCompleteModel.ObstacleStatus.Draft;
        var sql = $"select * from Obstacle where Status = {statusId}";
        
        return await connection.QueryAsync<ObstacleDto>(sql);
    }
}