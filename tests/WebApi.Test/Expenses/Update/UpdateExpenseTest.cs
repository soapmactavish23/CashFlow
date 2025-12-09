using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Text.Json;

namespace WebApi.Test.Expenses.Update
{
    public class UpdateExpenseTest : CashFlowClassFixture
    {

        private const string METHOD = "api/Expenses";

        private readonly string _token;
        private readonly long _expenseId;

        public UpdateExpenseTest(CustomerWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _token = webApplicationFactory.User_Team_Member.GetToken();
            _expenseId = webApplicationFactory.Expense.GetExpenseId();
        }

        [Fact]
        public async Task Success()
        {
            var request = RequestExpenseJsonBuilder.Builder();

            var result = await DoPut(requestUri: $"{METHOD}/{_expenseId}", request: request, token: _token);

            result.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Error_Title_Empty()
        {
            var request = RequestExpenseJsonBuilder.Builder();
            request.Title = string.Empty;

            var result = await DoPut(requestUri: $"{METHOD}/{_expenseId}", request: request, token: _token);

            result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

            var body = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(body);

            var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

            var expectedMessage = ResourceErrorMessage.ResourceManager.GetString("TITLE_REQUIRED", new System.Globalization.CultureInfo("en"));

            errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }

        [Fact]
        public async Task Error_Expense_Not_Found()
        {
            var request = RequestExpenseJsonBuilder.Builder();

            var result = await DoPut(requestUri: $"{METHOD}/1000", request: request, token: _token);

            result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);

            var body = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(body);

            var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

            var expectedMessage = ResourceErrorMessage.ResourceManager.GetString("EXPENSE_NOT_FOUND", new System.Globalization.CultureInfo("en"));

            errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }
    }
}
