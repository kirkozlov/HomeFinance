using HomeFinanace.DataAccess.Core.DBModels;
using HomeFinance.Domain.DomainModels;
using HomeFinance.Domain.Utils;
using HomeFinanceApi.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tag = HomeFinance.Domain.DomainModels.Tag;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HomeFinanceApi.Controllers;


public static class TagApiSet
{

    public static void Map(WebApplication app)
    {
        app.MapGet("api/tag", TagApiSet.Get);
        app.MapPost("api/tag", Post);
        app.MapPut("api/tag", Put);
        app.MapDelete("api/tag/{name}", Delete);
    }

    [Authorize]
    static async Task<IEnumerable<object>> Get(IGateway unitOfWork, UserService userService)
    {
        var tags = await unitOfWork.TagRepository.GetAll(userService.UserId);

        return tags;
    }

  
    [Authorize]
    static async Task Post(Tag tag, IGateway unitOfWork, UserService userService)
    {
        await unitOfWork.TagRepository.Add(tag, userService.UserId);
    }


    [Authorize]
    static async Task Put(Tag tag, IGateway unitOfWork, UserService userService)
    {
        await unitOfWork.TagRepository.Update(tag, userService.UserId);
    }


    [Authorize]
    static async Task Delete(string name, IGateway unitOfWork, UserService userService)
    {
        await unitOfWork.TagRepository.Remove(name, userService.UserId);
    }
}