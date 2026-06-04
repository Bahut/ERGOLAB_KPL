using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TUBES_KPL
{
    class Program
    {
        // Ganti port sesuai API kamu (cek Properties/launchSettings.json di project API)
        private static readonly HttpClient http = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:57918")
        };

        private static readonly JsonSerializerOptions _opts = new()
        {
            PropertyNameCaseInsensitive = true
        };

        static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine("=== LOAD CONFIGURATION (dari API) ===\n");

                // ── 1. Ambil kategori dari API ──────────────────────────
                Console.WriteLine("=== DAFTAR KATEGORI PENGADUAN ===");
                var catResponse = await http.GetStringAsync("/api/config/categories");
                var catConfig   = JsonSerializer.Deserialize<CategoryConfigDto>(catResponse, _opts)!;

                foreach (var cat in catConfig.Categories)
                    Console.WriteLine($"{cat.Id}. {cat.Name}");

                // ── 2. Ambil role permissions dari API ──────────────────
                Console.WriteLine("\n=== ROLE PERMISSIONS ===");
                var roleResponse = await http.GetStringAsync("/api/config/roles");
                var roleConfig   = JsonSerializer.Deserialize<RoleConfigDto>(roleResponse, _opts)!;

                foreach (var role in roleConfig.Roles)
                {
                    Console.WriteLine($"\nRole: {role.Role}");
                    foreach (var perm in role.Permissions)
                        Console.WriteLine($" - {perm}");
                }

                // ── 3. Submit complaint baru via API ────────────────────
                Console.WriteLine("\n=== GENERICS DEMO ===");

                var newComplaint = new
                {
                    title       = "Jalan Rusak",
                    category    = "Infrastruktur",
                    impact      = "Berat",
                    description = "Jalan berlubang",
                    location    = "Bandung",
                    reporter    = "Budi"
                };

                var postResp = await http.PostAsJsonAsync("/api/complaints", newComplaint);
                postResp.EnsureSuccessStatusCode();

                var complaint = JsonSerializer.Deserialize<ComplaintDto>(
                    await postResp.Content.ReadAsStringAsync(), _opts)!;

                Console.WriteLine($"Berhasil submit complaint!");
                Console.WriteLine($"Auto-Assigned Unit : {complaint.ResponsibleUnit}");
                Console.WriteLine($"Deadline SLA       : {complaint.DeadlineDate:d}");
                Console.WriteLine($"Status Awal        : {complaint.Status}");

                // ── 4. Ubah status via API ──────────────────────────────
                await ChangeStatus(complaint.Id, "verify");
                complaint = await GetComplaint(complaint.Id);
                Console.WriteLine($"\nStatus Sekarang    : {complaint.Status}");

                await ChangeStatus(complaint.Id, "process");
                complaint = await GetComplaint(complaint.Id);
                Console.WriteLine($"Status Sekarang    : {complaint.Status}");

                await ChangeStatus(complaint.Id, "finish");
                complaint = await GetComplaint(complaint.Id);
                Console.WriteLine($"Status Sekarang    : {complaint.Status}");

                Console.WriteLine("\nSimulasi selesai.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nERROR: {ex.Message}");
            }

            Console.WriteLine("\nTekan tombol apapun untuk keluar...");
            Console.ReadKey();
        }

        static async Task ChangeStatus(Guid id, string action)
        {
            var body = new StringContent(
                JsonSerializer.Serialize(new { action }),
                Encoding.UTF8,
                "application/json");

            var resp = await http.PostAsync($"/api/complaints/{id}/status", body);
            resp.EnsureSuccessStatusCode();
        }

        static async Task<ComplaintDto> GetComplaint(Guid id)
        {
            var json = await http.GetStringAsync($"/api/complaints/{id}");
            return JsonSerializer.Deserialize<ComplaintDto>(json, _opts)!;
        }

        // ── DTO sederhana buat deserialize response API ─────────────────
        record CategoryConfigDto(List<CategoryItemDto> Categories);
        record CategoryItemDto(int Id, string Name, string Icon);
        record RoleConfigDto(List<RolePermissionDto> Roles);
        record RolePermissionDto(string Role, List<string> Permissions);
        record ComplaintDto(
            Guid Id, string Title, string Category, string Impact,
            string Description, string Location, string Reporter,
            string ResponsibleUnit, DateTime CreatedDate, DateTime DeadlineDate,
            string Status);
    }
}
