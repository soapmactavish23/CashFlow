
using CashFlow.Application.UseCases.Expenses;
using CashFlow.Communication.Enums;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace Validators.Tests.Expenses
{
    public class ExpenseValidatorTests
    {
        [Fact]
        public void Success()
        {
            // Arrange
            var validator = new ExpenseValidator();
            var request = RequestExpenseJsonBuilder.Builder();

            // Act
            var result = validator.Validate(request);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Error_Title_Empty()
        {
            // Arrange
            var validator = new ExpenseValidator();
            var request = RequestExpenseJsonBuilder.Builder();
            request.Title = string.Empty;

            // Act
            var result = validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessage.TITLE_REQUIRED));
        }

        [Fact]
        public void Error_Date_Future()
        {
            // Arrange
            var validator = new ExpenseValidator();
            var request = RequestExpenseJsonBuilder.Builder();
            request.Date = DateTime.Now.AddDays(1);

            // Act
            var result = validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessage.EXPENSES_CANNOT_FOR_THE_FUTURE));
        }

        [Fact]
        public void Error_Payment_Type_Invalid()
        {
            // Arrange
            var validator = new ExpenseValidator();
            var request = RequestExpenseJsonBuilder.Builder();
            request.PaymentType = (PaymentType) 700;

            // Act
            var result = validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessage.PAYMENT_TYPE_INVALID));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Error_Amount_Invalid(decimal amount)
        {
            // Arrange
            var validator = new ExpenseValidator();
            var request = RequestExpenseJsonBuilder.Builder();
            request.Amount = amount;

            // Act
            var result = validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessage.AMOUNT_MUST_BE_GREATER_THAN_ZERO));
        }

        [Fact]
        public void Error_Tag_Invalid()
        {
            // Arrange
            var validator = new ExpenseValidator();
            var request = RequestExpenseJsonBuilder.Builder();
            request.Tags.Add((Tag)1000);

            // Act
            var result = validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessage.TAG_TYPE_NOT_SUPPORTED));
        }
    }
}
