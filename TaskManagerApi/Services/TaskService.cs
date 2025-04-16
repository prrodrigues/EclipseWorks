using TaskManagerApi.Data;
using TaskManagerApi.DTOs;
using TaskManagerApi.Interfaces;
using TaskManagerApi.Models;
using Microsoft.EntityFrameworkCore;
using TaskStatus = TaskManagerApi.Models.TaskStatus;

namespace TaskManagerApi.Services;

public class TaskService : ITaskService
{
    private readonly AppDbContext _context;
    public TaskService(AppDbContext context) => _context = context;

    public List<TaskItem> GetTasksByProject(int projectId) =>
        _context.Tasks.Where(t => t.ProjectId == projectId).ToList();

    public TaskItem CreateTask(int projectId, CreateTaskDto dto)
    {
        var project = _context.Projects.Include(p => p.Tasks).FirstOrDefault(p => p.Id == projectId);
        if (project == null) throw new Exception("Projeto não encontrado");
        if (project.Tasks.Count >= 20) throw new Exception("Limite de tarefas atingido");

        var task = new TaskItem(dto.Priority)
        {
            Title = dto.Title,
            Description = dto.Description,
            DueDate = dto.DueDate,
            ProjectId = projectId
        };
        _context.Tasks.Add(task);
        _context.SaveChanges();
        return task;
    }

    public TaskItem UpdateTask(int taskId, UpdateTaskDto dto, string modifiedBy)
    {
        var task = _context.Tasks.Include(t => t.History).FirstOrDefault(t => t.Id == taskId);
        if (task == null) throw new Exception("Tarefa não encontrada");

        if (dto.Title != null) task.Title = dto.Title;
        if (dto.Description != null) task.Description = dto.Description;
        if (dto.Status != null) task.Status = dto.Status.Value;

        task.History.Add(new TaskHistory
        {
            ModifiedAt = DateTime.UtcNow,
            ModifiedBy = modifiedBy,
            Changes = $"Tarefa atualizada: {dto.Title ?? ""} {dto.Description ?? ""} {dto.Status?.ToString() ?? ""}"
        });

        _context.SaveChanges();
        return task;
    }

    public void DeleteTask(int taskId)
    {
        var task = _context.Tasks.FirstOrDefault(t => t.Id == taskId);
        if (task == null) throw new Exception("Tarefa não encontrada");
        _context.Tasks.Remove(task);
        _context.SaveChanges();
    }
}