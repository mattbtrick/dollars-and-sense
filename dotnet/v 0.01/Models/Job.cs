namespace Models
{
    public class Job
    {
        public long JobId { get; set; }
        public string DisplayText { get; set; } = string.Empty;
        public string Instructions { get; set; } = string.Empty;
        public decimal Wage { get; set; }
        public List<JobRule> Rules { get; set; } = new List<JobRule>();
        public List<JobAssignment> Assignments { get; set; } = new List<JobAssignment>();
    }
}
