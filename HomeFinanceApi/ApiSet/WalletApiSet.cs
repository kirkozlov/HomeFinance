﻿using HomeFinance.Domain.DomainModels;
using HomeFinance.Domain.Utils;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HomeFinanceApi.Controllers;

public static class WalletApiSet
{

    public static void Map(WebApplication app)
    {
        app.MapGet("api/wallet", Get);
        app.MapPost("api/wallet", Post);
        app.MapPost("api/wallet/range", PostRange);
        app.MapPut("api/wallet", Put);
        app.MapDelete("api/wallet/{id:guid}", Delete);
    }

    [Authorize]
    static async Task<IEnumerable<object>> Get(IGateway unitOfWork)
    {
        var wallets = await unitOfWork.WalletRepository.GetAll();
        
        return wallets.Select(i => new {
            id = i.Id ?? throw new ApplicationException(),
            name = i.Name,
            groupName = i.GroupName,
            comment = i.Comment,
            balance = unitOfWork.OperationRepository.GetSumFor(i.Id.Value)
        });
    }


    [Authorize]
    static async Task<Wallet> Post(Wallet wallet, IGateway unitOfWork)
    {
        return await unitOfWork.WalletRepository.Add(wallet);
    }

    [Authorize]
    static async Task<IEnumerable<Wallet>> PostRange(Wallet[] wallets, IGateway unitOfWork)
    {
        return await unitOfWork.WalletRepository.AddRange(wallets);
    }


    [Authorize]
    static async Task<Wallet> Put(Wallet wallet, IGateway unitOfWork)
    {
        return await unitOfWork.WalletRepository.Update(wallet);
    }


    [Authorize]
    static async Task Delete(Guid id, IGateway unitOfWork)
    {
        await unitOfWork.WalletRepository.Remove(id);
    }
}