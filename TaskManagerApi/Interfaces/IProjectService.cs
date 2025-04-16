using TaskManagerApi.DTOs;
using TaskManagerApi.Models;

namespace TaskManagerApi.Interfaces;

public interface IProjectService
{
    List<Project> GetAll();
    Project Create(CreateProjectDto dto);
    void Delete(int projectId);
}