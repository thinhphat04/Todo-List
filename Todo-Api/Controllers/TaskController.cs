using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Todo_Api.Dto;
using Todo_Api.Models;
using Todo_Api.Services;

namespace Todo_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }
        
        [HttpGet("GetAllTasks")]
        public async Task<IActionResult> GetAllTasks()
        {
            try
            {
                var tasks = await _taskService.GetAllTasksAsync();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
     
        // GET: api/task?search=...&pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetTasks([FromQuery] string search, 
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var tasks = await _taskService.GetTasksAsync(search, pageNumber, pageSize);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                // Nếu có lỗi ngoài ý muốn, trả về 500
                return StatusCode(500, ex.Message);
            }
        }


        // GET: api/task/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            try
            {
                var task = await _taskService.GetTaskByIdAsync(id);
                if (task == null)
                    return NotFound();
                return Ok(task);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        // POST: api/task
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskCreateDto dto)
        {
            try
            {
                var task = await _taskService.CreateTaskAsync(dto);
                return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/task/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskUpdateDto dto)
        {
            try
            {
                if (id != dto.Id)
                    return BadRequest("ID không khớp.");

                var result = await _taskService.UpdateTaskAsync(dto);
                if (!result)
                    return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        
        // PUT: api/task/{id}/status?isCompleted=true
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateTaskStatus(int id, [FromQuery] bool isCompleted)
        {
            try
            {
                var result = await _taskService.UpdateTaskStatusAsync(id, isCompleted);
                if (!result)
                {
                    // Không tìm thấy Task hoặc dependency chưa hoàn thành
                    return BadRequest("Không thể cập nhật trạng thái. " +
                                      "Có thể task không tồn tại hoặc dependency chưa hoàn thành.");
                }
                return NoContent(); // 204
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        // DELETE: api/task/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                var result = await _taskService.DeleteTaskAsync(id);
                if (!result)
                    return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/task/{id}/dependencies?dependencyId={dependencyId}
        [HttpPost("{id}/dependencies")]
        public async Task<IActionResult> AddDependency(int id, [FromQuery] int dependencyId)
        {
            try
            {
                var result = await _taskService.AddDependencyAsync(id, dependencyId);
                if (!result)
                    return BadRequest("Không thể thêm dependency (có thể do vòng lặp phụ thuộc hoặc task không tồn tại).");
                return Ok("Thêm dependency thành công.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/task/{id}/dependencies/{dependencyId}
        [HttpDelete("{id}/dependencies/{dependencyId}")]
        public async Task<IActionResult> RemoveDependency(int id, int dependencyId)
        {
            try
            {
                var result = await _taskService.RemoveDependencyAsync(id, dependencyId);
                if (!result)
                    return NotFound();
                return Ok("Xoá dependency thành công.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/task/{id}/dependencies
        [HttpGet("{id}/dependencies")]
        public async Task<IActionResult> GetDependencyGraph(int id)
        {
            try
            {
                var graph = await _taskService.GetDependencyGraphAsync(id);
                if (graph == null)
                    return NotFound();
                return Ok(graph);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
