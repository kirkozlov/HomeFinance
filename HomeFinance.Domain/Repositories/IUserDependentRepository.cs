namespace HomeFinance.Domain.Repositories
{
    public interface IUserDependentRepository<T>
    {
        public Task<List<T>> GetAll(string userId);
        public Task<T?> GetById(int id, string userId);
        public Task Add(T dto, string userId);
        public Task Update(T dto, string userId);
        public Task Remove(int id, string userId);
    }


}
 