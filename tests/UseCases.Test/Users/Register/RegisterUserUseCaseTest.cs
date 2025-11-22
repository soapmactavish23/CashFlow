using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
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

        [Fact]
        public async Task Error_Name_Empty()
        {
            var request = RequestRegisterUserJsonBuilder.Builder();
            request.Name = string.Empty;

            var useCase = CreateUseCase(request.Email);

            var act = async () => await useCase.Execute(request);

            var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

            result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessage.NAME_EMPTY));
        }

        [Fact]
        public async Task Error_Email_Already_Exist()
        {
            var request = RequestRegisterUserJsonBuilder.Builder();

            var useCase = CreateUseCase(request.Email);

            var act = async () => await useCase.Execute(request);

            var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

            result.Where(ex => ex.GetErrors().Count() == 1 && ex.GetErrors().Contains(ResourceErrorMessage.EMAIL_ALREADY_REGISTERED);
        }

        private RegisterUserUseCase CreateUseCase(string? email = null)
        {
            var mapper = MapperBuilder.Builder();
            var unitOfWork = UnitOfWorkBuilder.Builder();
            var writeRepository = UserWriteOnlyRepositoryBuilder.Builder();
            var passwordEncripter = PasswordEncripterBuilder.Builder();
            var tokenGenerator = JwtTokenGeneratorBuilder.Builder();
            var readRepository = new UserReadOnlyRepositoryBuilder();

            if(string.IsNullOrWhiteSpace(email) == false)
            {
                readRepository.ExistActiveUserWithEmail(email);
            }

            return new RegisterUserUseCase(
                mapper, 
                passwordEncripter, 
                readRepository.Builder(), 
                writeRepository, 
                tokenGenerator, 
                unitOfWork);
        }
    }
}
