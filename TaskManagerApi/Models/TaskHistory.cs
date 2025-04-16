namespace TaskManagerApi.Models;

public class TaskHistory
{
    public int Id { get; set; }
    public string Changes { get; set; } = string.Empty;
    public DateTime ModifiedAt { get; set; }
    public string ModifiedBy { get; set; } = string.Empty;
}