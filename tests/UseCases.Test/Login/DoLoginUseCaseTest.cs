using CashFlow.Application.UseCases.Login.DoLogin;
using CashFlow.Communication.Requests;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Token;
using FluentAssertions;

namespace UseCases.Test.Login
{
    public class DoLoginUseCaseTest
    {

        [Fact]
        public async Task Success() 
        {
            var user = UserBuilder.Builder();

            var request = RequestLoginJsonBuilder.Builder();
            request.Email = user.Email;

            var useCase = CreateUseCase(user, request.Password);

            var result = await useCase.Execute(request);

            result.Should().NotBeNull();
            result.Name.Should().Be(user.Name);
            result.Token.Should().NotBeNullOrWhiteSpace();
        }


        [Fact]
        public async Task Error_User_Not_Found() 
        {
            var user = UserBuilder.Builder();
            var request = RequestLoginJsonBuilder.Builder();

            var useCase = CreateUseCase(user, request.Password);

            var act = async () => await useCase.Execute(request);

            var result = await act.Should().ThrowAsync<InvalidLoginException>();

            result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessage.EMAIL_OR_PASSWORD_INVALID));

        }

        [Fact]
        public async Task Error_Password_Not_Match() 
        {
            var user = UserBuilder.Builder();

            var request = RequestLoginJsonBuilder.Builder();
            request.Email = user.Email;

            var useCase = CreateUseCase(user);

            var act = async () => await useCase.Execute(request);

            var result = await act.Should().ThrowAsync<InvalidLoginException>();

            result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessage.EMAIL_OR_PASSWORD_INVALID));
        }

        private DoLoginUseCase CreateUseCase(User user, string? password = null)
        {
            var passwordEncripter = new PasswordEncripterBuilder().Verify(password).Builder();
            var tokenGenerator = JwtTokenGeneratorBuilder.Builder();
            var readRepository = new UserReadOnlyRepositoryBuilder().GetUserByEmail(user).Builder();

            return new DoLoginUseCase(readRepository, passwordEncripter, tokenGenerator);
        }

    }
}
