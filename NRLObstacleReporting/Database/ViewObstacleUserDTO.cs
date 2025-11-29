using Microsoft.AspNetCore.Identity;

namespace NRLObstacleReporting.Database;

/// <summary>
/// Datatransfer object targeting view view_ObstacleUser
/// </summary>
public record ViewObstacleUserDto : ObstacleDto
{
    public string? UserName { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
}