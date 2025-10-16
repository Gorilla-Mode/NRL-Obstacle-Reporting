namespace NRLObstacleReporting.Database;

public class StatusDatamodel
{
    public int StatusId { get; set; }
    public string StatusName { get; set; } = null!;
    
    public ICollection<ReportDatamodel> Reports { get; set; } = new List<ReportDatamodel>();
}