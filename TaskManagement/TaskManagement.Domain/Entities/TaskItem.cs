using TaskManagement.Domain.Enums;

namespace TaskManagement.Domain.Entities;

/// <summary>
/// Модель задания
/// </summary>
public sealed class TaskItem
{
    private TaskItem() { }

    /// <summary>
    /// Конструктор 
    /// </summary>
    /// <param name="title">Название</param>
    /// <param name="description">Описание</param>
    public TaskItem(string title, string description)
    {
        Title = title;
        Description = description;
    }

    /// <summary>
    /// Изменение задания
    /// </summary>
    /// <param name="title">Название</param>
    /// <param name="description">Описание</param>
    /// <param name="status">Статус</param>
    public void Update(string? title, string? description, Status? status)
    {
        if (!string.IsNullOrWhiteSpace(title))
            Title = title;
        if (description is not null)
            Description = description;
        if (status.HasValue)
            Status = status.Value;
        UpdatedAt = DateTime.UtcNow;
    }

    public override string? ToString()
    {
        return $"Id: {Id}, Title: {Title}, Description: {Description}, Status: {Status}";
    }

    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; private set; } = Guid.NewGuid();

    /// <summary>
    /// Название
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    /// Описание
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// Статус задачи
    /// </summary>
    public Status Status { get; private set; } = Status.New;

    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    /// <summary>
    /// Дата обновления
    /// </summary>
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
}
