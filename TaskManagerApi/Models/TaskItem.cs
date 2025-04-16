namespace TaskManagerApi.Models;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public TaskStatus Status { get; set; } = TaskStatus.Pendente;
    public TaskPriority Priority { get; private set; }
    public int ProjectId { get; set; }
    public List<Comment> Comments { get; set; } = new();
    public List<TaskHistory> History { get; set; } = new();

    public TaskItem(TaskPriority priority) => Priority = priority;
}