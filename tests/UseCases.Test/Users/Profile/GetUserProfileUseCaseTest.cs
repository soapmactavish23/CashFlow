using CashFlow.Application.UseCases.Users.Profile;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using FluentAssertions;

namespace UseCases.Test.Users.Profile
{
    public class GetUserProfileUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var user = UserBuilder.Builder();
            var useCase = CreateUseCase(user);

            var result = await useCase.Execute();

            result.Should().NotBeNull();
            result.Name.Should().Be(user.Name);
            result.Email.Should().Be(user.Email);
        }

        private GetUserProfileUseCase CreateUseCase(User user)
        {
            var mapper = MapperBuilder.Builder();
            var loggedUser = LoggedUserBuilder.Builder(user);

            return new GetUserProfileUseCase(loggedUser, mapper);
        }

    }
}
