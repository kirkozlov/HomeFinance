using HomeFinance.Domain.DomainModels;
using HomeFinance.Domain.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeFinanceApi.Controllers;

public static class OperationApiSet
{

    public static void Map(WebApplication app)
    {
        app.MapGet("api/operation", OperationApiSet.Get);
        app.MapGet("api/operation/{walletId:guid}", OperationApiSet.GetForWallet);
        app.MapPost("api/operation", Post);
        app.MapPut("api/operation", Put);
        app.MapDelete("api/operation/{id:guid}", Delete);
    }

    [Authorize]
    static async Task<IEnumerable<object>> Get(IGateway unitOfWork)
    {
        var operations = await unitOfWork.OperationRepository.GetAll();
        return operations;
    }

    [Authorize]
    static async Task<IEnumerable<object>> GetForWallet(Guid walletId, IGateway unitOfWork)
    {
        var operations = await unitOfWork.OperationRepository.GetForWallet(walletId);
        return operations;
    }


    [Authorize]
    static async Task Post(Operation operation, IGateway unitOfWork)
    {
        await unitOfWork.OperationRepository.Add(operation);
    }


    [Authorize]
    static async Task Put(Operation operation, IGateway unitOfWork)
    {
        await unitOfWork.OperationRepository.Update(operation);
    }


    [Authorize]
    static async Task Delete(Guid id, IGateway unitOfWork)
    {
        await unitOfWork.OperationRepository.Remove(id);
    }
}