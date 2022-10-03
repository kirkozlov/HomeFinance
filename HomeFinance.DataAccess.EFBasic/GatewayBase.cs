using System;
using System.Collections.Generic;
using HomeFinance.Domain.Repositories;
using HomeFinance.Domain.Utils;
using HomeFinance.DataAccess.EFBasic;
using HomeFinance.DataAccess.EFBasic.Repositories;
using HomeFinance.Domain.DomainModels;

namespace HomeFinance.DataAccess;

public  class Gateway : IGateway
{

    Lazy<IUserDependentRepository<Tag, string>> _categoryRepository;
    Lazy<IOperationRepository> _operationRepository;
    Lazy<IUserDependentRepository<Wallet, Guid>> _walletRepository;
    Lazy<IUserDependentRepository<RepeatableOperation, Guid>> _repeatableOperationRepository;
    public IUserDependentRepository<Tag, string> TagRepository => _categoryRepository.Value;
    public IOperationRepository OperationRepository =>_operationRepository.Value;
    public IUserDependentRepository<Wallet, Guid> WalletRepository =>_walletRepository.Value;
    public IUserDependentRepository<RepeatableOperation, Guid> RepeatableOperationRepository =>_repeatableOperationRepository.Value;

    readonly HomeFinanceContextBase _homeFinanceContext;

    public Gateway(HomeFinanceContextBase homeFinanceContext)
    {
        _homeFinanceContext = homeFinanceContext;

        _categoryRepository = new Lazy<IUserDependentRepository<Tag, string>>(() => new TagRepository(_homeFinanceContext));
        _operationRepository = new Lazy<IOperationRepository>(()=>new OperationRepository(_homeFinanceContext));
        _walletRepository = new Lazy<IUserDependentRepository<Wallet, Guid>>(() => new WalletRepository(_homeFinanceContext));
        _repeatableOperationRepository =new Lazy<IUserDependentRepository<RepeatableOperation, Guid>>(()=>new RepeatableOperationRepository(_homeFinanceContext));

    }

        
}