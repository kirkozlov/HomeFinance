namespace HomeFinance.Domain.Repositories;

public interface IUserDependentRepository<T, in TKey>
{
    public Task<List<T>> GetAll();
    public Task<T?> GetByKey(TKey key);
    public Task<T> Add(T domain);
    public Task<T> Update(T domain);
    public Task Remove(TKey key);
}