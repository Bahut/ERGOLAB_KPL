using System.Collections.Generic;

namespace TUBES_KPL.Models
{
    public class SlaConfig
    {
        public List<SlaRule> Rules { get; set; }
    }

    public class SlaRule
    {
        public string Category { get; set; }
        public string Impact { get; set; }
        public int MaxDays { get; set; }
    }
}