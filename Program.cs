using System;
using TUBES_KPL.Core;
using TUBES_KPL.Models;

namespace TUBES_KPL
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("=== LOAD CONFIGURATION ===");

                Console.WriteLine("=== DAFTAR KATEGORI PENGADUAN ===");

                CategoryConfig categoryConfig = ConfigLoader.Load<CategoryConfig>("categories.json");

                foreach (var category in categoryConfig.Categories)
                {
                    Console.WriteLine(
                        $"{category.Id}. {category.Name}");
                }

                RoleConfig roleConfig = ConfigLoader.Load<RoleConfig>("role_permission.json");

                Console.WriteLine("=== ROLE PERMISSIONS ===");

                foreach (var role in roleConfig.Roles)
                {
                    Console.WriteLine($"\nRole: {role.Role}");

                    foreach (var permission in role.Permissions)
                    {
                        Console.WriteLine($" - {permission}");
                    }
                }

                Console.WriteLine("=== TABLE-DRIVEN SIMULATION ===");

                Console.WriteLine("=== GENERICS DEMO ===");

                var complaint = new Complaint(
                    "Jalan Rusak",
                    "Infrastruktur",
                    "Berat",
                    "Jalan berlubang",
                    "Bandung",
                    "Budi");

                var result =
                    Result<Complaint>.Ok(
                        complaint,
                        "Berhasil");

                Console.WriteLine(result.Success);
                Console.WriteLine(result.Message);

                Console.WriteLine($"\nAuto-Assigned Unit : {complaint.ResponsibleUnit}");
                Console.WriteLine($"Deadline SLA       : {complaint.DeadlineDate.ToShortDateString()}");
                Console.WriteLine($"Status Awal        : {complaint.Status}");

                ComplaintWorkflow workflow = new ComplaintWorkflow();

                workflow.ChangeStatus(complaint, "verify");
                Console.WriteLine($"\nStatus Sekarang    : {complaint.Status}");

                workflow.ChangeStatus(complaint, "process");
                Console.WriteLine($"Status Sekarang    : {complaint.Status}");

                workflow.ChangeStatus(complaint, "finish");
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
    }
}