using System.Collections.Concurrent;

namespace Todos;

internal sealed class TodoService
{
    private static readonly ConcurrentDictionary<Guid, Todo> _todos = new();

    public void AddTodo(Todo todo) => _todos.TryAdd(todo.Id, todo);

    public void CompleteTodo(Guid id)
    {
        if (!_todos.TryGetValue(id, out var todo))
            return;
        
        var completedTodo = todo with { IsDone = true };
        _todos.TryUpdate(id, completedTodo, todo);
    }
    
    public Todo[] GetTodos() => _todos.Values.ToArray();
}