using CashFlow.Application.UseCases.Users.Update;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.User;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace UseCases.Test.Users.Update
{
    public class UpdateUserUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var user = UserBuilder.Builder();
            var request = RequestUpdateUserJsonBuilder.Builder();

            var useCase = CreateUseCase(user);

            var act = async () => await useCase.Execute(request);

            await act.Should().NotThrowAsync();

            user.Name.Should().Be(request.Name);
            user.Email.Should().Be(request.Email);

        }

        [Fact]
        public async Task Error_Name_Empty()
        {
            var user = UserBuilder.Builder();

            var request = RequestUpdateUserJsonBuilder.Builder();
            request.Name = string.Empty;

            var useCase = CreateUseCase(user);

            var act = async () => await useCase.Execute(request);

            var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

            result.Where(ex => ex.GetErrors().Contains(ResourceErrorMessage.NAME_EMPTY));
        }

        [Fact]
        public async Task Error_Email_Already_Exist()
        {
            var user = UserBuilder.Builder();
            var request = RequestUpdateUserJsonBuilder.Builder();

            var useCase = CreateUseCase(user, request.Email);

            var act = async () => await useCase.Execute(request);

            var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

            result.Where(ex => ex.GetErrors().Contains(ResourceErrorMessage.EMAIL_ALREADY_REGISTERED));
        }

        private UpdateUserUseCase CreateUseCase(User user, string? email = null)
        {
            var unitOfWork = UnitOfWorkBuilder.Builder();
            var updateRepository = UserUpdateOnlyRepositoryBuider.Builder(user);
            var loggedUser = LoggedUserBuilder.Builder(user);
            var readRepository = new UserReadOnlyRepositoryBuilder();

            if(string.IsNullOrWhiteSpace(email) == false)
            {
                readRepository.ExistActiveUserWithEmail(email);
            }

            return new UpdateUserUseCase(loggedUser, updateRepository, readRepository.Builder(), unitOfWork);
        }
    }
}
