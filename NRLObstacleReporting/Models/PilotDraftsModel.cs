namespace NRLObstacleReporting.Models;

public class PilotDraftsModel
{
    public IEnumerable<ObstacleCompleteModel> SubmittedDrafts { get; set; } = new List<ObstacleCompleteModel>();
}