using Contract;
using Domain;

namespace BusinessApplication;

public class TransactionProcessor : ITransactionProcessor
{
    private readonly ITransactionService _transactionService;
    private readonly ITransactionRepository _transactionRepository;

    public TransactionProcessor(ITransactionService transactionService, ITransactionRepository transactionRepository)
    {
        _transactionService = transactionService;
        _transactionRepository = transactionRepository;
    }

    public async Task ProcessTransactionAsync(TransactionDto transactionDto)
    {
        await _transactionService.ExecuteTransactionAsync(transactionDto.AccountId, transactionDto.Amount);

        var transaction = new Transaction
        {
            AccountId = transactionDto.AccountId,
            Amount = transactionDto.Amount,
            Timestamp = transactionDto.Timestamp
        };

        await _transactionRepository.AddAsync(transaction);
    }
}