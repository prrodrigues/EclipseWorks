using TaskManagerApi.DTOs;
using TaskManagerApi.Models;

namespace TaskManagerApi.Interfaces;

public interface ITaskService
{
    List<TaskItem> GetTasksByProject(int projectId);
    TaskItem CreateTask(int projectId, CreateTaskDto dto);
    TaskItem UpdateTask(int taskId, UpdateTaskDto dto, string modifiedBy);
    void DeleteTask(int taskId);
}