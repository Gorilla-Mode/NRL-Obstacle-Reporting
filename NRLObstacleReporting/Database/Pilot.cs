namespace NRLObstacleReporting.Database;

public class Pilot
{
    public int PilotId { get; set; }  //(PK)
    public string? Organization { get; set; }
    public string? Name { get; set; }
}