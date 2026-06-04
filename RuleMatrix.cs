using System;
using System.Collections.Generic;
using TUBES_KPL.Models;

namespace TUBES_KPL.Core
{
    public class RuleMatrix
    {
        private readonly Dictionary<string, string> assignmentTable = new Dictionary<string, string>();
        private readonly Dictionary<string, int> slaTable = new Dictionary<string, int>();
        private readonly Dictionary<string, string> escalationTable = new Dictionary<string, string>();
        private readonly Dictionary<string, string> notificationTable = new Dictionary<string, string>();

        private string K(string a, string b)
        {
            return $"{a.Trim().ToLower()}|{b.Trim().ToLower()}";
        }

        public RuleMatrix()
        {
            assignmentTable[K("Kebersihan", "Ringan")] = "Unit Kebersihan";
            assignmentTable[K("Kebersihan", "Sedang")] = "Unit Kebersihan";
            assignmentTable[K("Kebersihan", "Berat")] = "Unit Kebersihan";

            assignmentTable[K("Keamanan", "Ringan")] = "Unit Keamanan";
            assignmentTable[K("Keamanan", "Sedang")] = "Unit Keamanan";
            assignmentTable[K("Keamanan", "Berat")] = "Unit Keamanan";

            assignmentTable[K("Infrastruktur", "Ringan")] = "Unit Infrastruktur";
            assignmentTable[K("Infrastruktur", "Sedang")] = "Unit Infrastruktur";
            assignmentTable[K("Infrastruktur", "Berat")] = "Unit Infrastruktur";

            assignmentTable[K("Administrasi", "Ringan")] = "Unit Administrasi";
            assignmentTable[K("Administrasi", "Sedang")] = "Unit Administrasi";
            assignmentTable[K("Administrasi", "Berat")] = "Unit Administrasi";

            assignmentTable[K("Umum", "Ringan")] = "Unit Umum";
            assignmentTable[K("Umum", "Sedang")] = "Unit Umum";
            assignmentTable[K("Umum", "Berat")] = "Unit Umum";

            SlaConfig config = ConfigLoader.Load<SlaConfig>("sla_rules.json");

            foreach (var rule in config.Rules)
            {
                slaTable[K(rule.Category, rule.Impact)] = rule.MaxDays;
            }

            escalationTable[$"{ComplaintStatus.Diajukan}|2"] = "Notifikasi ke Kabid";
            escalationTable[$"{ComplaintStatus.Diverifikasi}|3"] = "Eskalasi ke Lurah";
            escalationTable[$"{ComplaintStatus.Diproses}|4"] = "Laporan ke Camat";

            NotificationConfig notifConfig = ConfigLoader.Load<NotificationConfig>("notification_templates.json");
        }

        public string GetUnit(string category, string impact)
        {
            return assignmentTable.TryGetValue(K(category, impact), out var val)
                ? val
                : "Unit Umum";
        }

        public int GetSLADays(string category, string impact)
        {
            return slaTable.TryGetValue(K(category, impact), out var val)
                ? val
                : 3;
        }

        public string CheckEscalation(ComplaintStatus status, int delayDays)
        {
            return escalationTable.TryGetValue($"{status}|{delayDays}", out var val)
                ? val
                : "Tidak Ada Eskalasi";
        }

        public string GetNotificationTemplate(ComplaintStatus status, string role)
        {
            return notificationTable.TryGetValue(K(status.ToString(), role), out var val)
                ? val
                : "Update status pengaduan Anda.";
        }
    }
}