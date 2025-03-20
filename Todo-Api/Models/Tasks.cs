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
    
    public ICollection<TaskDependency> TaskDependencies { get; set; } = new List<TaskDependency>();
    
    public ICollection<TaskDependency> DependentOnTasks { get; set; } = new List<TaskDependency>();
}

