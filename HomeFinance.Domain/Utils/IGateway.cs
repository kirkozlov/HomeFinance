using HomeFinance.Domain.Repositories;
using HomeFinance.Domain.DomainModels;
using HomeFinance.Domain.Services;

namespace HomeFinance.Domain.Utils;

public interface IGateway
{
    IUserDependentCollectionRepository<Tag, string> TagRepository { get; }
    IOperationRepository OperationRepository { get; }
    IUserDependentCollectionRepository<Wallet, Guid> WalletRepository { get; }
    IUserDependentCollectionRepository<RepeatableOperation, Guid> RepeatableOperationRepository { get; }
    IUserDependentCollectionRepository<TransientOperation, Guid> TransientOperationRepository { get; }
    IUserDependentRepository<UserPreferences> UserPreferencesRepository { get; }
    IMergeTagsService MergeTagsService { get; }
}