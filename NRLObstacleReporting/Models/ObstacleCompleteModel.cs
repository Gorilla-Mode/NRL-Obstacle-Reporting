namespace NRLObstacleReporting.Models;
using System.ComponentModel.DataAnnotations;

public class ObstacleCompleteModel
{
    /// <summary>
    /// Represents all possible types of an obstacle. 
    /// </summary>
    public enum ObstacleTypes
    {
        AirSpan = 0,
        PoleOrTower,
        Building,
        Construction,
        Bridge,
        Other
    }
    
    /// <summary>
    /// Represents the possible states of illumination of an obstacle. First state (unknown) is default
    /// </summary>
    public enum Illumination
    {
        Unknown = 0,
        NotIlluminated,
        Illuminated
    }
    
    /// <summary>
    /// Represents the possible states of Marking of an obstacle. First state (unknown) is default
    /// </summary>
    public enum ObstacleMarking
    {
        Unknown = 0,
        NotMarked,
        Marked
    }
    
    /// <summary>
    /// Represents the status of an obstacle. A different status than draft implies the obstacle has been submitted for
    /// review. First state (draft) is default.
    /// </summary>
    public enum ObstacleStatus
    {
        Draft = 0,
        Pending,
        Approved,
        Rejected,
        Deleted
    }
    
    public bool IsDraft { get; set; } = true; //TODO: remove this
    public string ObstacleId { get; set; }
    public string UserId { get; set; }
    public ObstacleStatus Status { get; set; }
    public ObstacleMarking Marking { get; set; }
    
    public DateTime CreationTime { get; set; }
    public DateTime UpdatedTime { get; set; }
    
    //Step 1 felt
    public ObstacleTypes Type { get; set; }
    
    [Required(ErrorMessage = "This field is required")]
    [Range(0, 150)]
    public int HeightMeter { get; set; }

    //Step 2 felt
    [Required(ErrorMessage = "This field is required")]
    [MaxLength(5000)]
    public string? GeometryGeoJson { get; set; }
    //Step 3 felt
    [MaxLength(100)]
    public string? Name { get; set; }
    
    [MaxLength(1000)]
    public string? Description { get; set; }

    public Illumination Illuminated { get; set; }
}