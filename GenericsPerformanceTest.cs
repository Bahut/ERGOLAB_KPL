using System;
using System.Collections.Generic;
using System.Diagnostics;
using TUBES_KPL.Core;
using TUBES_KPL.Models;

namespace TUBES_KPL.Testing
{
    public class PerformanceTestSimulation
    {
        public static void Run()
        {
            TestAddToRepository();
            TestPagedList();
            TestBatchValidation();
            TestResultCreation();

            Console.WriteLine("\nSemua Performance Test Berhasil!");
        }

        private static void TestAddToRepository()
        {
            var repo = new ComplaintRepository();
            var sw = Stopwatch.StartNew();

            for (int i = 0; i < 1000; i++)
            {
                var c = new Complaint(
                    $"Pengaduan {i}",
                    "Kebersihan",
                    "Ringan",
                    $"Deskripsi {i}",
                    "Bandung",
                    "Warga"
                );

                repo.Add(c);
            }

            sw.Stop();

            Console.WriteLine($"[PERF] Add 1000 items: {sw.ElapsedMilliseconds} ms");

            if (sw.ElapsedMilliseconds >= 3000)
                throw new Exception($"Test gagal: {sw.ElapsedMilliseconds} ms");
        }

        private static void TestPagedList()
        {
            var items = new List<Complaint>();

            for (int i = 0; i < 500; i++)
            {
                items.Add(
                    new Complaint(
                        $"Pengaduan {i}",
                        "Umum",
                        "Sedang",
                        $"Isi {i}",
                        "Jakarta",
                        "User"
                    )
                );
            }

            var sw = Stopwatch.StartNew();
            var paged = new PagedList<Complaint>(items, 1, 500, 500);
            sw.Stop();

            Console.WriteLine($"[PERF] PagedList 500 items: {sw.ElapsedMilliseconds} ms");

            if (paged.Items.Count != 500)
                throw new Exception("Jumlah item tidak sesuai");

            if (sw.ElapsedMilliseconds >= 1000)
                throw new Exception("PagedList terlalu lambat");
        }

        private static void TestBatchValidation()
        {
            var rules = new List<ValidationRule<string>>
            {
                new RequiredStringRule(),
                new MaxLengthRule(100)
            };

            var sw = Stopwatch.StartNew();

            for (int i = 0; i < 1000; i++)
            {
                var required = new RequiredStringRule();
                required.ValidateAll($"Input ke-{i}", rules, out _);
            }

            sw.Stop();

            Console.WriteLine($"[PERF] Batch validation 1000x: {sw.ElapsedMilliseconds} ms");

            if (sw.ElapsedMilliseconds >= 2000)
                throw new Exception("Batch validation terlalu lambat");
        }

        private static void TestResultCreation()
        {
            var sw = Stopwatch.StartNew();

            for (int i = 0; i < 1000; i++)
            {
                var c = new Complaint(
                    $"Judul {i}",
                    "Keamanan",
                    "Berat",
                    $"Desc {i}",
                    "Medan",
                    "User"
                );

                var result = Result<Complaint>.Ok(c, "OK");
            }

            sw.Stop();

            Console.WriteLine($"[PERF] Result<T> creation 1000x: {sw.ElapsedMilliseconds} ms");

            if (sw.ElapsedMilliseconds >= 3000)
                throw new Exception("Result<T> creation terlalu lambat");
        }
    }
}