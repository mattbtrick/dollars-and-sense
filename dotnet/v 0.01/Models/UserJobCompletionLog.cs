namespace Models
{
    public class UserJobCompletionLog
    {
        public long JobId { get; set; }
        public long UserId { get; set; }
        public decimal Wage { get; set; }
        public DateTime DateCompleted { get; set; }
        public long PaidByUserId { get; set; }
        public DateTime DatePaid { get; set; }
    }
}
