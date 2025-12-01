using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;

namespace CashFlow.Application.UseCases.Expenses.GetById
{
    public class GetExpenseByIdUseCase : IGetExpenseByIdUseCase
    {

        private readonly ILoggerUser _loggerUser;
        private readonly IExpensesReadOnlyRepository _repository;
        private readonly IMapper _mapper;

        public GetExpenseByIdUseCase(IExpensesReadOnlyRepository repository, IMapper mapper, ILoggerUser loggerUser)
        {
            _repository = repository;
            _mapper = mapper;
            _loggerUser = loggerUser;
        }

        public async Task<ResponseExpenseJson> Execute(long id)
        {
            var user = await _loggerUser.Get();

            var result = await _repository.GetById(user, id);

            return _mapper.Map<ResponseExpenseJson>(result);
        }
    }
}
