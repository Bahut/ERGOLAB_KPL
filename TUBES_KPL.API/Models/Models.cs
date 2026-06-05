namespace TUBES_KPL.API.Models
{
    // ── Config models ──────────────────────────────────────────
    public class CategoryConfig
    {
        public List<CategoryItem> Categories { get; set; } = new();
    }

    public class CategoryItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Icon { get; set; } = "";
    }

    public class RoleConfig
    {
        public List<RolePermission> Roles { get; set; } = new();
    }

    public class RolePermission
    {
        public string Role { get; set; } = "";
        public List<string> Permissions { get; set; } = new();
    }

    public class SlaConfig
    {
        public List<SlaRule> Rules { get; set; } = new();
    }

    public class SlaRule
    {
        public string Category { get; set; } = "";
        public string Impact { get; set; } = "";
        public int MaxDays { get; set; }
    }

    public class NotificationConfig
    {
        public List<NotificationTemplate> Templates { get; set; } = new();
    }

    public class NotificationTemplate
    {
        public string Event { get; set; } = "";
        public string Message { get; set; } = "";
    }

    // ── Complaint models ───────────────────────────────────────
    public enum ComplaintStatus
    {
        Diajukan,
        Diverifikasi,
        Diproses,
        Selesai,
        Ditolak
    }

    public class ComplaintDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public string Category { get; set; } = "";
        public string Impact { get; set; } = "";
        public string Description { get; set; } = "";
        public string Location { get; set; } = "";
        public string Reporter { get; set; } = "";
        public string ResponsibleUnit { get; set; } = "";
        public DateTime CreatedDate { get; set; }
        public DateTime DeadlineDate { get; set; }
        public ComplaintStatus Status { get; set; }
    }

    public class CreateComplaintRequest
    {
        public string Title { get; set; } = "";
        public string Category { get; set; } = "";
        public string Impact { get; set; } = "Sedang";
        public string Description { get; set; } = "";
        public string Location { get; set; } = "";
        public string Reporter { get; set; } = "";
    }

    public class ChangeStatusRequest
    {
        public string Action { get; set; } = ""; // verify | process | finish | reject
    }
}
