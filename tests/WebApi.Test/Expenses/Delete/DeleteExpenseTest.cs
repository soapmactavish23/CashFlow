using CashFlow.Exception;
using FluentAssertions;
using System.Text.Json;

namespace WebApi.Test.Expenses.Delete
{
    public class DeleteExpenseTest : CashFlowClassFixture
    {

        private const string METHOD = "api/Expenses";

        private readonly string _token;
        private readonly long _expenseId;

        public DeleteExpenseTest(CustomerWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _token = webApplicationFactory.User_Team_Member.GetToken();
            _expenseId = webApplicationFactory.Expense_MemberTeam.GetExpenseId();
        }

        [Fact]
        public async Task Success()
        {
            var result = await DoDelete(requestUri: $"{METHOD}/{_expenseId}", token: _token);

            result.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            result = await DoGet(requestUri: $"{METHOD}/{_expenseId}", token: _token);

            result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Error_Expense_Not_Found()
        {
            var result = await DoDelete(requestUri: $"{METHOD}/1000", token: _token);

            result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);

            var body = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(body);

            var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

            var expectedMessage = ResourceErrorMessage.ResourceManager.GetString("EXPENSE_NOT_FOUND", new System.Globalization.CultureInfo("en"));

            errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }
    }
}
