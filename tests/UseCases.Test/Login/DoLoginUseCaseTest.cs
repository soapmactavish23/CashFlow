using CashFlow.Application.UseCases.Login.DoLogin;
using CashFlow.Communication.Requests;
using CashFlow.Domain.Entities;
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
            var useCase = CreateUseCase(user);

            var result = await useCase.Execute(request);

            result.Should().NotBeNull();
            result.Name.Should().Be(user.Name);
            result.Token.Should().NotBeNullOrWhiteSpace();
        }


        [Fact]
        public async Task Error_User_Not_Found() { }

        [Fact]
        public async Task Error_Password_Not_Match() { }

        private DoLoginUseCase CreateUseCase(User user) {
            var passwordEncripter = PasswordEncripterBuilder.Builder();
            var tokenGenerator = JwtTokenGeneratorBuilder.Builder();
            var readRepository = new UserReadOnlyRepositoryBuilder().GetUserByEmail(user).Builder();

            return new DoLoginUseCase(readRepository, passwordEncripter, tokenGenerator);
        }

    }
}
