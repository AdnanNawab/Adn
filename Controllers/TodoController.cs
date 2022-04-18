
using System.Security.Claims;
using Adn.DTOs;
using Adn.Repositories;
using Adn.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Models;

namespace Adn.Controllers;

[ApiController]
[Authorize]
[Route("api/todo")]
public class TodoController : ControllerBase
{
    private readonly ILogger<TodoController> _logger;

    private readonly ITodoRepository _todo;

    public TodoController(ILogger<TodoController> logger, ITodoRepository todo)
    {
        _logger = logger;
        _todo = todo;

    }

    private int GetUserIdFromClaims(IEnumerable<Claim> claims)
    {
        return Convert.ToInt32(claims.Where(x => x.Type == TodoConstants.Id).First().Value);

    }

    [HttpPost]

    public async Task<ActionResult<TodoItem>> CreateTodo([FromBody] TodoItemCreateDTO Data)
    {
        var UserId = GetUserIdFromClaims(User.Claims);
        // var UserId = Convert.ToInt32(claims.Where(x => x.Type == TodoConstants.Id).First().Value);


        var ToCreateItem = new TodoItem
        {

            Title = Data.Title.Trim(),
            UserId = UserId,


        };

        var CreatedItem = await _todo.Create(ToCreateItem);

        return StatusCode(201, CreatedItem);


    }

    // [HttpPut]
    // public async Task<ActionResult<TodoItem>> UpdateTodo([FromBody] TodoItemUpdateDTO Data)
    // {
    //     var UserId = GetUserIdFromClaims(User.Claims);

    //     var existingItem = await _todo.GetById(Data.TodoId);

    //     if (existingItem is null)
    //         return NotFound();

    //     if (existingItem.UserId != UserId)

    //         return StatusCode(403, "You cannot Update other's TODO");

    //     var ToUpdateItem = existingItem with
    //     {
    //         Title = Data.Title is null ? existingItem.Title : Data.Title.Trim(),

    //         IsComplete = !Data.IsComplete.HasValue ? existingItem.IsComplete : Data.IsComplete.Value

    //     };

    //     await _todo.Update(ToUpdateItem);

    //     return NoContent();
    // }

    [HttpPut("{todo_id}")]
    public async Task<ActionResult> UpdateTodo([FromRoute] int todo_id,
[FromBody] TodoItemUpdateDTO Data)
    {
        var userId = GetUserIdFromClaims(User.Claims);

        var existingItem = await _todo.GetById(todo_id);

        if (existingItem is null)
            return NotFound();

        if (existingItem.UserId != userId)
            return StatusCode(404, "You cannot update other's TODO");

        var toUpdateItem = existingItem with
        {
            Title = Data.Title is null ? existingItem.Title : Data.Title.Trim(),
            IsComplete = !Data.IsComplete.HasValue ? existingItem.IsComplete : Data.IsComplete.Value,
        };

        await _todo.Update(toUpdateItem);

        return NoContent();
    }


    [HttpDelete("{todo_id}")]


    public async Task<ActionResult> DeleteTodo([FromRoute] int todo_id)

    {
        var userId = GetUserIdFromClaims(User.Claims);

        var existingItem = await _todo.GetById(todo_id);

        if (existingItem is null)
            return NotFound();


        if (existingItem.UserId != userId)

            return StatusCode(403, "You cannot delete other's TODO");




        await _todo.Delete(todo_id);

        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult> GetAllTodos()
    {
        var allTodo = await _todo.GetAll();
        return Ok(allTodo);

    }








}