using EasyNetQ;
using EasyNetQ.Topology;
using Microsoft.Extensions.DependencyInjection;

namespace TaskManagement.Infrastructure.RabbitMq;

/// <summary>
/// Регистрация брокера сообщений
/// </summary>
public static class RabbitMqExtension
{
    /// <summary>
    /// Название очереди
    /// </summary>
    private const string queueName = "task-log";
    /// <summary>
    /// Название exchange
    /// </summary>
    private const string exchangeName = "task-log";

    /// <summary>
    /// Создание подключение к rabbitmq и инициализация queue и exchange
    /// </summary>
    /// <param name="connectionString">Настройки rabbitmq</param>
    /// <returns></returns>
    public static void AddRabbitMq(
        this IServiceCollection services,
        string connectionString)
    {
        Task.Delay(1000).Wait();
        var bus = RabbitHutch.CreateBus(connectionString);

        var exchange = bus.Advanced.ExchangeDeclare(
            name: exchangeName,
            configure: conf =>
            {
                conf.AsDurable(true);
                conf.WithType(ExchangeType.Fanout);
                conf.AsAutoDelete(false);
            });

        var queue = bus.Advanced.QueueDeclare(
            name: queueName,
            configure: conf =>
            {
                conf.AsDurable(true);
                conf.AsExclusive(false);
            });

        bus.Advanced.Bind(
            exchange: exchange,
            queue: queue,
            routingKey: string.Empty
        );
        services.AddSingleton(bus);
        services.AddSingleton(new RabbitMqExchange(exchange));
        services.AddSingleton(new RabbitMqQueue(queue));
    }
}

public class RabbitMqExchange
{
    public RabbitMqExchange(Exchange exchange)
    {
        Exchange = exchange;
    }

    public Exchange Exchange { get; }
}

public class RabbitMqQueue
{
    public RabbitMqQueue(Queue queue)
    {
        Queue = queue;
    }

    public Queue Queue { get; }
}
