namespace HomeFinance.Domain.Repositories;

public interface IUserDependentCollectionRepository<T, in TKey>: IUserDependentRepository<T>
{
    public Task<T?> GetByKey(TKey key);
    public Task<IEnumerable<T>> AddRange(IEnumerable<T> domain);
    public Task<IEnumerable<T>> Update(IEnumerable<T> domain);
    public Task Remove(TKey key);
}