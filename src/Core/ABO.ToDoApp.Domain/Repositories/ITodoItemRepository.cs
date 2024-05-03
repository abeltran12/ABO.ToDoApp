using ABO.ToDoApp.Domain.Entities;
using ABO.ToDoApp.Domain.Models;

namespace ABO.ToDoApp.Domain.Repositories;

public interface ITodoItemRepository
{
    Task<IEnumerable<TodoItemSelect>> GetAll(int idTodoList);
    Task<List<TodoItem>> GetAllForUpdate(int idTodoList);
    Task<int> GetTodoItemsCount(int idTodoList);
    Task<TodoItem> GetByIdAsync(int idTodoList, int id);
    Task CreateAsync(TodoItem item);
    void Update(TodoItem item);
    void UpdateAll(List<TodoItem> items);
}
