﻿using HomeFinance.Domain.Utils;
using HomeFinanceApi.Dto;
using Microsoft.AspNetCore.Authorization;
using Tag = HomeFinance.Domain.DomainModels.Tag;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HomeFinanceApi.Controllers;


public static class TagApiSet
{

    public static void Map(WebApplication app)
    {
        app.MapGet("api/tag", TagApiSet.Get);
        app.MapPost("api/tag", Post);
        app.MapPost("api/tag/merge", Merge);
        app.MapPut("api/tag", Put);
        app.MapPut("api/tag/multiple", PutMultiple);
        app.MapDelete("api/tag/{name}", Delete);
    }

    [Authorize]
    static async Task<IEnumerable<object>> Get(IGateway unitOfWork)
    {
        var tags = await unitOfWork.TagRepository.GetAll();

        return tags;
    }

  
    [Authorize]
    static async Task Post(Tag tag, IGateway unitOfWork)
    {
        await unitOfWork.TagRepository.Add(tag);
    }


    [Authorize]
    static async Task Put(Tag tag, IGateway unitOfWork)
    {
        await unitOfWork.TagRepository.Update(tag);
    }

    [Authorize]
    static async Task PutMultiple(Tag[] tags, IGateway unitOfWork)
    {
        await unitOfWork.TagRepository.Update(tags);
    }

    [Authorize]
    static async Task Merge(MergeTagsDto dto, IGateway unitOfWork)
    {
        await unitOfWork.MergeTagsService.MergeTags(dto.NewName, dto.OldNames);
    }


    [Authorize]
    static async Task Delete(string name, IGateway unitOfWork)
    {
        await unitOfWork.TagRepository.Remove(name);
    }
}