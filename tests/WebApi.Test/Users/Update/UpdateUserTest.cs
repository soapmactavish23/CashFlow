using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Text.Json;

namespace WebApi.Test.Users.Update
{
    public class UpdateUserTest : CashFlowClassFixture
    {

        private const string METHOD = "api/User";

        private readonly string _token;

        public UpdateUserTest(CustomerWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _token = webApplicationFactory.User_Team_Member.GetToken();
        }

        [Fact]
        public async Task Success()
        {
            var request = RequestUpdateUserJsonBuilder.Builder();

            var response = await DoPut(METHOD, request, token: _token);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Error_Empty_Name()
        {
            var request = RequestUpdateUserJsonBuilder.Builder();
            request.Name = string.Empty;

            var response = await DoPut(METHOD, request, token: _token, culture: "en");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

            var expectedMessage = ResourceErrorMessage.ResourceManager.GetString("NAME_EMPTY");

            errors.Should().Contain(c => c.GetString()!.Equals(expectedMessage));
        }
    }
}
