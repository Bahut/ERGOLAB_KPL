using Microsoft.AspNetCore.Mvc;
using TUBES_KPL.API.Models;
using System.Text.Json;

namespace TUBES_KPL.API.Controllers
{
    [ApiController]
    [Route("api/complaints")]
    public class ComplaintController : ControllerBase
    {
        // In-memory store
        private static readonly List<ComplaintDto> _store = new();

        private static readonly Dictionary<(ComplaintStatus, string), ComplaintStatus> _transitions = new()
        {
            { (ComplaintStatus.Diajukan,    "verify"),  ComplaintStatus.Diverifikasi },
            { (ComplaintStatus.Diajukan,    "reject"),  ComplaintStatus.Ditolak      },
            { (ComplaintStatus.Diverifikasi,"process"), ComplaintStatus.Diproses     },
            { (ComplaintStatus.Diproses,    "finish"),  ComplaintStatus.Selesai      },
        };

        private readonly string _dataPath;
        private readonly JsonSerializerOptions _jsonOpts = new() { PropertyNameCaseInsensitive = true };

        public ComplaintController(IWebHostEnvironment env)
        {
            _dataPath = Path.Combine(env.ContentRootPath, "Data");
        }

        // GET api/complaints
        [HttpGet]
        public IActionResult GetAll() => Ok(_store);

        // GET api/complaints/{id}
        [HttpGet("{id:guid}")]
        public IActionResult GetById(Guid id)
        {
            var item = _store.FirstOrDefault(c => c.Id == id);
            return item is null ? NotFound() : Ok(item);
        }

        // POST api/complaints
        [HttpPost]
        public IActionResult Create([FromBody] CreateComplaintRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Title))
                return BadRequest("Judul tidak boleh kosong.");

            if (string.IsNullOrWhiteSpace(req.Description))
                return BadRequest("Deskripsi tidak boleh kosong.");

            string unit = GetUnit(req.Category, req.Impact);
            int slaDays = GetSlaDays(req.Category, req.Impact);

            var complaint = new ComplaintDto
            {
                Id              = Guid.NewGuid(),
                Title           = req.Title,
                Category        = req.Category,
                Impact          = req.Impact,
                Description     = req.Description,
                Location        = req.Location,
                Reporter        = req.Reporter,
                ResponsibleUnit = unit,
                CreatedDate     = DateTime.Now,
                DeadlineDate    = DateTime.Now.AddDays(slaDays),
                Status          = ComplaintStatus.Diajukan
            };

            _store.Add(complaint);
            return CreatedAtAction(nameof(GetById), new { id = complaint.Id }, complaint);
        }

        // POST api/complaints/{id}/status
        [HttpPost("{id:guid}/status")]
        public IActionResult ChangeStatus(Guid id, [FromBody] ChangeStatusRequest req)
        {
            var complaint = _store.FirstOrDefault(c => c.Id == id);
            if (complaint is null) return NotFound();

            var key = (complaint.Status, req.Action.ToLower());
            if (!_transitions.TryGetValue(key, out var newStatus))
                return BadRequest($"Transisi tidak valid dari {complaint.Status} dengan aksi '{req.Action}'.");

            complaint.Status = newStatus;
            return Ok(complaint);
        }

        // ── Helpers (baca sla_rules.json & assignment) ─────────
        private string GetUnit(string category, string impact)
        {
            // Assignment sederhana: Unit {Category}
            var valid = new[] { "Kebersihan", "Keamanan", "Infrastruktur", "Administrasi", "Umum" };
            return valid.Contains(category) ? $"Unit {category}" : "Unit Umum";
        }

        private int GetSlaDays(string category, string impact)
        {
            try
            {
                string path = Path.Combine(_dataPath, "sla_rules.json");
                string json = System.IO.File.ReadAllText(path);
                var config = JsonSerializer.Deserialize<SlaConfig>(json, _jsonOpts);
                var rule = config?.Rules.FirstOrDefault(r =>
                    r.Category.Equals(category, StringComparison.OrdinalIgnoreCase) &&
                    r.Impact.Equals(impact, StringComparison.OrdinalIgnoreCase));
                return rule?.MaxDays ?? 3;
            }
            catch { return 3; }
        }
    }
}
