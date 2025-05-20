namespace TaskManagement.Application.Models;

/// <summary>
/// Команда создания задачи
/// </summary>
public sealed record TaskCreateCommand(string Title, string Description);
