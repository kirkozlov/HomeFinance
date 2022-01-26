using HomeFinance.Domain.Dtos;
using HomeFinance.Domain.Models;

namespace HomeFinance.Domain.Repositories
{
    public interface ICategoryRepository: IUserDependentRepository<CategoryDto>
    {
    }
}
 