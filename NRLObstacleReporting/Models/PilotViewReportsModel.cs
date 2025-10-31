using NRLObstacleReporting.Database;

namespace NRLObstacleReporting.Models;

public class PilotViewReportsModel
{
    public IEnumerable<ObstacleCompleteModel> SubmittedReports { get; set; } = new List<ObstacleCompleteModel>();
}