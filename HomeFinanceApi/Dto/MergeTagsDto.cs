namespace HomeFinanceApi.Dto;

public record MergeTagsDto(string NewName, string[] OldNames, string ParentTagName);

