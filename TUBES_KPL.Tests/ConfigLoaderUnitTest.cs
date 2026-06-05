using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace TUBES_KPL.Testing
{
    [TestClass]
    public class ConfigLoaderUnitTest
    {
        // Path ke folder JSON — relatif dari bin/Debug/net10.0 saat test jalan
        private static readonly string JsonDir = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "..", "..", "..", ".."
        );

        private string JsonPath(string fileName) =>
            Path.GetFullPath(Path.Combine(JsonDir, fileName));

        // ── Unit Test: Load berhasil ────────────────────────────────

        [TestMethod]
        public void Load_Categories_ReturnsValidData()
        {
            var config = ConfigLoader.Load<CategoryConfig>(JsonPath("categories.json"));

            Assert.IsNotNull(config);
            Assert.IsTrue(config.Categories.Count > 0);
        }

        [TestMethod]
        public void Load_Categories_ContainsExpectedNames()
        {
            var config = ConfigLoader.Load<CategoryConfig>(JsonPath("categories.json"));

            var names = config.Categories.ConvertAll(c => c.Name);
            CollectionAssert.Contains(names, "Kebersihan");
            CollectionAssert.Contains(names, "Infrastruktur");
        }

        [TestMethod]
        public void Load_Roles_ReturnsValidData()
        {
            var config = ConfigLoader.Load<RoleConfig>(JsonPath("role_permission.json"));

            Assert.IsNotNull(config);
            Assert.IsTrue(config.Roles.Count > 0);
        }

        [TestMethod]
        public void Load_Roles_EachRoleHasPermissions()
        {
            var config = ConfigLoader.Load<RoleConfig>(JsonPath("role_permission.json"));

            foreach (var role in config.Roles)
            {
                Assert.IsTrue(role.Permissions.Count > 0,
                    $"Role '{role.Role}' tidak punya permission.");
            }
        }

        [TestMethod]
        public void Load_SlaRules_ReturnsValidData()
        {
            var config = ConfigLoader.Load<SlaConfig>(JsonPath("sla_rules.json"));

            Assert.IsNotNull(config);
            Assert.IsTrue(config.Rules.Count > 0);
        }

        [TestMethod]
        public void Load_SlaRules_MaxDaysPositive()
        {
            var config = ConfigLoader.Load<SlaConfig>(JsonPath("sla_rules.json"));

            foreach (var rule in config.Rules)
            {
                Assert.IsTrue(rule.MaxDays > 0,
                    $"MaxDays untuk {rule.Category}/{rule.Impact} harus > 0.");
            }
        }

        [TestMethod]
        public void Load_NotificationTemplates_ReturnsValidData()
        {
            var config = ConfigLoader.Load<NotificationConfig>(JsonPath("notification_templates.json"));

            Assert.IsNotNull(config);
            Assert.IsTrue(config.Templates.Count > 0);
        }

        // ── Unit Test: File tidak ditemukan ─────────────────────────

        [TestMethod]
        public void Load_FileNotFound_ThrowsFileNotFoundException()
        {
            bool threw = false;
            try
            {
                ConfigLoader.Load<CategoryConfig>("file_yang_tidak_ada.json");
            }
            catch (FileNotFoundException)
            {
                threw = true;
            }
            Assert.IsTrue(threw, "Harus throw FileNotFoundException.");
        }

        // ── Unit Test: JSON tidak valid ─────────────────────────────

        [TestMethod]
        public void Load_InvalidJson_ThrowsException()
        {
            string tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "{ ini bukan json valid !!!");

            bool threw = false;
            try
            {
                ConfigLoader.Load<CategoryConfig>(tempFile);
            }
            catch (Exception)
            {
                threw = true;
            }
            finally
            {
                File.Delete(tempFile);
            }

            Assert.IsTrue(threw, "Harus throw Exception untuk JSON tidak valid.");
        }

        // ── Unit Test: JSON kosong / null result ────────────────────

        [TestMethod]
        public void Load_EmptyJson_ThrowsException()
        {
            string tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "null");

            bool threw = false;
            try
            {
                ConfigLoader.Load<CategoryConfig>(tempFile);
            }
            catch (Exception)
            {
                threw = true;
            }
            finally
            {
                File.Delete(tempFile);
            }

            Assert.IsTrue(threw, "Harus throw Exception untuk JSON null.");
        }
    }
}
