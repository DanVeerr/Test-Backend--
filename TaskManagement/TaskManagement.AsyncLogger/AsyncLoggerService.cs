using System.Text.Json;
using System.Text;
using EasyNetQ.Topology;
using EasyNetQ.Consumer;
using TaskManagement.Application.Events;
using EasyNetQ;
using TaskManagement.Infrastructure.RabbitMq;

namespace TaskManagement.AsyncLogger;

/// <summary>
/// Сервис для логгирование событий из очереди
/// </summary>
public class AsyncLoggerService
{
    /// <summary>
    /// Логгер
    /// </summary>
    private readonly ILogger<AsyncLoggerService> _logger;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="bus">Экземпляр Bus</param>
    /// <param name="queue">Экземпляр Queue</param>
    /// <param name="logger">Логгер</param>
    public AsyncLoggerService(
        IBus bus,
        RabbitMqQueue queue,
        ILogger<AsyncLoggerService> logger
    )
    {
        _logger = logger;

        Func<ReadOnlyMemory<byte>, MessageProperties, MessageReceivedInfo, CancellationToken, Task<AckStrategy>> handler = HandleAsync;
        bus.Advanced.Consume(x => x.ForQueue(queue.Queue, handler, c => { }));
    }

    /// <summary>
    /// Метод срабатывабщий, когда приходит сообщение
    /// </summary>
    /// <param name="bytes">Сообщение</param>
    /// <param name="messageProperties">Свойства сообщения</param>
    /// <param name="messageReceivedInfo">Информация о сообщении</param>
    /// <returns></returns>
    private Task<AckStrategy> HandleAsync(
        ReadOnlyMemory<byte> bytes,
        MessageProperties messageProperties,
        MessageReceivedInfo messageReceivedInfo,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var logDataJson = Encoding.UTF8.GetString(bytes.ToArray());
            var logData = System.Text.Json.JsonSerializer.Deserialize<LogEvent>(logDataJson)!;

            _logger.LogInformation($"Произошло событие {logData.Event} изменилось с {logData.OldValue?.ToString()} до {logData.NewValue?.ToString()}");

            return Task.FromResult(AckStrategies.Ack);
        }
        catch (JsonException e)
        {
            _logger.LogError(e, "Произошла ошибка при обработке сообщения");

            return Task.FromResult(AckStrategies.Ack);
        }
    }
}
