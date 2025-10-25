using NRLObstacleReporting.Database;

namespace NRLObstacleReporting.Repositories;

public class DraftRepository : RepositoryBase, IDraftRepository
{
    
    //TODO: Implements these things
    public Task EditDraft(int id, ObstacleDto data)
    {
        throw new NotImplementedException();
    }

    public Task SubmitDraft(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ObstacleDto>> GetAllDrafts()
    {
        throw new NotImplementedException();
    }
}