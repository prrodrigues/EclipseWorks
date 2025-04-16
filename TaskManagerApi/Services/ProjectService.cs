using TaskManagerApi.Data;
using TaskManagerApi.DTOs;
using TaskManagerApi.Interfaces;
using TaskManagerApi.Models;
using Microsoft.EntityFrameworkCore;
using TaskStatus = TaskManagerApi.Models.TaskStatus;

namespace TaskManagerApi.Services;

public class ProjectService : IProjectService
{
    private readonly AppDbContext _context;
    public ProjectService(AppDbContext context) => _context = context;

    public List<Project> GetAll() => _context.Projects.ToList();

    public Project Create(CreateProjectDto dto)
    {
        var project = new Project { Name = dto.Name };
        _context.Projects.Add(project);
        _context.SaveChanges();
        return project;
    }

    public void Delete(int projectId)
    {
        var project = _context.Projects.Include(p => p.Tasks).FirstOrDefault(p => p.Id == projectId);
        if (project == null) throw new Exception("Projeto não encontrado");
        if (project.Tasks.Any(t => t.Status == TaskStatus.Pendente))
            throw new Exception("Conclua ou remova as tarefas pendentes antes de remover o projeto");
        _context.Projects.Remove(project);
        _context.SaveChanges();
    }
}