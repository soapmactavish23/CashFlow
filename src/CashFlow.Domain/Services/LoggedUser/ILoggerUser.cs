using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Services.LoggedUser
{
    public interface ILoggerUser
    {
        Task<User> Get();
    }
}
