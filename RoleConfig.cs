public class RoleConfig
{
    public List<RolePermission> Roles { get; set; } = [];
}

public class RolePermission
{
    public required string Role { get; set; }
    public List<string> Permissions { get; set; } = [];
}