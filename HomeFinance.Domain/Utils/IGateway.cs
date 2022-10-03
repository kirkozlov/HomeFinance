using HomeFinance.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeFinance.Domain.DomainModels;

namespace HomeFinance.Domain.Utils;

public interface IGateway
{
    IUserDependentRepository<Tag, string> TagRepository { get; }
    IOperationRepository OperationRepository { get; }
    IUserDependentRepository<Wallet, Guid> WalletRepository { get; }
    IUserDependentRepository<RepeatableOperation, Guid> RepeatableOperationRepository { get; }
}