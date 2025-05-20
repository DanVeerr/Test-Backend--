using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Models;

/// <summary>
/// Дто задания
/// </summary>
public sealed record TaskDto(
    Guid Id,
    string Title,
    string? Description,
    Status Status,
    DateTime CreatedAt,
    DateTime UpdatedAt);
