namespace HomeFinance.Domain.Services;

public interface IRepeatableService
{
    Task FindAndExecuteRepeatableOperation();
}