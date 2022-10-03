using HomeFinance.Domain.DomainModels;
using HomeFinance.Domain.Utils;
using HomeFinanceApi.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HomeFinanceApi.Controllers;

public static class WalletApiSet
{

    public static void Map(WebApplication app)
    {
        app.MapGet("api/wallet", Get);
        app.MapPost("api/wallet", Post);
        app.MapPut("api/wallet", Put);
        app.MapDelete("api/wallet/{id:guid}", Delete);
    }

    [Authorize]
    static async Task<IEnumerable<object>> Get(IGateway unitOfWork, UserService userService)
    {
        var wallets = await unitOfWork.WalletRepository.GetAll(userService.UserId);
        
        var allOperations = (await unitOfWork.OperationRepository.GetAll(userService.UserId)).ToList();

        return wallets.Select(i => new {
            id = i.Id ?? throw new ApplicationException(),
            name = i.Name,
            groupName = i.GroupName,
            comment = i.Comment,
            balance = allOperations.GetSumFor(i.Id.Value)
        });
    }


    [Authorize]
    static async Task<Wallet> Post(Wallet wallet, IGateway unitOfWork, UserService userService)
    {
        return await unitOfWork.WalletRepository.Add(wallet, userService.UserId);
    }


    [Authorize]
    static async Task<Wallet> Put(Wallet wallet, IGateway unitOfWork, UserService userService)
    {
        return await unitOfWork.WalletRepository.Update(wallet, userService.UserId);
    }


    [Authorize]
    static async Task Delete(Guid id, IGateway unitOfWork, UserService userService)
    {
        await unitOfWork.WalletRepository.Remove(id, userService.UserId);
    }
}