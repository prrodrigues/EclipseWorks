using System;
using System.Collections.Generic;
using Xunit;
using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Data;
using TaskManagerApi.DTOs;
using TaskManagerApi.Models;
using TaskManagerApi.Services;

namespace TaskManagerApi.Tests.Services;

public class TaskServiceTests
{
    private AppDbContext GetDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public void CreateTask_AddsTaskToProject()
    {
        var context = GetDbContext("CreateTask");
        var project = new Project { Name = "Projeto" };
        context.Projects.Add(project);
        context.SaveChanges();

        var service = new TaskService(context);
        var dto = new CreateTaskDto
        {
            Title = "Tarefa 1",
            Description = "Descricao",
            DueDate = DateTime.UtcNow.AddDays(1),
            Priority = TaskPriority.Alta
        };

        var task = service.CreateTask(project.Id, dto);

        Assert.Single(context.Tasks);
        Assert.Equal("Tarefa 1", task.Title);
    }

    [Fact]
    public void CreateTask_ThrowsException_WhenProjectNotFound()
    {
        var context = GetDbContext("CreateTask_NoProject");
        var service = new TaskService(context);

        var dto = new CreateTaskDto
        {
            Title = "Tarefa",
            Description = "Descricao",
            DueDate = DateTime.UtcNow,
            Priority = TaskPriority.Baixa
        };

        Assert.Throws<Exception>(() => service.CreateTask(999, dto));
    }

    [Fact]
    public void CreateTask_ThrowsException_WhenLimitExceeded()
    {
        var context = GetDbContext("CreateTask_Limit");
        var project = new Project { Name = "Projeto" };
        for (int i = 0; i < 20; i++)
            project.Tasks.Add(new TaskItem(TaskPriority.Baixa));

        context.Projects.Add(project);
        context.SaveChanges();

        var service = new TaskService(context);
        var dto = new CreateTaskDto
        {
            Title = "Nova Tarefa",
            Description = "Teste",
            DueDate = DateTime.UtcNow,
            Priority = TaskPriority.Media
        };

        Assert.Throws<Exception>(() => service.CreateTask(project.Id, dto));
    }

    [Fact]
    public void UpdateTask_UpdatesFieldsAndAddsHistory()
    {
        var context = GetDbContext("UpdateTask");
        var task = new TaskItem(TaskPriority.Alta)
        {
            Title = "T1",
            Description = "D1",
            Status = TaskStatus.Pendente
        };
        context.Tasks.Add(task);
        context.SaveChanges();

        var service = new TaskService(context);
        var dto = new UpdateTaskDto
        {
            Title = "T2",
            Description = "D2",
            Status = TaskStatus.Concluida
        };

        var updated = service.UpdateTask(task.Id, dto, "user1");

        Assert.Equal("T2", updated.Title);
        Assert.Equal("D2", updated.Description);
        Assert.Equal(TaskStatus.Concluida, updated.Status);
        Assert.Single(updated.History);
    }

    [Fact]
    public void DeleteTask_RemovesTask()
    {
        var context = GetDbContext("DeleteTask");
        var task = new TaskItem(TaskPriority.Media);
        context.Tasks.Add(task);
        context.SaveChanges();

        var service = new TaskService(context);
        service.DeleteTask(task.Id);

        Assert.Empty(context.Tasks);
    }
}