using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace ToDoListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private static List<Task> _tasks = new List<Task>
        {
            new Task { Id = 1, Title = "Помыть посуду", IsCompleted = false },
            new Task { Id = 2, Title = "Сделать уборку", IsCompleted = true }
        };

        
        [HttpGet]
        public ActionResult<IEnumerable<Task>> GetTasks(bool? isCompleted = null)
        {
            if (isCompleted.HasValue)
            {
                return _tasks.Where(t => t.IsCompleted == isCompleted.Value).ToList();
            }
            return _tasks;
        }

        
        [HttpGet("{id}")]
        public ActionResult<Task> GetTask(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return NotFound();
            }
            return task;
        }

        
        [HttpPost]
        public ActionResult<Task> PostTask([FromBody] Task task)
        {
            // Проверка на пустой заголовок
            if (string.IsNullOrEmpty(task.Title))
            {
                return BadRequest("Title cannot be empty.");
            }

            // Генерация нового ID
            task.Id = _tasks.Max(t => t.Id) + 1;

            // Добавление задачи в список
            _tasks.Add(task);

            // Возвращение созданной задачи с кодом 201 (Created)
            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }

        
        [HttpPut("{id}")]
        public ActionResult PutTask(int id, [FromBody] Task task)
        {
            if (id != task.Id)
            {
                return BadRequest("Task ID mismatch.");
            }

            var existingTask = _tasks.FirstOrDefault(t => t.Id == id);
            if (existingTask == null)
            {
                return NotFound();
            }

            existingTask.Title = task.Title;
            existingTask.IsCompleted = task.IsCompleted;

            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public ActionResult DeleteTask(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            _tasks.Remove(task);
            return NoContent();
        }
    }

    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
    }
}