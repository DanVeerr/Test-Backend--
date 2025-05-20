using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Service;
using TaskManagement.Infrastructure.Db;
using TaskManagement.Infrastructure.RabbitMq;

namespace TaskManagement.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var connectionPostgreSQL = builder.Configuration.GetConnectionString("PostgreSQL");
        var connectionRabbitMq = builder.Configuration.GetConnectionString("RabbitMq");

        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen();

        builder.Services.AddRabbitMq(connectionRabbitMq);
        builder.Services.AddDbContext<TaskDbContext>(
            optionsAction: opts => opts.UseNpgsql(connectionPostgreSQL),
            contextLifetime: ServiceLifetime.Transient);

        builder.Services.AddTransient<ITaskService, TaskService>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
