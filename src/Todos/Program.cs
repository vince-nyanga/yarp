using Microsoft.AspNetCore.Mvc;
using Todos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<TodoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", TodosHandler.GetTodos)
    .WithName("GetTodos")
    .WithOpenApi();

app.MapPost("/", TodosHandler.AddTodo)
    .WithName("AddTodo")
    .WithOpenApi();

app.MapPatch("{id}/complete", TodosHandler.CompleteTodo)
    .WithName("CompleteTodo")
    .WithOpenApi();

app.Run();

internal sealed class TodosHandler
{
    public static Todo[] GetTodos(TodoService service)
        => service.GetTodos();

    public static IResult AddTodo(TodoService service, [FromBody] AddTodoRequest request)
    {
        service.AddTodo(new(Guid.NewGuid(), request.Title, false));
        return Results.Created();
    }

    public static IResult CompleteTodo(TodoService service, Guid id)
    {
        service.CompleteTodo(id);
        return Results.Ok();
    }
}

internal record AddTodoRequest(string Title);