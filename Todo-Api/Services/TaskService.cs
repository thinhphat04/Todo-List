using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo_Api.Data;
using Todo_Api.Dto;

using Todo_Api.Models;
using TasksEntity = Todo_Api.Models.Tasks;

namespace Todo_Api.Services;

    public class TaskService : ITaskService
    {
        private readonly TodoContext _context;

        public TaskService(TodoContext context)
        {
            _context = context;
        }
    
        public async Task<IEnumerable<Tasks>> GetAllTasksAsync()
        {
            return await _context.Tasks.AsNoTracking().ToListAsync();
        }

        public async Task<Tasks> CreateTaskAsync(TaskCreateDto dto)
        {
            var task = new TasksEntity
            {
                Title = dto.Title,
                Description = dto.Description,
                DueDate = dto.DueDate,
                Priority = dto.Priority,
                IsCompleted = false
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<IEnumerable<Tasks>> GetTasksAsync(string? search, int pageNumber, int pageSize)
        {
            IQueryable<Tasks> query = _context.Tasks.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(t => 
                    EF.Functions.Like(t.Title, $"%{search}%") || 
                    EF.Functions.Like(t.Description, $"%{search}%"));
            }

            return await query
                .OrderBy(t => t.DueDate)  
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }




        public async Task<Tasks> GetTaskByIdAsync(int id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        public async Task<bool> UpdateTaskAsync(TaskUpdateDto dto)
        {
            var task = await _context.Tasks.FindAsync(dto.Id);
            if (task == null)
                return false;

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.DueDate = dto.DueDate;
            task.Priority = dto.Priority;
           // task.IsCompleted = dto.IsCompleted;

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateTaskStatusAsync(int id, bool isCompleted)
        {
            var task = await _context.Tasks
                .Include(t => t.TaskDependencies)
                .ThenInclude(td => td.Dependency)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
                return false;

            if (isCompleted)
            {
                foreach (var depRelation in task.TaskDependencies)
                {
                    var depTask = depRelation.Dependency; 
                    if (depTask != null && !depTask.IsCompleted)
                    {
                        return false;
                    }
                }
            }

            task.IsCompleted = isCompleted;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _context.Tasks
                .Include(t => t.TaskDependencies)
                .Include(t => t.DependentOnTasks)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (task == null)
                return false;

            _context.TaskDependencies.RemoveRange(task.TaskDependencies);
            _context.TaskDependencies.RemoveRange(task.DependentOnTasks);
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddDependencyAsync(int taskId, int dependencyId)
        {
            if (taskId == dependencyId)
                return false;

            var task = await _context.Tasks.FindAsync(taskId);
            var dependencyTask = await _context.Tasks.FindAsync(dependencyId);
            if (task == null || dependencyTask == null)
                return false;

            // Kiểm tra vòng lặp dependency bằng thuật toán DFS
            if (await CreatesCycleAsync(dependencyTask, task))
                return false;

            var existing = await _context.TaskDependencies.FindAsync(taskId, dependencyId);
            if (existing != null)
                return false;

            var dependency = new TaskDependency
            {
                TaskId = taskId,
                DependencyId = dependencyId
            };

            _context.TaskDependencies.Add(dependency);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveDependencyAsync(int taskId, int dependencyId)
        {
            var dependency = await _context.TaskDependencies.FindAsync(taskId, dependencyId);
            if (dependency == null)
                return false;
            _context.TaskDependencies.Remove(dependency);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<TaskDependencyDto> GetDependencyGraphAsync(int taskId)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null)
                return null;
            return await BuildDependencyGraphAsync(task);
        }
        
        

        private async Task<TaskDependencyDto> BuildDependencyGraphAsync(TasksEntity task)
        {
            var dto = new TaskDependencyDto { Id = task.Id, Title = task.Title };
            var dependencies = await _context.TaskDependencies
                .Where(td => td.TaskId == task.Id)
                .Include(td => td.Dependency)
                .ToListAsync();

            foreach (var dep in dependencies)
            {
                var childDto = await BuildDependencyGraphAsync(dep.Dependency);
                dto.Dependencies.Add(childDto);
            }
            return dto;
        }

        // Kiểm tra vòng lặp dependency bằng thuật toán DFS
        private async Task<bool> CreatesCycleAsync(TasksEntity startTask, TasksEntity targetTask)
        {
            var visited = new HashSet<int>();
            return await DFSAsync(startTask, targetTask, visited);
        }

        private async Task<bool> DFSAsync(TasksEntity current, TasksEntity target, HashSet<int> visited)
        {
            if (current.Id == target.Id)
                return true;
            visited.Add(current.Id);
            var dependencies = await _context.TaskDependencies
                .Where(td => td.TaskId == current.Id)
                .Include(td => td.Dependency)
                .ToListAsync();
            foreach (var dep in dependencies)
            {
                if (!visited.Contains(dep.Dependency.Id))
                {
                    if (await DFSAsync(dep.Dependency, target, visited))
                        return true;
                }
            }
            return false;
        }
        
    }

