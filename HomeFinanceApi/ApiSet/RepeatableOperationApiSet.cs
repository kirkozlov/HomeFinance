using HomeFinance.Domain.DomainModels;
using HomeFinance.Domain.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeFinanceApi.Controllers;

public static class RepeatableOperationApiSet
{

    public static void Map(WebApplication app)
    {
        app.MapGet("api/operation/repeatable", RepeatableOperationApiSet.GetAll);
        app.MapGet("api/operation/repeatable/{id:guid}", RepeatableOperationApiSet.GetById);
        app.MapPost("api/operation/repeatable", Post);
        app.MapPut("api/operation/repeatable", Put);
        app.MapDelete("api/operation/repeatable/{id:guid}", Delete);
    }

    [Authorize]
    static async Task<IEnumerable<object>> GetAll(IGateway unitOfWork)
    {
        var operations = await unitOfWork.RepeatableOperationRepository.GetAll();
        return operations;
    }

    [Authorize]
    static async Task<object?> GetById([FromRoute] Guid id, IGateway unitOfWork)
    {
        var operation = await unitOfWork.RepeatableOperationRepository.GetByKey(id);
        return operation;
    }
    
    [Authorize]
    static async Task Post(RepeatableOperation operation, IGateway unitOfWork)
    {
        await unitOfWork.RepeatableOperationRepository.Add(operation);
    }
    
    [Authorize]
    static async Task Put(RepeatableOperation operation, IGateway unitOfWork)
    {
        await unitOfWork.RepeatableOperationRepository.Update(operation);
    }
    
    [Authorize]
    static async Task Delete(Guid id, IGateway unitOfWork)
    {
        await unitOfWork.RepeatableOperationRepository.Remove(id);
    }
}