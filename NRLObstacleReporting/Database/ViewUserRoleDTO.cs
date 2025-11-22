using Microsoft.AspNetCore.Identity;

namespace NRLObstacleReporting.Database;

public class ViewUserRoleDto : IdentityUser
{
    public required string RoleId { get; set; }
}