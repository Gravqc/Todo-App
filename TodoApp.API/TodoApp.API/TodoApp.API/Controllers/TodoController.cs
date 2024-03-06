using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.API.Data;
using TodoApp.API.Models;

namespace TodoApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoDbContext _todoDbContext;

        public TodoController(TodoDbContext todoDbContext)
        {
            _todoDbContext = todoDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> getAllTodos()
        {
            var todos = await _todoDbContext.Todos.Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreatedDate).ToListAsync();
            return Ok(todos);
        }

        [HttpGet]
        [Route("deleted-todo")]
        public async Task<IActionResult> getDeletedTodos()
        {
            var todos = await _todoDbContext.Todos.Where(x => x.IsDeleted == true).OrderByDescending(x => x.CreatedDate).ToListAsync();
            return Ok(todos);
        }

        [HttpPost]
        public async Task<IActionResult> AddTodo(Todo todo)
        {
            todo.Id = Guid.NewGuid();
            _todoDbContext.Todos.Add(todo);
            await _todoDbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateTodo([FromRoute] Guid id, Todo todoUpdateRequest)
        {
            var todo = await _todoDbContext.Todos.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }
            todo.IsCompleted = todoUpdateRequest.IsCompleted;
            todo.CompletedDate = DateTime.Now;
            await _todoDbContext.SaveChangesAsync();
            return Ok(todo);
        }

        [HttpPut]
        [Route("deleted-todo/{id:guid}")]
        public async Task<IActionResult> undoDelete([FromRoute] Guid id, Todo undoDelTodo)
        {
            var todo = await _todoDbContext.Todos.FindAsync(id);

            if (todo == null)
            {
                return NotFound();
            }
            todo.DeletedDate = null;
            todo.IsDeleted = false;

            await _todoDbContext.SaveChangesAsync();
            return Ok(todo);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteTodo([FromRoute] Guid id)
        {
            var todo = await _todoDbContext.Todos.FindAsync(id);
            if(todo == null)
            {
                return NotFound();
            }
            todo.IsDeleted = true;
            todo.DeletedDate = DateTime.Now;
            await _todoDbContext.SaveChangesAsync();    
            return Ok(todo);
        }
    }
}
