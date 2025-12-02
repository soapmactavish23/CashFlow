using CashFlow.Domain.Entities;
using CashFlow.Domain.Services.LoggedUser;
using Moq;

namespace CommonTestUtilities.LoggedUser
{
    public class LoggedUserBuilder
    {
        public static ILoggerUser Builder(User user)
        {
            var mock = new Mock<ILoggerUser>();

            mock.Setup(loggedUser => loggedUser.Get()).ReturnsAsync(user);

            return mock.Object;
        }
    }
}
