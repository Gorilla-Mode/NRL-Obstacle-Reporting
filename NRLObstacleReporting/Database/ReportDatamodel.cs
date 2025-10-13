namespace NRLObstacleReporting.Database;

public class ReportDatamodel
{
    public int ReportId { get; set; }        
    public DateTime Dato { get; set; }
    public string? Status { get; set; } 
    
    public int PilotId { get; set; }
    public PilotDatamodel? Pilot { get; set; } // navigasjon
}