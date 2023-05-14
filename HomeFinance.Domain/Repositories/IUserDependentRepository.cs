namespace HomeFinance.Domain.Repositories;

public interface IUserDependentRepository<T>
{
    public Task<List<T>> GetAll();
    public Task<T> Add(T domain);
    public Task<T> Update(T domain);
}
