using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Events;

public record LogEvent(string Event, TaskItem? OldValue, TaskItem? NewValue = null);
