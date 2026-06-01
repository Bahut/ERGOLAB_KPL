using System;
using TUBES_KPL.Core;
using TUBES_KPL.Models;
using TUBES_KPL.Testing;

namespace TUBES_KPL
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("=== TABLE-DRIVEN SIMULATION ===");

                Complaint complaint = new Complaint(
                    "Jalan Rusak",
                    "Infrastruktur",
                    "Berat",
                    "Jalan berlubang besar di depan sekolah",
                    "Jl. Merdeka No.10",
                    "warga123"
                );

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

                Console.WriteLine("\n====================================");
                Console.WriteLine("TABLE DRIVEN UNIT TEST");
                Console.WriteLine("====================================");
                TableDrivenUnitTest.Run();

                Console.WriteLine("\n====================================");
                Console.WriteLine("PERFORMANCE TEST");
                Console.WriteLine("====================================");
                PerformanceTest.Run();

                Console.WriteLine("\n====================================");
                Console.WriteLine("WORKFLOW UNIT TEST");
                Console.WriteLine("====================================");
                UnitTestSimulation.Run();
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