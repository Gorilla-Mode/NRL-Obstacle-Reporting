namespace NRLObstacleReporting.Database;

public class RegistrarDTO
{
    public int RegistrarId { get; set; }  // (PK)
    public string? Name { get; set; }
    public string? Role { get; set; }
    public string? Organization { get; set; }
}