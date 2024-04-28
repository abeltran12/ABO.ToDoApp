using ABO.ToDoApp.Domain.Entities;
using ABO.ToDoApp.Domain.Models;
using ABO.ToDoApp.Domain.RequestFilters;

namespace ABO.ToDoApp.Domain.Repositories;

public interface ITodoListRepository
{
    Task<PagedList<TodoListSelect>> GetAll(TodoListParameters todoListParameters);
    Task<TodoList> GetByIdAsync(int id);
    Task CreateAsync(TodoList todoList);
    void Update(TodoList todoList);
    void Delete(TodoList todoList);
}
