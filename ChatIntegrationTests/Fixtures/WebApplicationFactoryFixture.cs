using DataContext;
using Domain;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace ChatIntegrationTests.Fixtures;

public class WebApplicationFactoryFixture : IAsyncLifetime
{
    private readonly WebApplicationFactory<Program> _factory;
    private string _connectionString = "Server=DESKTOP-UJ9A1E3;Database=ChatTestIntegration;User Id=sa;Password=marta16bilas;Integrated Security=true;TrustServerCertificate=True;";
 
    private readonly int InitialUserCount = 2;
    private readonly int InitialChatCount = 2;

    public WebApplicationFactoryFixture()
    {
        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<ChatDataContext>));
                services.AddDbContext<ChatDataContext>(options =>
                {
                    var serviceProvider = services.BuildServiceProvider();
                    string connectionString = serviceProvider.GetRequiredService<IConfiguration>().GetConnectionString(_connectionString);
                    options.UseSqlServer(connectionString);
                });
            });
        });
    }

    async Task IAsyncLifetime.InitializeAsync()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var scopService = scope.ServiceProvider;
            var dbContext = scopService.GetRequiredService<ChatDataContext>();

            await dbContext.Database.EnsureCreatedAsync();

            await dbContext.Users.AddRangeAsync(UserFixture.GetUsers(InitialUserCount));
            await dbContext.SaveChangesAsync();

            await dbContext.Chats.AddRangeAsync(ChatFixture.GetChats(InitialChatCount));
            await dbContext.SaveChangesAsync();

            foreach (var chat in dbContext.Chats)
            {
                foreach (var user in chat.Users)
                {
                    await dbContext.UsersChats.AddAsync(new UserChat { ChatId = chat.Id, UserId = user.Id });
                }
            }
        }
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var scopService = scope.ServiceProvider;
            var dbContext = scopService.GetRequiredService<ChatDataContext>();
            await dbContext.Database.EnsureDeletedAsync();
        }
    }
}
