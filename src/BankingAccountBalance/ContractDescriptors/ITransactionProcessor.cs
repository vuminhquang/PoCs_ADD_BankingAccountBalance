namespace Contract;

// Feature Interfaces
public interface ITransactionProcessor
{
    Task ProcessTransactionAsync(TransactionDto transaction);
}