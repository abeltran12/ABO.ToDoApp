using ABO.ToDoApp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ABO.ToDoApp.Domain.Repositories;

public interface ITodoListRepository
{
    Task CreateAsync(TodoList todoList);

    Task<TodoList> GetByNameAsync(string name);

    IEnumerable<TodoList> GetAll();

    Task<TodoList> GetByIdAsync(int id);
}
