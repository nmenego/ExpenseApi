public class ExpenseClaim
{
    public ExpenseClaimDetail ExpenseClaimDetail { get; set; }
    public string Vendor { get; set; }

    public string Description { get; set; }

    public System.DateTime Date { get; set; }
}