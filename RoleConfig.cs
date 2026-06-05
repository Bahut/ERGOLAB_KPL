public class RoleConfig
{
    public List<RolePermission> Roles { get; set; }
}

public class RolePermission
{
    public string Role { get; set; }

    public List<string> Permissions { get; set; }
}