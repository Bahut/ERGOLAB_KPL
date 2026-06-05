using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TUBES_KPL.API.Models;

namespace TUBES_KPL.API.Controllers
{
    [ApiController]
    [Route("api/config")]
    public class ConfigController : ControllerBase
    {
        private readonly string _dataPath;
        private readonly JsonSerializerOptions _jsonOpts = new() { PropertyNameCaseInsensitive = true };

        public ConfigController(IWebHostEnvironment env)
        {
            _dataPath = Path.Combine(env.ContentRootPath, "Data");
        }

        private T Load<T>(string fileName)
        {
            string path = Path.Combine(_dataPath, fileName);
            string json = System.IO.File.ReadAllText(path);
            return JsonSerializer.Deserialize<T>(json, _jsonOpts)!;
        }

        // GET api/config/categories
        [HttpGet("categories")]
        public IActionResult GetCategories()
        {
            var config = Load<CategoryConfig>("categories.json");
            return Ok(config);
        }

        // GET api/config/roles
        [HttpGet("roles")]
        public IActionResult GetRoles()
        {
            var config = Load<RoleConfig>("role_permission.json");
            return Ok(config);
        }

        // GET api/config/sla
        [HttpGet("sla")]
        public IActionResult GetSla()
        {
            var config = Load<SlaConfig>("sla_rules.json");
            return Ok(config);
        }

        // GET api/config/notifications
        [HttpGet("notifications")]
        public IActionResult GetNotifications()
        {
            var config = Load<NotificationConfig>("notification_templates.json");
            return Ok(config);
        }
    }
}
