namespace HomeFinance.Domain.Services;

public interface IRepetableService
{
    Task FindAndExcecuteRepeatableOperation();
}