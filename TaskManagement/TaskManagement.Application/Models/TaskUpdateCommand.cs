using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Models;

/// <summary>
/// Команда обновления задачи
/// </summary>
public sealed record TaskUpdateCommand(
    Guid Id,
    string Title,
    string? Description,
    Status Status);
