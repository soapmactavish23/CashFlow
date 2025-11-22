using CashFlow.Domain.Entities;
using CashFlow.Infrastructure.Security;
using Moq;

namespace CommonTestUtilities.Token
{
    public class JwtTokenGeneratorBuilder
    {

        public static IAccessTokenGenerator Builder()
        {
            var mock = new Mock<IAccessTokenGenerator>();

            mock.Setup(accessTokenGenerator => accessTokenGenerator.Generate(It.IsAny<User>()))
                .Returns("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkFuYSBDbGF1ZGlhIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvc2lkIjoiZDE2NGE3NzQtNWRmNC00NTFkLWI1OTMtZDUzZmM1YjllODZhIiwibmJmIjoxNzYzODM0MjYwLCJleHAiOjE3NjM4OTQyNjAsImlhdCI6MTc2MzgzNDI2MH0.lA0CKQbIV2j_ye8-DEkBbRxYBE5zADYu1yLz9NXngsU");

            return mock.Object;
        }

    }
}
