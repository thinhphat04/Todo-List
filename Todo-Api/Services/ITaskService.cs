using Todo_Api.Dto;
using Todo_Api.Models;

namespace Todo_Api.Services;


    public interface ITaskService
    {
        //get all tasks
        Task<IEnumerable<Tasks>> GetAllTasksAsync();

        Task<Tasks> CreateTaskAsync(TaskCreateDto dto);
        Task<IEnumerable<Tasks>> GetTasksAsync(string search = null, int pageNumber = 1, int pageSize = 10);
        Task<Tasks> GetTaskByIdAsync(int id);
        Task<bool> UpdateTaskStatusAsync(int id, bool isCompleted);

        Task<bool> UpdateTaskAsync(TaskUpdateDto dto);
        Task<bool> DeleteTaskAsync(int id);
        Task<bool> AddDependencyAsync(int taskId, int dependencyId);
        Task<bool> RemoveDependencyAsync(int taskId, int dependencyId);
        Task<TaskDependencyDto> GetDependencyGraphAsync(int taskId);
    }

