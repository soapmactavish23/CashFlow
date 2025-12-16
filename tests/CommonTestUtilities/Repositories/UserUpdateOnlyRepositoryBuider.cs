using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.User;
using Moq;

namespace CommonTestUtilities.Repositories
{
    public class UserUpdateOnlyRepositoryBuider
    {
        public static IUserUpdateOnlyRepository Builder(User user)
        {
            var mock = new Mock<IUserUpdateOnlyRepository>();

            mock.Setup(repository => repository.GetById(user.Id)).ReturnsAsync(user);

            return mock.Object;
        }
    }
}
