using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Expenses.Register
{
    public class RegisterExpenseTest : CashFlowClassFixture
    {

        private const string METHOD = "api/Expenses";

        private readonly HttpClient _httpClient;
        private readonly string _token;

        public RegisterExpenseTest(CustomerWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _httpClient = webApplicationFactory.CreateClient();
            _token = webApplicationFactory.GetToken();
        }

        [Fact]
        public async Task Success()
        {
            var request = RequestRegisterExpenseJsonBuilder.Builder();

            var result = await DoPost(METHOD, request, token: _token);

            result.StatusCode.Should().Be(HttpStatusCode.Created);

            var body = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(body);

            response.RootElement.GetProperty("title").GetString().Should().Be(request.Title);

        }

        [Fact]
        public async Task Error_Title_Empty()
        {
            var request = RequestRegisterExpenseJsonBuilder.Builder();
            request.Title = string.Empty;

            var result = await DoPost(METHOD, request, token: _token);

            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var body = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(body);

            var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

            var expectedMessage = ResourceErrorMessage.ResourceManager.GetString("TITLE_REQUIRED", new System.Globalization.CultureInfo("en"));

            errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }

    }
}
