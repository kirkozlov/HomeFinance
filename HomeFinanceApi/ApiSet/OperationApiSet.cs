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
    static async Task<IEnumerable<object>> Get(IGateway unitOfWork, UserService userService)
    {
        var operations = await unitOfWork.OperationRepository.GetAll(userService.UserId);
        return operations;
    }

    [Authorize]
    static async Task<IEnumerable<object>> GetForWallet(Guid walletId, IGateway unitOfWork, UserService userService)
    {
        var operations = await unitOfWork.OperationRepository.GetForWallet(userService.UserId,walletId);
        return operations;
    }


    [Authorize]
    static async Task Post(Operation operation, IGateway unitOfWork, UserService userService)
    {
        await unitOfWork.OperationRepository.Add(operation, userService.UserId);
    }


    [Authorize]
    static async Task Put(Operation operation, IGateway unitOfWork, UserService userService)
    {
        await unitOfWork.OperationRepository.Update(operation, userService.UserId);
    }


    [Authorize]
    static async Task Delete(Guid id, IGateway unitOfWork, UserService userService)
    {
        await unitOfWork.OperationRepository.Remove(id, userService.UserId);
    }
}