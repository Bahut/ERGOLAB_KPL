using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;

namespace TUBES_KPL.Testing
{
    [TestClass]
    public class ConfigLoaderPerformanceTest
    {
        private static readonly string JsonDir = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "..", "..", "..", ".."
        );

        private string JsonPath(string fileName) =>
            Path.GetFullPath(Path.Combine(JsonDir, fileName));

        // ── Batas waktu yang dianggap acceptable ────────────────────
        private const int MaxSingleLoadMs   = 200;  // 1x load < 200ms
        private const int MaxRepeatedLoadMs = 500;  // 100x load < 500ms

        // ── Performance: Load tunggal ───────────────────────────────

        [TestMethod]
        public void Performance_LoadCategories_SingleLoad_FastEnough()
        {
            var sw = Stopwatch.StartNew();
            ConfigLoader.Load<CategoryConfig>(JsonPath("categories.json"));
            sw.Stop();

            Console.WriteLine($"Load categories (1x): {sw.ElapsedMilliseconds}ms");
            Assert.IsTrue(sw.ElapsedMilliseconds < MaxSingleLoadMs,
                $"Load terlalu lambat: {sw.ElapsedMilliseconds}ms (maks {MaxSingleLoadMs}ms)");
        }

        [TestMethod]
        public void Performance_LoadRoles_SingleLoad_FastEnough()
        {
            var sw = Stopwatch.StartNew();
            ConfigLoader.Load<RoleConfig>(JsonPath("role_permission.json"));
            sw.Stop();

            Console.WriteLine($"Load roles (1x): {sw.ElapsedMilliseconds}ms");
            Assert.IsTrue(sw.ElapsedMilliseconds < MaxSingleLoadMs,
                $"Load terlalu lambat: {sw.ElapsedMilliseconds}ms (maks {MaxSingleLoadMs}ms)");
        }

        [TestMethod]
        public void Performance_LoadSlaRules_SingleLoad_FastEnough()
        {
            var sw = Stopwatch.StartNew();
            ConfigLoader.Load<SlaConfig>(JsonPath("sla_rules.json"));
            sw.Stop();

            Console.WriteLine($"Load SLA rules (1x): {sw.ElapsedMilliseconds}ms");
            Assert.IsTrue(sw.ElapsedMilliseconds < MaxSingleLoadMs,
                $"Load terlalu lambat: {sw.ElapsedMilliseconds}ms (maks {MaxSingleLoadMs}ms)");
        }

        // ── Performance: Load berulang (stress test) ────────────────

        [TestMethod]
        public void Performance_LoadCategories_100Times_FastEnough()
        {
            // Warmup 1x agar JIT tidak dihitung
            ConfigLoader.Load<CategoryConfig>(JsonPath("categories.json"));

            var sw = Stopwatch.StartNew();
            for (int i = 0; i < 100; i++)
                ConfigLoader.Load<CategoryConfig>(JsonPath("categories.json"));
            sw.Stop();

            Console.WriteLine($"Load categories (100x): {sw.ElapsedMilliseconds}ms");
            Assert.IsTrue(sw.ElapsedMilliseconds < MaxRepeatedLoadMs,
                $"100x load terlalu lambat: {sw.ElapsedMilliseconds}ms (maks {MaxRepeatedLoadMs}ms)");
        }

        [TestMethod]
        public void Performance_LoadAllConfigs_Sequential_FastEnough()
        {
            var sw = Stopwatch.StartNew();

            ConfigLoader.Load<CategoryConfig>(JsonPath("categories.json"));
            ConfigLoader.Load<RoleConfig>(JsonPath("role_permission.json"));
            ConfigLoader.Load<SlaConfig>(JsonPath("sla_rules.json"));
            ConfigLoader.Load<NotificationConfig>(JsonPath("notification_templates.json"));

            sw.Stop();

            Console.WriteLine($"Load semua config (sequential): {sw.ElapsedMilliseconds}ms");
            Assert.IsTrue(sw.ElapsedMilliseconds < MaxSingleLoadMs * 4,
                $"Load semua config terlalu lambat: {sw.ElapsedMilliseconds}ms");
        }

        // ── Performance: File tidak ada (harus cepat gagal) ─────────

        [TestMethod]
        public void Performance_FileNotFound_FailsFast()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                ConfigLoader.Load<CategoryConfig>("tidak_ada.json");
            }
            catch (FileNotFoundException) { }
            sw.Stop();

            Console.WriteLine($"FileNotFound fail time: {sw.ElapsedMilliseconds}ms");
            Assert.IsTrue(sw.ElapsedMilliseconds < 50,
                $"Gagal terlalu lambat: {sw.ElapsedMilliseconds}ms (maks 50ms)");
        }
    }
}
