using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Communication.Requests;
using CommonTestUtilities.Requests;

namespace Validators.Tests.Expenses
{
    public class RegisterExpenseValidatorTests
    {
        [Fact]
        public void Success()
        {
            // Arrange
            var validator = new RegisterExpenseValidator();
            var request = RequestRegisterExpenseJsonBuilder.Builder();

            // Act
            var result = validator.Validate(request);

            // Assert
            Assert.True(result.IsValid);
        }
    }
}
