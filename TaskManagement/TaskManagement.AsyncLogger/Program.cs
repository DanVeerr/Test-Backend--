using TaskManagement.Infrastructure.RabbitMq;

namespace TaskManagement.AsyncLogger;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        var connectionRabbitMq = builder.Configuration.GetConnectionString("RabbitMq");

        builder.Services.AddSingleton<AsyncLoggerService>();
        builder.Services.AddRabbitMq(connectionRabbitMq);
        var host = builder.Build();

        host.Services.GetRequiredService<AsyncLoggerService>();

        host.Run();
    }
}