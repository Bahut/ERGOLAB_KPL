using Microsoft.VisualStudio.TestTools.UnitTesting;
using TUBES_KPL.Core;

namespace TUBES_KPL.Testing
{
    [TestClass]
    public class TableDrivenUnitTest
    {
        [TestMethod]
        public void TestAssignmentMatrix()
        {
            RuleMatrix rules = new RuleMatrix();

            string unit = rules.GetUnit("Infrastruktur", "Berat");

            Assert.AreEqual(
                "Unit Infrastruktur",
                unit
            );
        }

        [TestMethod]
        public void TestSLAMatrix()
        {
            RuleMatrix rules = new RuleMatrix();

            int sla = rules.GetSLADays(
                "Keamanan",
                "Sedang"
            );

            Assert.AreEqual(1, sla);
        }

        [TestMethod]
        public void TestEscalationMatrix()
        {
            RuleMatrix rules = new RuleMatrix();

            string escalation =
                rules.CheckEscalation(
                    ComplaintStatus.Diverifikasi,
                    3
                );

            Assert.IsTrue(
                escalation.Contains("Lurah")
            );
        }

        [TestMethod]
        public void TestNotificationMatrix()
        {
            RuleMatrix rules = new RuleMatrix();

            string notification =
                rules.GetNotificationTemplate(
                    ComplaintStatus.Diajukan,
                    "Warga"
                );

            Assert.IsFalse(
                string.IsNullOrWhiteSpace(notification)
            );
        }
    }
}