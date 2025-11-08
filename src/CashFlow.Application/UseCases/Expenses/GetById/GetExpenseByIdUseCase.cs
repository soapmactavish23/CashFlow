using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expenses;

namespace CashFlow.Application.UseCases.Expenses.GetById
{
    public class GetExpenseByIdUseCase : IGetExpenseByIdUseCase
    {

        private readonly IExpensesReadOnlyRepository _repository;
        private readonly IMapper _mapper;

        public GetExpenseByIdUseCase(IExpensesReadOnlyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ResponseExpenseJson> Execute(long id)
        {
            var result = await _repository.GetById(id);

            return _mapper.Map<ResponseExpenseJson>(result);
        }
    }
}
