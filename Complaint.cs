using System;
using TUBES_KPL.Core;

namespace TUBES_KPL.Models
{
    public class Complaint
    {
        public string Title { get; private set; }
        public string Category { get; private set; }
        public string Description { get; private set; }
        public string Location { get; private set; }
        public string Reporter { get; private set; }

        public ComplaintStatus Status { get; set; }

        public Complaint(
            string title,
            string category,
            string description,
            string location,
            string reporter)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Judul tidak boleh kosong");

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Deskripsi tidak boleh kosong");

            Title = title;
            Category = category;
            Description = description;
            Location = location;
            Reporter = reporter;

            Status = ComplaintStatus.Diajukan;
        }
    }
}