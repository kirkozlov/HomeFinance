using HomeFinance.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeFinance.Domain.Utils
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository CategoryRepository { get; }
        IOperationRepository OperationRepository { get; }
        IWalletRepository WalletRepository { get; }
        IRepeatableOperationRepository RepeatableOperationRepository { get; }

        public void Rollback();
        public void Commit();

    }
}
