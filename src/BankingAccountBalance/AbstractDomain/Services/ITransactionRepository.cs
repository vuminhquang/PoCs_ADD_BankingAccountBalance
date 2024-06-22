namespace Domain;

public interface ITransactionRepository
{
    Task AddAsync(Transaction transaction);
}