using CashFlow.Application.UseCases.Users.Register;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Token;
using FluentAssertions;

namespace UseCases.Test.Users.Register
{
    public class RegisterUserUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var request = RequestRegisterUserJsonBuilder.Builder();
            var useCase = CreateUseCase();

            var result = await useCase.Execute(request);

            result.Should().NotBeNull();
            result.Name.Should().Be(request.Name);
            result.Token.Should().NotBeNullOrWhiteSpace();
        }

        private RegisterUserUseCase CreateUseCase()
        {
            var mapper = MapperBuilder.Builder();
            var unitOfWork = UnitOfWorkBuilder.Builder();
            var writeRepository = UserWriteOnlyRepositoryBuilder.Builder();
            var passwordEncripter = PasswordEncripterBuilder.Builder();
            var tokenGenerator = JwtTokenGeneratorBuilder.Builder();

            return new RegisterUserUseCase(mapper, passwordEncripter, null, writeRepository, tokenGenerator, unitOfWork);
        }
    }
}
