using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace WebApi.Test.Expenses.Register
{
    public class RegisterExpenseTest : IClassFixture<CustomerWebApplicationFactory>
    {

        private const string METHOD = "api/Expenses";

        private readonly HttpClient _httpClient;
        private readonly string _token;

        public RegisterExpenseTest(CustomerWebApplicationFactory webApplicationFactory)
        {
            _httpClient = webApplicationFactory.CreateClient();
            _token = webApplicationFactory.GetToken();
        }

        [Fact]
        public async Task Success()
        {
            var request = RequestRegisterExpenseJsonBuilder.Builder();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var result = await _httpClient.PostAsJsonAsync(METHOD, request);

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

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var result = await _httpClient.PostAsJsonAsync(METHOD, request);

            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var body = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(body);

            var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

            var expectedMessage = ResourceErrorMessage.ResourceManager.GetString("TITLE_REQUIRED", new System.Globalization.CultureInfo("en"));

            errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }

    }
}
