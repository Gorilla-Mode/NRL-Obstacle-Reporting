namespace NRLObstacleReporting.Database;

public class PilotDto
{
    public int PilotId { get; set; }  //(PK)
    public string? Organization { get; set; }
    public string? Name { get; set; }
}