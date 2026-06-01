using System;
using TUBES_KPL.Core;

namespace TUBES_KPL.Testing
{
    public class TableDrivenUnitTest
    {
        public static void Run()
        {
            Console.WriteLine("Testing 4 Matriks Table-Driven");

            RuleMatrix rules = new RuleMatrix();

            // 1. Matriks Penugasan
            string unit = rules.GetUnit("Infrastruktur", "Berat");

            if (unit != "Unit Infrastruktur")
                throw new Exception("Matriks Penugasan Gagal");

            Console.WriteLine($"[OK] Penugasan : Infrastruktur + Berat -> {unit}");

            // 2. Matriks SLA
            int sla = rules.GetSLADays("Keamanan", "Sedang");

            if (sla != 1)
                throw new Exception("Matriks SLA Gagal");

            Console.WriteLine($"[OK] SLA : Keamanan + Sedang -> {sla} hari");

            // 3. Matriks Eskalasi
            string escalation =
                rules.CheckEscalation(ComplaintStatus.Diverifikasi, 3);

            if (!escalation.Contains("Lurah"))
                throw new Exception("Matriks Eskalasi Gagal");

            Console.WriteLine($"[OK] Eskalasi : {escalation}");

            // 4. Matriks Notifikasi
            string notification =
                rules.GetNotificationTemplate(
                    ComplaintStatus.Diajukan,
                    "Warga");

            if (string.IsNullOrWhiteSpace(notification))
                throw new Exception("Matriks Notifikasi Gagal");

            Console.WriteLine($"[OK] Notifikasi : {notification}");

            Console.WriteLine("\nSemua Unit Test Table-Driven Berhasil!");
        }
    }
}