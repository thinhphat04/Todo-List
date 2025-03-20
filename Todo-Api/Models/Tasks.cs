using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Todo_Api.Models;

public class Tasks
{
    public int Id { get; set; }
    public string Title { get; set; }    
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public int Priority { get; set; }
    public bool IsCompleted { get; set; }

    // Các task mà task này phụ thuộc vào
    public ICollection<TaskDependency> TaskDependencies { get; set; } = new List<TaskDependency>();

    // Các task phụ thuộc vào task này
    public ICollection<TaskDependency> DependentOnTasks { get; set; } = new List<TaskDependency>();
}

