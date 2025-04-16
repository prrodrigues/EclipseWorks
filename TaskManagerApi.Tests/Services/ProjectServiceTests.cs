using System;
using System.Collections.Generic;
using Moq;
using Xunit;
using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Data;
using TaskManagerApi.DTOs;
using TaskManagerApi.Models;
using TaskManagerApi.Services;

namespace TaskManagerApi.Tests.Services;

public class ProjectServiceTests
{
    private AppDbContext GetDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public void Create_AddsProjectToDatabase()
    {
        var context = GetDbContext("Create_AddsProject");
        var service = new ProjectService(context);

        var dto = new CreateProjectDto { Name = "Projeto Teste" };
        var result = service.Create(dto);

        Assert.Single(context.Projects);
        Assert.Equal("Projeto Teste", result.Name);
    }

    [Fact]
    public void GetAll_ReturnsAllProjects()
    {
        var context = GetDbContext("GetAll_Projects");
        context.Projects.Add(new Project { Name = "P1" });
        context.Projects.Add(new Project { Name = "P2" });
        context.SaveChanges();

        var service = new ProjectService(context);
        var result = service.GetAll();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void Delete_RemovesProject_WhenNoPendingTasks()
    {
        var context = GetDbContext("Delete_Project");
        var project = new Project { Name = "Projeto", Tasks = new List<TaskItem>() };
        context.Projects.Add(project);
        context.SaveChanges();

        var service = new ProjectService(context);
        service.Delete(project.Id);

        Assert.Empty(context.Projects);
    }

    [Fact]
    public void Delete_ThrowsException_WhenProjectHasPendingTasks()
    {
        var context = GetDbContext("Delete_With_Pending_Tasks");
        var project = new Project
        {
            Name = "Projeto",
            Tasks = new List<TaskItem>
            {
                new TaskItem(TaskPriority.Media) { Status = TaskStatus.Pendente }
            }
        };
        context.Projects.Add(project);
        context.SaveChanges();

        var service = new ProjectService(context);

        var ex = Assert.Throws<Exception>(() => service.Delete(project.Id));
        Assert.Equal("Conclua ou remova as tarefas pendentes antes de remover o projeto", ex.Message);
    }
}