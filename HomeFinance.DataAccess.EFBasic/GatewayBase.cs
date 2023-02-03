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

    Lazy<IUserDependentRepository<Tag, string>> _categoryRepository;
    Lazy<IOperationRepository> _operationRepository;
    Lazy<IUserDependentRepository<Wallet, Guid>> _walletRepository;
    Lazy<IUserDependentRepository<RepeatableOperation, Guid>> _repeatableOperationRepository;
    Lazy<IMergeTagsService> _mergeTagService;
    public IUserDependentRepository<Tag, string> TagRepository => _categoryRepository.Value;
    public IOperationRepository OperationRepository =>_operationRepository.Value;
    public IUserDependentRepository<Wallet, Guid> WalletRepository =>_walletRepository.Value;
    public IUserDependentRepository<RepeatableOperation, Guid> RepeatableOperationRepository =>_repeatableOperationRepository.Value;

    public IMergeTagsService MergeTagsService=>_mergeTagService.Value;

    readonly HomeFinanceContextBase _homeFinanceContext;

    public Gateway(HomeFinanceContextBase homeFinanceContext, IUserService userService)
    {
        _homeFinanceContext = homeFinanceContext;

        _categoryRepository = new Lazy<IUserDependentRepository<Tag, string>>(() => new TagRepository(_homeFinanceContext, userService.UserId));
        _operationRepository = new Lazy<IOperationRepository>(()=>new OperationRepository(_homeFinanceContext, userService.UserId));
        _walletRepository = new Lazy<IUserDependentRepository<Wallet, Guid>>(() => new WalletRepository(_homeFinanceContext, userService.UserId));
        _repeatableOperationRepository =new Lazy<IUserDependentRepository<RepeatableOperation, Guid>>(()=>new RepeatableOperationRepository(_homeFinanceContext, userService.UserId));
        _mergeTagService = new Lazy<IMergeTagsService>(() => new MergeTagsService(_homeFinanceContext, userService.UserId));
    }

        
}