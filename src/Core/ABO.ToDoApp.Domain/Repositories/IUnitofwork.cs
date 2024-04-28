namespace ABO.ToDoApp.Domain.Repositories;

public interface IUnitofwork
{
    ITodoListRepository TodoListRepository { get; }
    ITodoItemRepository TodoItemRepository { get; }
    Task SaveAsync();
}
