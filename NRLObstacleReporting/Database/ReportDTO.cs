namespace NRLObstacleReporting.Database;

public class ReportDto
{
    public int ReportId { get; set; }        
    public DateTime Dato { get; set; }
    
    //FK til statusId
    public int StatusId { get; set; }
    public StatusDto Status { get; set; } = null!;
    
    //FK til pilotId
    public int PilotId { get; set; }
    public PilotDto? Pilot { get; set; } 
}