namespace NRLObstacleReporting.Database;

public class PilotDatamodel
{
    public int PilotId { get; set; }  //(PK)
    public string? Organization { get; set; }
    public string? Name { get; set; }
}