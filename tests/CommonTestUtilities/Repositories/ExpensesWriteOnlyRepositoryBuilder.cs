using CashFlow.Domain.Repositories.Expenses;
using Moq;

namespace CommonTestUtilities.Repositories
{
    public class ExpensesWriteOnlyRepositoryBuilder
    {

        public static IExpensesWriteOnlyRepository Builder()
        {
            var mock = new Mock<IExpensesWriteOnlyRepository>();

            return mock.Object;
        }

    }
}
