using HomeFinance.Domain.DomainModels;
using HomeFinance.Domain.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeFinanceApi.Controllers;

public static class TransientOperationApiSet
{
    public static void Map(WebApplication app)
    {
        app.MapGet("api/transient", TransientOperationApiSet.GetAll);
        app.MapGet("api/transient/{id:guid}", TransientOperationApiSet.GetById);
        app.MapPost("api/transient", Post);
        app.MapPost("api/transient/range", PostRange);
        app.MapPut("api/transient", Put);
        app.MapDelete("api/transient/{id:guid}", Delete);
    }

    [Authorize]
    static async Task<IEnumerable<object>> GetAll(IGateway unitOfWork)
    {

        var operations = await unitOfWork.TransientOperationRepository.GetAll();
        return operations;
    }

    [Authorize]
    static async Task<object?> GetById([FromRoute] Guid id, IGateway unitOfWork)
    {
        var operation = await unitOfWork.TransientOperationRepository.GetByKey(id);
        return operation;
    }
    
    [Authorize]
    static async Task Post(TransientOperation operation, IGateway unitOfWork)
    {
        await  unitOfWork.TransientOperationRepository.Add(operation);
    }

    [Authorize]
    static async Task PostRange(TransientOperation[] operations, IGateway unitOfWork)
    {
        await unitOfWork.TransientOperationRepository.AddRange(operations);
    }


    [Authorize]
    static async Task Put(TransientOperation operation, IGateway unitOfWork)
    {
        await unitOfWork.TransientOperationRepository.Update(operation);
    }


    [Authorize]
    static async Task Delete(Guid id, IGateway unitOfWork)
    {
        await unitOfWork.TransientOperationRepository.Remove(id);
    }
}