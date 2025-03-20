using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Todo_Api.Models;

public class TaskDependency
{
    [Key]
    public int TaskId { get; set; }
    public Tasks Task { get; set; }

    public int DependencyId { get; set; }
    public Tasks Dependency { get; set; }
}