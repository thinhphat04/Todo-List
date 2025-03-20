namespace Todo_Api.Dto;

public class TaskDependencyDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public List<TaskDependencyDto> Dependencies { get; set; } = new List<TaskDependencyDto>();
}