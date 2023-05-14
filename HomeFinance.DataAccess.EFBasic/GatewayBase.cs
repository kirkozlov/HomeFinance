using HomeFinance.Domain.Repositories;
using HomeFinance.Domain.Utils;
using HomeFinance.DataAccess.EFBasic;
using HomeFinance.DataAccess.EFBasic.Repositories;
using HomeFinance.Domain.DomainModels;
using HomeFinance.Domain.Services;
using HomeFinance.DataAccess.EFBasic.Services;

namespace HomeFinance.DataAccess;

public  class Gateway : IGateway
{
    readonly Lazy<IUserDependentCollectionRepository<Tag, string>> _categoryRepository;
    readonly Lazy<IOperationRepository> _operationRepository;
    readonly Lazy<IUserDependentCollectionRepository<Wallet, Guid>> _walletRepository;
    readonly Lazy<IUserDependentCollectionRepository<RepeatableOperation, Guid>> _repeatableOperationRepository;
    readonly Lazy<IUserDependentCollectionRepository<TransientOperation, Guid>> _transientOperationRepository;
    readonly Lazy<IUserDependentRepository<UserPreferences>> _userPreferencesRepository;
    readonly Lazy<IMergeTagsService> _mergeTagService;
    public IUserDependentCollectionRepository<Tag, string> TagRepository => _categoryRepository.Value;
    public IOperationRepository OperationRepository =>_operationRepository.Value;
    public IUserDependentCollectionRepository<Wallet, Guid> WalletRepository =>_walletRepository.Value;
    public IUserDependentCollectionRepository<RepeatableOperation, Guid> RepeatableOperationRepository =>_repeatableOperationRepository.Value;
    public IUserDependentCollectionRepository<TransientOperation, Guid> TransientOperationRepository => _transientOperationRepository.Value;
    public IUserDependentRepository<UserPreferences> UserPreferencesRepository => _userPreferencesRepository.Value;
    public IMergeTagsService MergeTagsService=>_mergeTagService.Value;

    readonly HomeFinanceContextBase _homeFinanceContext;

    public Gateway(HomeFinanceContextBase homeFinanceContext, IUserService userService)
    {
        _homeFinanceContext = homeFinanceContext;

        _categoryRepository = new Lazy<IUserDependentCollectionRepository<Tag, string>>(() => new TagRepository(_homeFinanceContext, userService.UserId));
        _operationRepository = new Lazy<IOperationRepository>(()=>new OperationRepository(_homeFinanceContext, userService.UserId));
        _walletRepository = new Lazy<IUserDependentCollectionRepository<Wallet, Guid>>(() => new WalletRepository(_homeFinanceContext, userService.UserId));
        _repeatableOperationRepository =new Lazy<IUserDependentCollectionRepository<RepeatableOperation, Guid>>(()=>new RepeatableOperationRepository(_homeFinanceContext, userService.UserId));
        _transientOperationRepository = new Lazy<IUserDependentCollectionRepository<TransientOperation, Guid>>(() => new TransientOperationRepository(_homeFinanceContext, userService.UserId));
        _userPreferencesRepository = new Lazy<IUserDependentRepository<UserPreferences>>(() => new UserPreferencesRepository(_homeFinanceContext, userService.UserId));
        _mergeTagService = new Lazy<IMergeTagsService>(() => new MergeTagsService(_homeFinanceContext, userService.UserId));
    }

        
}