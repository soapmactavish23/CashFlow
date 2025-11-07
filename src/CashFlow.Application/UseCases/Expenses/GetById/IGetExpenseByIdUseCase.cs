using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.Expenses.GetById
{
    public interface IGetExpenseByIdUseCase
    {
        public Task<ResponseExpensesJson> Execute(long id);
    }
}
