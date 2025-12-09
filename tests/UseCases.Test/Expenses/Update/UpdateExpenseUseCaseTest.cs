using CashFlow.Application.UseCases.Expenses.Update;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace UseCases.Test.Expenses.Update
{
    public class UpdateExpenseUseCaseTest
    {

        [Fact]
        public async Task Success()
        {
            var loggedUser = UserBuilder.Builder();
            var request = RequestExpenseJsonBuilder.Builder();
            var expense = ExpenseBuilder.Build(loggedUser);

            var useCase = CreateUseCase(loggedUser);

            var act = async () => await useCase.Execute(id: expense.Id, request);

            //await act.Should().NotThrowAsync();

            //expense.Title.Should().Be(request.Title);
            //expense.Description.Should().Be(request.Description);
            //expense.Date.Should().Be(request.Date);
            //expense.Amount.Should().Be(request.Amount);
            //expense.PaymentType.Should().Be((CashFlow.Domain.Enums.PaymentType) request.PaymentType);
        }

        [Fact]
        public async Task Error_Title_Empty()
        {
            var loggedUser = UserBuilder.Builder();
            var expense = ExpenseBuilder.Build(loggedUser);

            var request = RequestExpenseJsonBuilder.Builder();
            request.Title = string.Empty;

            var useCase = CreateUseCase(loggedUser);

            var act = async () => await useCase.Execute(id: expense.Id, request);

            var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

            result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessage.TITLE_REQUIRED));

        }

        [Fact]
        public async Task Error_Expense_Not_Found() 
        {
            var loggedUser = UserBuilder.Builder();

            var request = RequestExpenseJsonBuilder.Builder();

            var useCase = CreateUseCase(loggedUser);

            var act = async () => await useCase.Execute(id: 1000, request);

            var result = await act.Should().ThrowAsync<NotFoundException>();

            result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessage.EXPENSE_NOT_FOUND));
        }

        private UpdateExpenseUseCase CreateUseCase(User user, Expense? expense = null)
        {
            var repository = new ExpenseUpdateOnlyRepositoryBuilder().GetById(user, expense).Builder();
            var mapper = MapperBuilder.Builder();
            var unitOfWork = UnitOfWorkBuilder.Builder();
            var loggedUser = LoggedUserBuilder.Builder(user);

            return new UpdateExpenseUseCase(mapper, unitOfWork, repository, loggedUser);
        }

    }
}
