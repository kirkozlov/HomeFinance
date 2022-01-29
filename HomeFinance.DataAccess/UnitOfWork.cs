﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeFinance.Domain.Repositories;
using HomeFinance.DataAccess.Repositories;
using HomeFinance.Domain.Utils;
using Microsoft.EntityFrameworkCore.Storage;

namespace HomeFinance.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {

        Lazy<ICategoryRepository> _categoryRepository;
        Lazy<IOperationRepository> _operationRepository;
        Lazy<IWalletRepository> _walletRepository;
        Lazy<IRepeatableOperationRepository> _repeatableOperationRepository;



        public ICategoryRepository CategoryRepository => _categoryRepository.Value;
        public IOperationRepository OperationRepository =>_operationRepository.Value;
        public IWalletRepository WalletRepository =>_walletRepository.Value;
        public IRepeatableOperationRepository RepeatableOperationRepository =>_repeatableOperationRepository.Value;



        readonly HomeFinanceContext _homeFinanceContext;
        readonly IDbContextTransaction _transaction;

        public UnitOfWork(HomeFinanceContext homeFinanceContext)
        {
            _homeFinanceContext = homeFinanceContext;


            _categoryRepository = new Lazy<ICategoryRepository>(() => new CategoryRepository(_homeFinanceContext));
            _operationRepository = new Lazy<IOperationRepository>(()=>new OperationRepository(_homeFinanceContext));
            _walletRepository = new Lazy<IWalletRepository>(() => new WalletRepository(_homeFinanceContext));
            _repeatableOperationRepository =new Lazy<IRepeatableOperationRepository>(()=>new RepeatableOperationRepository(_homeFinanceContext));

            _transaction = _homeFinanceContext.Database.BeginTransaction();
        }

        public void Dispose()
        {
            Commit();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public void  Commit()
        {
            _transaction.Commit();
        }
    }
}
