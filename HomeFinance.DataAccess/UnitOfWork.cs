using System;
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
        public ICategoryRepository CategoryRepository { get;  }

        public IOperationRepository OperationRepository { get;  }

        public IWalletRepository WalletRepository { get;  }

        public IRepeatableOperationRepository RepeatableOperationRepository { get; }

        readonly HomeFinanceContext _homeFinanceContext;
        readonly IDbContextTransaction _transaction;

        public UnitOfWork(HomeFinanceContext homeFinanceContext)
        {
            _homeFinanceContext = homeFinanceContext;


            CategoryRepository = new CategoryRepository(homeFinanceContext);
            OperationRepository = new OperationRepository(homeFinanceContext);
            WalletRepository = new WalletRepository(homeFinanceContext);
            RepeatableOperationRepository = new RepeatableOperationRepository(homeFinanceContext);

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
