using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace UseCases.Test.Expenses.Register
{
    public class RegisterExpenseUseCaseTest
    {
        [Fact]
        public async Task Success() 
        {
            var loggedUser = UserBuilder.Builder();
            var request = RequestExpenseJsonBuilder.Builder();
            var useCase = CreateUseCase(loggedUser);

            var result = await useCase.Execute(request);

            result.Should().NotBeNull();
            result.Title.Should().Be(request.Title);

        }

        [Fact]
        public async Task Error_Title_Empty() 
        {
            var loggedUser = UserBuilder.Builder();

            var request = RequestExpenseJsonBuilder.Builder();
            request.Title = string.Empty;

            var useCase = CreateUseCase(loggedUser);

            var act = async () => await useCase.Execute(request);

            var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

            result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessage.TITLE_REQUIRED));
        }

        private RegisterExpenseUseCase CreateUseCase(User user)
        {
            var repository = ExpensesWriteOnlyRepositoryBuilder.Builder();
            var mapper = MapperBuilder.Builder();
            var unitOfWork = UnitOfWorkBuilder.Builder();
            var loggedUser = LoggedUserBuilder.Builder(user);

            return new RegisterExpenseUseCase(repository, unitOfWork, mapper, loggedUser);
        }

    }
}
