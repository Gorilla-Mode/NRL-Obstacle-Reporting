namespace NRLObstacleReporting.Database;

public class ReportDatamodel
{
    public int ReportId { get; set; }        
    public DateTime Dato { get; set; }
    
   //FK til statusId
    public int StatusId { get; set; }
    public StatusDatamodel Status { get; set; } = null!;
    
    //FK til pilotId
    public int PilotId { get; set; }
    public PilotDatamodel? Pilot { get; set; } 
}