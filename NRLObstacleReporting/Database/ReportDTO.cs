namespace NRLObstacleReporting.Database;

public class ReportDTO
{
    public int ReportId { get; set; }        
    public DateTime Dato { get; set; }
    
    //FK til statusId
    public int StatusId { get; set; }
    public StatusDTO Status { get; set; } = null!;
    
    //FK til pilotId
    public int PilotId { get; set; }
    public PilotDTO? Pilot { get; set; } 
}