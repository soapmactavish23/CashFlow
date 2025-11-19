using CashFlow.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Domain.Repositories.User
{
    public class UserRepository : IUserReadOnlyRepository
    {

        private readonly CashFlowDbContext _dbContext;

        public UserRepository(CashFlowDbContext dbContext) => _dbContext = dbContext;

        public async Task<bool> ExistActiveUserWithEmail(string email)
        {
            return await _dbContext.Users.AnyAsync(user => user.Email.Equals(email));
        }
    }
}
