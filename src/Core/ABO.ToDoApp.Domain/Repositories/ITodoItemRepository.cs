using ABO.ToDoApp.Domain.Entities;
using ABO.ToDoApp.Domain.Models;

namespace ABO.ToDoApp.Domain.Repositories;

public interface ITodoItemRepository
{
    Task<IEnumerable<TodoItemSelect>> GetAll(int idTodoList, string userId);
    Task<TodoItem> GetByIdAsync(int id, string userId);
    Task CreateAsync(TodoItem item);
    void Update(TodoItem item);
    void DeleteRange(List<TodoItem> items);
}
