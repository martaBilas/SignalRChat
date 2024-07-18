using ChatTest.Hubs;
using DataContext;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;


public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSignalR();

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ChatDataContext>(options =>
            options.UseSqlServer(
                connectionString,
                b => b.MigrationsAssembly("DataContext")));

        builder.Services.AddScoped<IChatService, ChatService>();
        builder.Services.AddScoped<IUserService, UserService>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();


        app.MapControllers();
        app.MapHub<ChatHub>("/chatHub");

        app.Run();
    }
}
