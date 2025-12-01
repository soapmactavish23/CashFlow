using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;

namespace CashFlow.Application.UseCases.Expenses.GetAll
{
    public class GetAllExpenseUseCase(IExpensesReadOnlyRepository repository, IMapper mapper, ILoggerUser loggerUser) : IGetAllExpenseUseCase
    {

        private readonly ILoggerUser _loggerUser = loggerUser;
        private readonly IExpensesReadOnlyRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<ResponseExpensesJson> Execute()
        {
            var user = await _loggerUser.Get();
            var result = await _repository.GetAll(user);
            return new ResponseExpensesJson
            {
                Expenses = _mapper.Map<List<ResponseShortExpenseJson>>(result)
            };

        }
    }
}
