using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Infrastructure.DataAccess;
using CashFlow.Infrastructure.Security;
using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Test.Resources;

namespace WebApi.Test
{
    public class CustomerWebApplicationFactory : WebApplicationFactory<Program>
    {

        public ExpenseIdentityManager Expense_Admin { get; private set; } = default!;
        public ExpenseIdentityManager Expense_MemberTeam { get; private set; } = default!;
        public UserIdentityManager User_Team_Member { get; private set; } = default!;
        public UserIdentityManager User_Admin { get; private set; } = default!;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .UseEnvironment("Test")
                .ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<CashFlowDbContext>));

                    if (descriptor != null)
                        services.Remove(descriptor);

                    var provider = new ServiceCollection()
                        .AddEntityFrameworkInMemoryDatabase()
                        .BuildServiceProvider();

                    services.AddDbContext<CashFlowDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting");
                        options.UseInternalServiceProvider(provider);
                    });

                    var sp = services.BuildServiceProvider();
                    using var scope = sp.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<CashFlowDbContext>();
                    db.Database.EnsureCreated();

                    var passwordEncripter = scope.ServiceProvider.GetRequiredService<IPasswordEncripter>();
                    var accessTokenGenerator = scope.ServiceProvider.GetRequiredService<IAccessTokenGenerator>();

                    StartDatabase(db, passwordEncripter, accessTokenGenerator);

                });
        }


        private void StartDatabase(CashFlowDbContext dbContext, IPasswordEncripter passwordEncripter, IAccessTokenGenerator accessTokenGenerator)
        {
            var userTeamMember = AddUsersTeamMember(dbContext, passwordEncripter, accessTokenGenerator);
            var expenseTeamMember = AddExpenses(dbContext, userTeamMember, expenseId: 1);
            Expense_MemberTeam = new ExpenseIdentityManager(expenseTeamMember);

            var userAdmin = AddUsersAdmin(dbContext, passwordEncripter, accessTokenGenerator);
            var expenseAdmin = AddExpenses(dbContext, userAdmin, expenseId: 2);
            Expense_Admin = new ExpenseIdentityManager(expenseAdmin);

            dbContext.SaveChanges();
        }

        private User AddUsersTeamMember(
            CashFlowDbContext dbContext, 
            IPasswordEncripter passwordEncripter, 
            IAccessTokenGenerator accessTokenGenerator)
        {
            var user = UserBuilder.Builder();
            user.Id = 1;
            var password = user.Password;
            
            user.Password = passwordEncripter.Encrypt(user.Password);

            dbContext.Users.Add(user);

            var token = accessTokenGenerator.Generate(user);

            User_Team_Member = new UserIdentityManager(user, password, token);
            
            return user;
        }

        private User AddUsersAdmin(
            CashFlowDbContext dbContext,
            IPasswordEncripter passwordEncripter,
            IAccessTokenGenerator accessTokenGenerator)
        {
            var user = UserBuilder.Builder(role: Roles.ADMIN);
            user.Id = 2;
            var password = user.Password;

            user.Password = passwordEncripter.Encrypt(user.Password);

            dbContext.Users.Add(user);

            var token = accessTokenGenerator.Generate(user);

            User_Admin = new UserIdentityManager(user, password, token);

            return user;
        }

        private Expense AddExpenses(CashFlowDbContext dbContext, User user, long expenseId)
        {
            var expense = ExpenseBuilder.Build(user);
            expense.Id = expenseId;

            dbContext.Expenses.Add(expense);

            return expense;
        }
    }

}

