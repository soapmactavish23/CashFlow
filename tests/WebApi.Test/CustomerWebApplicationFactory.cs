using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Infrastructure.DataAccess;
using CashFlow.Infrastructure.Security;
using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Test
{
    public class CustomerWebApplicationFactory : WebApplicationFactory<Program>
    {

        private User _user;
        private string _password;
        private string _token;

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

                    var tokenGenerator = scope.ServiceProvider.GetRequiredService<IAccessTokenGenerator>();

                    StartDatabase(db, passwordEncripter);

                    _token = tokenGenerator.Generate(_user);

                });
        }

        public string GetName() => _user.Name;
        public string GetEmail() => _user.Email;
        public string GetPassword() => _password;

        public string GetToken() => _token;

        private void StartDatabase(CashFlowDbContext dbContext, IPasswordEncripter passwordEncripter)
        {
            AddUsers(dbContext, passwordEncripter);
            AddExpenses(dbContext, _user);
            dbContext.SaveChanges();
        }

        private void AddUsers(CashFlowDbContext dbContext, IPasswordEncripter passwordEncripter)
        {
            _user = UserBuilder.Builder();
            _password = _user.Password;
            _user.Password = passwordEncripter.Encrypt(_user.Password);

            dbContext.Users.Add(_user);

        }

        private void AddExpenses(CashFlowDbContext dbContext, User user)
        {
            var expense = ExpenseBuilder.Build(user);

            dbContext.Expenses.Add(expense);
        }
    }

}

