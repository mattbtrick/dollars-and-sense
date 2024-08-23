namespace Models
{
    public class Expense
    {
        public long ExpenseId { get; set; }
        public string DisplayText { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}
