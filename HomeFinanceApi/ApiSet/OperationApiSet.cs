using HomeFinance.Domain.DomainModels;
using HomeFinance.Domain.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeFinanceApi.Controllers;

public static class OperationApiSet
{

    public static void Map(WebApplication app)
    {
        app.MapGet("api/operation", OperationApiSet.GetAll);
        app.MapGet("api/operation/{id:guid}", OperationApiSet.GetById);
        app.MapGet("api/operation/wallet/{walletId:guid}", OperationApiSet.GetForWallet);
        app.MapPost("api/operation", Post);
        app.MapPost("api/operation/range", PostRange);
        app.MapPut("api/operation", Put);
        app.MapDelete("api/operation/{id:guid}", Delete);
    }

    [Authorize]
    static async Task<IEnumerable<object>> GetAll([FromQuery] Guid? walletId, [FromQuery] long? from, [FromQuery] long? to, IGateway unitOfWork)
    {
        var fromDt = from.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(from.Value).LocalDateTime : (DateTime?)null;
        var toDt = to.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(to.Value).LocalDateTime : (DateTime?)null;
        var operations = await unitOfWork.OperationRepository.GetForWalletAndPeriod(walletId, fromDt, toDt);
        return operations;
    }

    [Authorize]
    static async Task<object?> GetById([FromRoute] Guid id, IGateway unitOfWork)
    {
        var operation = await unitOfWork.OperationRepository.GetByKey(id);
        return operation;
    }

    [Authorize]
    static async Task<IEnumerable<object>> GetForWallet(Guid walletId, IGateway unitOfWork)
    {
        var operations = await unitOfWork.OperationRepository.GetForWalletAndPeriod(walletId, null, null);
        return operations;
    }


    [Authorize]
    static async Task Post(Operation operation, IGateway unitOfWork)
    {
        await unitOfWork.OperationRepository.Add(operation);
    }

    [Authorize]
    static async Task PostRange(Operation[] operations, IGateway unitOfWork)
    {
        await unitOfWork.OperationRepository.AddRange(operations);
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