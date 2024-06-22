using Domain;
using Microsoft.Extensions.Caching.Distributed;

namespace Infrastructure;

using StackExchange.Redis;
using Microsoft.Extensions.Caching.Distributed;

public class TransactionService : ITransactionService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IConnectionMultiplexer _redis;

    public TransactionService(IAccountRepository accountRepository, IConnectionMultiplexer redis)
    {
        _accountRepository = accountRepository;
        _redis = redis;
    }

    public async Task ExecuteTransactionAsync(string accountId, decimal amount)
    {
        var lockKey = $"lock-account-{accountId}";
        var lockValue = Guid.NewGuid().ToString();
        var db = _redis.GetDatabase();
        
        while (!await db.LockTakeAsync(lockKey, lockValue, TimeSpan.FromSeconds(30)))
        {
            await Task.Delay(100); // Wait for 100 ms before trying again
        }

        try
        {
            var account = await _accountRepository.GetByIdAsync(accountId);
            if (account == null) throw new Exception("Account not found");

            account.ApplyTransaction(amount);
            await _accountRepository.UpdateAsync(account);
        }
        finally
        {
            await db.LockReleaseAsync(lockKey, lockValue);
        }
    }
}