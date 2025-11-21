using CashFlow.Domain.Repositories;
using Moq;

namespace CommonTestUtilities.Repositories
{
    public class UnitOfWorkBuilder
    {

        public static IUnitOfWork Builder()
        {
            var mock = new Mock<IUnitOfWork>();

            return mock.Object;
        }

    }
}
