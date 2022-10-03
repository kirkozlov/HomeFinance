namespace HomeFinance.Domain.Repositories;

public interface IUserDependentRepository<T, in TKey>
{
    public Task<List<T>> GetAll(string userId);
    public Task<T?> GetByKey(TKey key, string userId);
    public Task<T> Add(T domain, string userId);
    public Task<T> Update(T domain, string userId);
    public Task Remove(TKey key, string userId);
}