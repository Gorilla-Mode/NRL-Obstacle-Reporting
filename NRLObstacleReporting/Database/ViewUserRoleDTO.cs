namespace NRLObstacleReporting.Database;

/// <summary>
/// Data-transfer object targeting view view_UserRole
/// </summary>
public record ViewUserRoleDto 
{
    public required string RoleId { get; init; }
    public required string UserId { get; init; }
    
    public string? UserName { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
}