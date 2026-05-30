using System;
using TUBES_KPL.Core;
using TUBES_KPL.Models;

namespace TUBES_KPL.Testing
{
    public class UnitTestSimulation
    {
        public static void Run()
        {
            ComplaintWorkflow workflow = new ComplaintWorkflow();

            Complaint complaint = new Complaint(
                "Lampu Jalan Mati",
                "Listrik",
                "Lampu mati sejak kemarin",
                "Jl. Asia Afrika",
                "Budi"
            );

            workflow.ChangeStatus(complaint, "verify");

            if (complaint.Status != ComplaintStatus.Diverifikasi)
            {
                throw new Exception("Unit Test Gagal");
            }

            Console.WriteLine("Unit Test Berhasil");
        }
    }
}