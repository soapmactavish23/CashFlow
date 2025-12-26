using FluentAssertions;

namespace WebApi.Test.Delete
{
    public class DeleteUserTest : CashFlowClassFixture
    {

        private const string METHOD = "api/User";
        
        private readonly string _token;
        public DeleteUserTest(CustomerWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
        {
            _token = webApplicationFactory.User_Team_Member.GetToken();
        }

        [Fact]
        public async Task Success()
        {
            var result = await DoDelete(METHOD, _token);

            result.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }
    }
}
