namespace Domain;

public class Account
{
    public string Id { get; internal set; }
    public decimal Balance { get; internal set; }
    
    public void ApplyTransaction(decimal amount)
    {
        if (Balance + amount < 0)
        {
            throw new InvalidOperationException("Insufficient funds");
        }

        Balance += amount;
    }
}