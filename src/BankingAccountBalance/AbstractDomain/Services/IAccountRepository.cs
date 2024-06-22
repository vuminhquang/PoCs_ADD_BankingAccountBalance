namespace Domain;

public interface IAccountRepository
{
    Task<Account> GetByIdAsync(string accountId);
    Task UpdateAsync(Account account);
}