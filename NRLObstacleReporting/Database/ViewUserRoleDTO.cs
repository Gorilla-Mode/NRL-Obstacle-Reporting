using Microsoft.AspNetCore.Identity;

namespace NRLObstacleReporting.Database;

/// <summary>
/// Data-transfer object targeting view view_UserRole
/// </summary>
public class ViewUserRoleDto : IdentityUser
{
    public required string RoleId { get; init; }
    public required string UserId { get; init; }
}