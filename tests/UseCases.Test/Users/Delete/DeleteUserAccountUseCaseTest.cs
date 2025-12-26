using CashFlow.Application.UseCases.Users.Delete;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using FluentAssertions;

namespace UseCases.Test.Users.Delete
{
    public class DeleteUserAccountUseCaseTest
    {

        [Fact]
        public async Task Success()
        {
            var user = UserBuilder.Builder();
            var useCase = CreateUseCase(user);

            var act = async () => await useCase.Execute();

            await act.Should().NotThrowAsync();
        }


        private DeleteUserAccountUseCase CreateUseCase(User user)
        {
            var repository = UserWriteOnlyRepositoryBuilder.Builder();
            var loggedUser = LoggedUserBuilder.Builder(user);
            var unitOfWork = UnitOfWorkBuilder.Builder();

            return new DeleteUserAccountUseCase(unitOfWork, repository, loggedUser);
        }

    }
}
