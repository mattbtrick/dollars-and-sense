namespace Models
{
    public class UserExpense
    {
        public long ExpenseId { get; set; }
        public long UserId { get; set; }
        public long PaymentScheduleId { get; set; }
    }
}
