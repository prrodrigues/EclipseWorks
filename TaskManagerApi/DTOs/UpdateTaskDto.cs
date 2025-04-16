using TaskManagerApi.Models;
using TaskStatus = TaskManagerApi.Models.TaskStatus;

namespace TaskManagerApi.DTOs;

public class UpdateTaskDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public TaskStatus? Status { get; set; }
}