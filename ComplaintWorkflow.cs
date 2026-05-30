using System;
using System.Collections.Generic;
using TUBES_KPL.Models;

namespace TUBES_KPL.Core
{
    public class ComplaintWorkflow
    {
        private readonly Dictionary<(ComplaintStatus, string), ComplaintStatus> transitionTable;

        public ComplaintWorkflow()
        {
            transitionTable = new Dictionary<(ComplaintStatus, string), ComplaintStatus>
            {
                {(ComplaintStatus.Diajukan, "verify"), ComplaintStatus.Diverifikasi},
                {(ComplaintStatus.Diajukan, "reject"), ComplaintStatus.Ditolak},

                {(ComplaintStatus.Diverifikasi, "process"), ComplaintStatus.Diproses},

                {(ComplaintStatus.Diproses, "finish"), ComplaintStatus.Selesai}
            };
        }

        public void ChangeStatus(Complaint complaint, string action)
        {
            if (complaint == null)
                throw new ArgumentNullException(nameof(complaint));

            if (string.IsNullOrWhiteSpace(action))
                throw new ArgumentException("Action tidak boleh kosong");

            var key = (complaint.Status, action.ToLower());

            if (!transitionTable.ContainsKey(key))
            {
                throw new InvalidOperationException(
                    $"Transisi tidak valid dari {complaint.Status} dengan aksi {action}");
            }

            complaint.Status = transitionTable[key];
        }
    }
}