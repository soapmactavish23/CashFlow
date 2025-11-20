using CashFlow.Domain.Entities;

namespace CashFlow.Infrastructure.Security
{
    public interface IAccessTokenGenerator
    {
        string Generate(User user);
    }
}
