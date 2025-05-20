using Microsoft.EntityFrameworkCore;
using NSubstitute;
using TaskManagement.Application.Models;
using TaskManagement.Application.Service;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using TaskManagement.Infrastructure.Db;
using EasyNetQ;
using EasyNetQ.Topology;
using TaskManagement.Infrastructure.RabbitMq;

namespace TaskManagement.Tests;

public class TaskServiceTests
{
    private readonly TaskDbContext _dbContext;
    private readonly TaskService _service;

    public TaskServiceTests()
    {
        var options = new DbContextOptionsBuilder<TaskDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContext = new TaskDbContext(options);

        var mockBus = Substitute.For<IBus>();
        var mockExchange = new Exchange("logs");

        _service = new TaskService(_dbContext, mockBus, new RabbitMqExchange(mockExchange));
    }

    [Fact]
    public async Task CreateAsync_ShouldAddNewTaskToDatabase()
    {
        var title = "Test Title";
        var desc = "Test Description";
        var command = new TaskCreateCommand(title, desc);

        var result = await _service.CreateAsync(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(title, result.Title);
        Assert.Equal(desc, result.Description);
        Assert.Single(_dbContext.Tasks);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllTasks()
    {
        _dbContext.Tasks.Add(new TaskItem("Title1", "Desc1"));
        _dbContext.Tasks.Add(new TaskItem("Title2", "Desc2"));
        await _dbContext.SaveChangesAsync();

        var result = await _service.GetAllAsync(CancellationToken.None);

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetAsync_ShouldReturnCorrectTask()
    {
        var task = new TaskItem("Title", "Desc");
        _dbContext.Tasks.Add(task);
        await _dbContext.SaveChangesAsync();

        var result = await _service.GetAsync(task.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(task.Id, result!.Id);
    }

    [Fact]
    public async Task UpdateAsync_ShouldModifyExistingTask()
    {
        var task = new TaskItem("Old Title", "Old Desc");
        _dbContext.Tasks.Add(task);
        await _dbContext.SaveChangesAsync();

        var newTitle = "New Title";
        var newDesc = "New Desc";
        var newStatus = Status.InProgress;
        var command = new TaskUpdateCommand(task.Id, newTitle, newDesc, newStatus);

        var result = await _service.UpdateAsync(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(newTitle, result!.Title);
        Assert.Equal(newDesc, result.Description);
        Assert.Equal(newStatus, result.Status);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveTaskFromDatabase()
    {
        var task = new TaskItem("To Delete", "Desc");
        _dbContext.Tasks.Add(task);
        await _dbContext.SaveChangesAsync();

        var result = await _service.DeleteAsync(task.Id, CancellationToken.None);

        Assert.True(result);
        Assert.Empty(_dbContext.Tasks);
    }
}
