using Microsoft.AspNetCore.Mvc;
using TaskManagerApi.DTOs;
using TaskManagerApi.Interfaces;

namespace TaskManagerApi.Controllers;

[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _service;
    public TasksController(ITaskService service) => _service = service;

    [HttpGet("project/{projectId}")]
    public IActionResult GetByProject(int projectId) => Ok(_service.GetTasksByProject(projectId));

    [HttpPost("project/{projectId}")]
    public IActionResult Create(int projectId, CreateTaskDto dto) => Ok(_service.CreateTask(projectId, dto));

    [HttpPut("{taskId}")]
    public IActionResult Update(int taskId, UpdateTaskDto dto, [FromQuery] string user) =>
        Ok(_service.UpdateTask(taskId, dto, user));

    [HttpDelete("{taskId}")]
    public IActionResult Delete(int taskId)
    {
        _service.DeleteTask(taskId);
        return NoContent();
    }
}