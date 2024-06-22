namespace Contract;

public class TransactionCreatedEvent
{
    public string AccountId { get; set; }
    public decimal Amount { get; set; }
    public string TransactionType { get; set; } // "credit" or "debit"
}