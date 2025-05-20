using EasyNetQ.Topology;
using EasyNetQ;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Events;
using TaskManagement.Application.Models;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Db;
using TaskManagement.Infrastructure.RabbitMq;

namespace TaskManagement.Application.Service;

/// <summary>
/// Сервис заданий
/// </summary>
public sealed class TaskService : ITaskService
{
    /// <summary>
    /// Контекст БД
    /// </summary>
    private readonly TaskDbContext _dbContext;

    /// <summary>
    /// Шина брокера сообщений
    /// </summary>
    private readonly IBus _bus;

    /// <summary>
    /// Контекст БД
    /// </summary>
    private readonly Exchange _exchange;

    public TaskService(
        TaskDbContext dbContext,
        IBus bus,
        RabbitMqExchange exchange)
    {
        _dbContext = dbContext;
        _bus = bus;
        _exchange = exchange.Exchange;
        dbContext.Database.EnsureCreated();
    }

    ///<inheritdoc/>
    public async Task<TaskDto> CreateAsync(TaskCreateCommand taskCreateCommand, CancellationToken ct)
    {
        var entity = new TaskItem(taskCreateCommand.Title, taskCreateCommand.Description);
        _dbContext.Add(entity);

        await _dbContext.SaveChangesAsync(ct);

        PublishLog(new("Create", null, entity));

        return ToDto(entity);
    }

    ///<inheritdoc/>
    public async Task<IEnumerable<TaskDto>> GetAllAsync(CancellationToken ct)
    {
        var tasks = await _dbContext.Tasks
            .AsNoTracking()
            .Select(x => ToDto(x))
            .ToArrayAsync(ct);

        return tasks;
    }

    ///<inheritdoc/>
    public async Task<TaskDto?> GetAsync(Guid id, CancellationToken ct)
    {
        var task = await _dbContext.Tasks
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => ToDto(x))
            .FirstOrDefaultAsync(ct);

        return task;
    }

    ///<inheritdoc/>
    public async Task<TaskDto?> UpdateAsync(
        TaskUpdateCommand taskUpdateCommand,
        CancellationToken ct)
    {
        var task = await _dbContext.Tasks.FirstOrDefaultAsync(x => x.Id == taskUpdateCommand.Id, ct);

        if (task is null) return null;

        var oldTask = task;
        task.Update(taskUpdateCommand.Title, taskUpdateCommand.Description, taskUpdateCommand.Status);

        await _dbContext.SaveChangesAsync(ct);

        PublishLog(new("Update", oldTask, task));

        return ToDto(task);
    }

    ///<inheritdoc/>
    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        var task = await _dbContext.Tasks
            .FirstOrDefaultAsync(x => x.Id == id, ct);
        if (task is null) return false;

        _dbContext.Remove(task);
        await _dbContext.SaveChangesAsync(ct);

        PublishLog(new("Delete", task));

        return true;
    }

    /// <summary>
    /// Опубликовать лог в раббит
    /// </summary>
    /// <param name="logEvent">Ивент для отправки</param>
    private async void PublishLog(LogEvent logEvent)
    {
        await _bus.Advanced.PublishAsync(
            exchange: _exchange,
            routingKey: string.Empty,
            mandatory: true,
            message: new Message<LogEvent>(logEvent));
    }

    /// <summary>
    /// Приведение к Дто
    /// </summary>
    /// <param name="taskItem">Сущность задания</param>
    /// <returns></returns>
    private static TaskDto ToDto(TaskItem taskItem) => 
        new(
            taskItem.Id, 
            taskItem.Title,
            taskItem.Description,
            taskItem.Status,
            taskItem.CreatedAt,
            taskItem.UpdatedAt);
}
