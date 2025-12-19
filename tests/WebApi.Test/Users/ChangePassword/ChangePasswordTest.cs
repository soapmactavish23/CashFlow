using CashFlow.Communication.Requests;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Globalization;
using System.Text.Json;

namespace WebApi.Test.Users.ChangePassword
{
    public class ChangePasswordTest : CashFlowClassFixture
    {

        private const string METHOD = "api/User/change-password";

        private readonly string _token;
        private readonly string _password;
        private readonly string _email;

        public ChangePasswordTest(CustomerWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _token = webApplicationFactory.User_Team_Member.GetToken();
            _password = webApplicationFactory.User_Team_Member.GetPassword();
            _email = webApplicationFactory.User_Team_Member.GetEmail();
        }

        [Fact]
        public async Task Success()
        {
            var request = RequestChangePasswordJsonBuilder.Builder();
            request.Password = _password;

            var response = await DoPut(METHOD, request, _token);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            var loginRequest = new RequestLoginJson
            {
                Email = _email,
                Password = _password
            };

            response = await DoPost("api/Login", loginRequest);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Error_Password_Different_Current_Password()
        {
            var request = RequestChangePasswordJsonBuilder.Builder();

            var response = await DoPut(METHOD, request, _token, culture: "en");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

            var expectedMessage = ResourceErrorMessage.ResourceManager.GetString("PASSWORD_DIFERENT_CURRENT_PASSWORD", new CultureInfo("en"));

            errors.Should().Contain(c => c.GetString()!.Equals(expectedMessage));
        }
    }
}
