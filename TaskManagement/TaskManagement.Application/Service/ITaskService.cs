using TaskManagement.Application.Models;

namespace TaskManagement.Application.Service;

/// <summary>
/// Интерфейс сервиса заданий
/// </summary>
public interface ITaskService
{
    /// <summary>
    /// Создание задания
    /// </summary>
    /// <param name="taskCreateCommand">Команда создания задачи</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Дто задания</returns>
    Task<TaskDto> CreateAsync(TaskCreateCommand taskCreateCommand, CancellationToken ct);

    /// <summary>
    /// Получение всех заданий
    /// </summary>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Дто заданий</returns>
    Task<IEnumerable<TaskDto>> GetAllAsync(CancellationToken ct);

    /// <summary>
    /// Получение задания
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Дто задания</returns>
    Task<TaskDto?> GetAsync(Guid id, CancellationToken ct);

    /// <summary>
    /// Изменение задания
    /// </summary>
    /// <param name="taskUpdateCommand">Команда обновления задачи</param>   
    /// <param name="ct">Токен отмены</param>
    /// <returns>Измененное дто</returns>
    Task<TaskDto?> UpdateAsync(
        TaskUpdateCommand taskUpdateCommand,
        CancellationToken ct);

    /// <summary>
    /// Удаление задания
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Произошло ли удаление задания</returns>
    Task<bool> DeleteAsync(Guid id, CancellationToken ct);
}
