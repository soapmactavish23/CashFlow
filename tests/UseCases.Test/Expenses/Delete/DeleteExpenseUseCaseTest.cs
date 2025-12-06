using CashFlow.Application.UseCases.Expenses.Delete;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using FluentAssertions;

namespace UseCases.Test.Expenses.Delete
{
    public class DeleteExpenseUseCaseTest
    {

        [Fact]
        public async Task Success() 
        {
            var loggedUser = UserBuilder.Builder();
            var expense = ExpenseBuilder.Build(loggedUser);

            var useCase = CreateUseCase(loggedUser, expense);

            var act = async () => await useCase.Execute(expense.Id);

            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task Error_Expense_Not_Found()
        {
            var loggedUser = UserBuilder.Builder();

            var useCase = CreateUseCase(loggedUser);

            var act = async () => await useCase.Execute(id: 1000);

            var result = await act.Should().ThrowAsync<NotFoundException>();

            result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessage.EXPENSE_NOT_FOUND));
        }

        private DeleteExpenseUseCase CreateUseCase(User user, Expense? expense = null)
        {
            var repositoryWriteOnly = ExpensesWriteOnlyRepositoryBuilder.Builder();
            var repository = new ExpenseReadOnlyRepositoryBuilder().GetById(user, expense).Builder();
            var unitOfWork = UnitOfWorkBuilder.Builder();
            var loggedUser = LoggedUserBuilder.Builder(user);

            return new DeleteExpenseUseCase(repositoryWriteOnly, unitOfWork, loggedUser, repository);
        }

    }
}
