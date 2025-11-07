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
    public int ObstacleId { get; set; }
    public ObstacleStatus Status { get; set; }
    public ObstacleMarking Marking { get; set; }
    
    //Step 1 felt
    public ObstacleTypes Type { get; set; }
    
    [Required(ErrorMessage = "This field is required")]
    [Range(0, 150)]
    public int HeightMeter { get; set; }

    //Step 2 felt
    [Required(ErrorMessage = "This field is required")]
    public string? GeometryGeoJson { get; set; }
    //Step 3 felt
    [MaxLength(100)]
    public string? Name { get; set; }
    
    [MaxLength(1000)]
    public string? Description { get; set; }

    public Illumination Illuminated { get; set; }
    
    public string StatusText
    {
        get
        {
            return Status switch
            {
                ObstacleStatus.Pending => "Pending",
                ObstacleStatus.Approved => "Accepted",
                ObstacleStatus.Rejected => "Rejected",
                ObstacleStatus.Draft => "Draft",
                ObstacleStatus.Deleted => "Deleted",
                _ => "Unknown"
            };
        }
    }

    public string StatusColor
    {
        get
        {
            return Status switch
            {
                ObstacleStatus.Pending => "text-yellow-600",
                ObstacleStatus.Approved => "text-green-600",
                ObstacleStatus.Rejected => "text-red-600",
                ObstacleStatus.Draft => "text-gray-500",
                ObstacleStatus.Deleted => "text-gray-400",
                _ => "text-gray-600"
            };
        }
    }

}