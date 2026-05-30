using System;
using TUBES_KPL.Core;
using TUBES_KPL.Models;

namespace TUBES_KPL
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== LurahOnline ===");
            Console.WriteLine("Pengelolaan Status Komplain");

            Complaint complaint = new Complaint(
                "Jalan Rusak",
                "Infrastruktur",
                "Jalan berlubang besar di depan sekolah",
                "Jl. Merdeka No.10",
                "warga123"
            );

            ComplaintWorkflow workflow = new ComplaintWorkflow();

            Console.WriteLine($"\nStatus Awal : {complaint.Status}");

            workflow.ChangeStatus(complaint, "verify");
            Console.WriteLine($"Status Sekarang : {complaint.Status}");

            workflow.ChangeStatus(complaint, "process");
            Console.WriteLine($"Status Sekarang : {complaint.Status}");

            workflow.ChangeStatus(complaint, "finish");
            Console.WriteLine($"Status Sekarang : {complaint.Status}");

            Console.WriteLine("\nSimulasi selesai.");
        }
    }
}