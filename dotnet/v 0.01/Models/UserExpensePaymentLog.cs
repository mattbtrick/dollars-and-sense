namespace Models
{
    public class UserExpensePaymentLog
    {
        public long ExpenseId { get; set; }
        public long UserId { get; set; }
        public decimal AmountPaid { get; set; }
        public DateTime DatePaid { get; set; }
    }
}
