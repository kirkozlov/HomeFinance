using HomeFinance.Domain.Repositories;
using HomeFinance.Domain.DomainModels;
using HomeFinance.Domain.Services;

namespace HomeFinance.Domain.Utils;

public interface IGateway
{
    IUserDependentRepository<Tag, string> TagRepository { get; }
    IOperationRepository OperationRepository { get; }
    IUserDependentRepository<Wallet, Guid> WalletRepository { get; }
    IUserDependentRepository<RepeatableOperation, Guid> RepeatableOperationRepository { get; }
    IUserDependentRepository<TransientOperation, Guid> TransientOperationRepository { get; }
    IMergeTagsService MergeTagsService { get; }
}