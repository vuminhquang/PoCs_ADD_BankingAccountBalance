namespace Domain;

public interface ITransactionService
{
    Task ExecuteTransactionAsync(string accountId, decimal amount);
}