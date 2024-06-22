namespace Domain;

public class Transaction
{
    public int Id { get; set; }
    public string AccountId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Timestamp { get; set; }
    
    // reference to the account
    public Account Account { get; set; }
}